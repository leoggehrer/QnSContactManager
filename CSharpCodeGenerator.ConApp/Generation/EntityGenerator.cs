//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using CommonBase.Extensions;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class EntityGenerator : ClassGenerator
    {
        protected EntityGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static EntityGenerator Create(SolutionProperties solutionProperties)
        {
            return new EntityGenerator(solutionProperties);
        }

        public string EntityNameSpace => $"{SolutionProperties.LogicProjectName}.{SolutionProperties.EntitiesFolder}";

        public string CreateNameSpace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"{EntityNameSpace}.{Generator.GetSubNamespaceFromInterface(type)}";
        }

        private bool CanCreateEntity(Type type)
        {
            bool create = true;

            CanCreateEntity(type, ref create);
            return create;
        }
        partial void CanCreateEntity(Type type, ref bool create);
        private bool CanCreateProperty(Type type, string propertyName)
        {
            bool create = true;

            CanCreateProperty(type, propertyName, ref create);
            return create;
        }
        partial void CanCreateProperty(Type type, string propertyName, ref bool create);
        partial void CreateEntityAttributes(Type type, List<string> codeLines);

        private IEnumerable<string> CreateEntityFromInterface(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();
            var entityName = CreateEntityNameFromInterface(type);

            CreateEntityAttributes(type, result);
            result.Add($"partial class {entityName} : {type.FullName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(entityName));
            result.AddRange(CreatePartialConstrutor("public", entityName));
            foreach (var item in GetPublicProperties(type).Where(p => p.DeclaringType.Name.Equals("IIdentifiable") == false
                                                                   && CanCreateProperty(type, p.Name)))
            {
                result.AddRange(CreatePartialProperty(item));
            }
            result.AddRange(CreateCopyProperties(type));
            result.AddRange(CreateEquals(type));
            result.AddRange(CreateGetHashCode(type));
            result.Add("}");
            return result;
        }

        public IEnumerable<string> CreateModulesEntities()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.ModuleTypes)
            {
                if (CanCreateEntity(type))
                {
                    result.AddRange(EnvelopeWithANamespace(CreateEntityFromInterface(type), CreateNameSpace(type), "using System;"));
                    result.AddRange(EnvelopeWithANamespace(CreateModuleEntity(type), CreateNameSpace(type)));
                }
            }
            return result;
        }
        private IEnumerable<string> CreateModuleEntity(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();

            result.Add($"partial class {CreateEntityNameFromInterface(type)} : ModuleObject");
            result.Add("{");
            result.Add("}");
            return result;
        }

        public IEnumerable<string> CreateBusinesssEntities()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.BusinessTypes)
            {
                if (CanCreateEntity(type))
                {
                    result.AddRange(EnvelopeWithANamespace(CreateEntityFromInterface(type), CreateNameSpace(type), "using System;"));
                    result.AddRange(EnvelopeWithANamespace(CreateBusinessEntity(type), CreateNameSpace(type)));
                }
            }
            return result;
        }
        private IEnumerable<string> CreateBusinessEntity(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();

            result.Add($"partial class {CreateEntityNameFromInterface(type)} : IdentityObject");
            result.Add("{");
            result.Add("}");
            return result;
        }

        public IEnumerable<string> CreatePersistenceEntities()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);
            var persistenceTypes = contractsProject.PersistenceTypes;

            foreach (var type in persistenceTypes)
            {
                if (CanCreateEntity(type))
                {
                    string nameSpace = CreateNameSpace(type);

                    result.Add($"//Entity for: {type.Name}");
                    result.AddRange(EnvelopeWithANamespace(CreateEntityFromInterface(type), nameSpace, "using System;"));
                    result.AddRange(EnvelopeWithANamespace(CreatePersistenceEntity(type), nameSpace));
                    result.AddRange(EnvelopeWithANamespace(CreateEntityToEntityFromContracts(type, persistenceTypes, null), nameSpace));
                }
            }
            return result;
        }
        private IEnumerable<string> CreatePersistenceEntity(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();

            result.Add($"partial class {CreateEntityNameFromInterface(type)} : IdentityObject");
            result.Add("{");
            result.Add("}");
            return result;
        }

        /// <summary>
        /// Diese Methode erstellt den Programmcode der Beziehungen zwischen den Entitaeten aus den Schnittstellen-Typen.
        /// </summary>
        /// <param name="type">Der Schnittstellen-Typ.</param>
        /// <param name="types">Die Schnittstellen-Typen.</param>
        /// <param name="mapPropertyName">Ein Lambda-Ausdruck zum konvertieren des Eigenschaftsnamen.</param>
        /// <returns>Die Entitaet als Text.</returns>
        public static IEnumerable<string> CreateEntityToEntityFromContracts(Type type, IEnumerable<Type> types, Func<string, string> mapPropertyName)
        {
            type.CheckArgument(nameof(type));
            types.CheckArgument(nameof(types));

            var result = new List<string>();
            var typeName = Generator.CreateEntityNameFromInterface(type);

            result.Add($"partial class {typeName}");
            result.Add("{");

            foreach (var other in types)
            {
                var otherName = Generator.CreateEntityNameFromInterface(other);

                foreach (var pi in other.GetProperties())
                {
                    if (pi.Name.Equals($"{typeName}Id"))
                    {
                        var otherFullName = Generator.CreateEntityFullNameFromInterface(other);
                        var propertyName = mapPropertyName != null ? mapPropertyName(otherName + "s") : otherName + "s";

                        result.Add(($"public System.Collections.Generic.ICollection<{otherFullName}> {propertyName} " + "{ get; set; }"));
                    }
                }
            }
            foreach (var pi in type.GetProperties())
            {
                foreach (var other in types)
                {
                    var otherName = Generator.CreateEntityNameFromInterface(other);

                    if (pi.Name.Equals($"{otherName}Id"))
                    {
                        var otherFullName = Generator.CreateEntityFullNameFromInterface(other);
                        var propertyName = mapPropertyName != null ? mapPropertyName(otherName) : otherName;

                        result.Add(($"public {otherFullName} {propertyName} " + "{ get; set; }"));
                    }
                }
            }
            result.Add("}");
            return result;
        }
    }
}
//MdEnd
