//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using CommonBase.Extensions;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class AspMvcGenerator : ClassGenerator
    {
        protected AspMvcGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static AspMvcGenerator Create(SolutionProperties solutionProperties)
        {
            return new AspMvcGenerator(solutionProperties);
        }

        public string AspMvcNameSpace => $"{SolutionProperties.AspMvcProjectName}.{SolutionProperties.AspMvcModelsFolder}";

        public string CreateNameSpace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"{AspMvcNameSpace}.{Generator.GetSubNamespaceFromInterface(type)}";
        }
        private bool CanCreate(Type type)
        {
            bool create = true;

            CanCreateModel(type, ref create);
            return create;
        }
        partial void CanCreateModel(Type type, ref bool create);
        partial void CreateModelAttributes(Type type, List<string> codeLines);
        partial void CreateModelPropertyAttributes(Type type, string propertyName, List<string> codeLines);

        public IEnumerable<string> CreateModelFromInterface(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();
            var entityName = CreateEntityNameFromInterface(type);

            CreateModelAttributes(type, result);
            result.Add($"public partial class {entityName} : {type.FullName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(entityName));
            result.AddRange(CreatePartialConstrutor("public", entityName));
            foreach (var item in GetPublicProperties(type).Where(p => p.DeclaringType.Name.Equals("IIdentifiable") == false))
            {
                CreateModelPropertyAttributes(type, item.Name, result);
                result.AddRange(CreatePartialProperty(item));
            }
            result.AddRange(CreateCopyProperties(type));
            result.Add("}");
            return result;
        }

        public IEnumerable<string> CreateModulesModels()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.ModuleTypes)
            {
                if (CanCreate(type))
                {
                    result.AddRange(EnvelopeWithANamespace(CreateModelFromInterface(type), CreateNameSpace(type)));
                    result.AddRange(EnvelopeWithANamespace(CreateModuleModel(type), CreateNameSpace(type)));
                }
            }
            return result;
        }
        public IEnumerable<string> CreateBusinessModels()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.BusinessTypes)
            {
                if (CanCreate(type))
                {
                    result.AddRange(EnvelopeWithANamespace(CreateModelFromInterface(type), CreateNameSpace(type)));
                    result.AddRange(EnvelopeWithANamespace(CreateBusinessModel(type), CreateNameSpace(type)));
                }
            }
            return result;
        }
        public IEnumerable<string> CreatePersistenceModels()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.PersistenceTypes)
            {
                if (CanCreate(type))
                {
                    result.AddRange(EnvelopeWithANamespace(CreateModelFromInterface(type), CreateNameSpace(type)));
                    result.AddRange(EnvelopeWithANamespace(CreatePersistenceModel(type), CreateNameSpace(type)));
                }
            }
            return result;
        }
        private IEnumerable<string> CreateBusinessModel(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();

            result.Add($"partial class {CreateEntityNameFromInterface(type)} : Models.IdentityModel");
            result.Add("{");
            result.Add("}");
            return result;
        }
        private IEnumerable<string> CreatePersistenceModel(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();

            result.Add($"partial class {CreateEntityNameFromInterface(type)} : Models.IdentityModel");
            result.Add("{");
            result.Add("}");
            return result;
        }
        private IEnumerable<string> CreateModuleModel(Type type)
        {
            type.CheckArgument(nameof(type));

            List<string> result = new List<string>();

            result.Add($"partial class {CreateEntityNameFromInterface(type)} : Models.ModelObject");
            result.Add("{");
            result.Add("}");
            return result;
        }
    }
}
//MdEnd
