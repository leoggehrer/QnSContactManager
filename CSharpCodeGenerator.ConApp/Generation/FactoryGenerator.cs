//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using CommonBase.Extensions;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class FactoryGenerator : ClassGenerator
    {
        protected FactoryGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static FactoryGenerator Create(SolutionProperties solutionProperties)
        {
            return new FactoryGenerator(solutionProperties);
        }

        #region General
        //private bool CanCreate(string generationName, Type type)
        //{
        //    bool create = true;

        //    CanCreateController(generationName, type, ref create);
        //    return create;
        //}
        //partial void CanCreateController(string generationName, Type type, ref bool create);
        private bool CanCreateLogicAccess(Type type)
        {
            bool create = true;

            CanCreateLogicAccess(type, ref create);
            return create;
        }
        partial void CanCreateLogicAccess(Type type, ref bool create);
        private bool CanCreateAdapterAccess(Type type)
        {
            bool create = true;

            CanCreateAdapterAccess(type, ref create);
            return create;
        }
        partial void CanCreateAdapterAccess(Type type, ref bool create);
        #endregion General

        #region Logic
        public string LogicNameSpace => $"{SolutionProperties.LogicProjectName}";
        public string CreateLogicNameSpace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"{LogicNameSpace}.{Generator.GetSubNamespaceFromInterface(type)}";
        }
        public IEnumerable<string> CreateLogicFactory()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);
            var types = contractsProject.PersistenceTypes.Union(contractsProject.BusinessTypes);
            var first = true;

            result.Add("public static partial class Factory");
            result.Add("{");
            result.Add("public static Contracts.Client.IControllerAccess<I> Create<I>() where I : Contracts.IIdentifiable");
            result.Add("{");
            result.Add("Contracts.Client.IControllerAccess<I> result = null;");
            foreach (var type in types.Where(t => CanCreateLogicAccess(t)))
            {
                string entityName = CreateEntityNameFromInterface(type);
                string controllerNameSpace = $"Controllers.{GetSubNamespaceFromInterface(type)}";

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new {controllerNameSpace}.{entityName}Controller(CreateContext()) as Contracts.Client.IControllerAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("return result;");
            result.Add("}");

            result.Add("public static Contracts.Client.IControllerAccess<I> Create<I>(object sharedController) where I : Contracts.IIdentifiable");
            result.Add("{");
            result.Add("Contracts.Client.IControllerAccess<I> result = null;");
            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t)))
            {
                string entityName = CreateEntityNameFromInterface(type);
                string controllerNameSpace = $"Controllers.{GetSubNamespaceFromInterface(type)}";

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new {controllerNameSpace}.{entityName}Controller(sharedController as Controllers.ControllerObject) as Contracts.Client.IControllerAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("return result;");
            result.Add("}");

            result.Add("public static Contracts.Client.IControllerAccess<I> Create<I>(string sessionToken) where I : Contracts.IIdentifiable");
            result.Add("{");
            result.Add("Contracts.Client.IControllerAccess<I> result = null;");
            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t)))
            {
                string entityName = CreateEntityNameFromInterface(type);
                string controllerNameSpace = $"Controllers.{GetSubNamespaceFromInterface(type)}";

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new {controllerNameSpace}.{entityName}Controller(CreateContext()) " + "{ SessionToken = sessionToken } as Contracts.Client.IControllerAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("return result;");
            result.Add("}");
            result.Add("}");

            return EnvelopeWithANamespace(result, LogicNameSpace);
        }
        #endregion Logic

        #region Adapter
        public string AdapterNameSpace => $"{SolutionProperties.AdaptersProjectName}";
        public string CreateControllerNameSpace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"Controllers.{GetSubNamespaceFromInterface(type)}";
        }
        public string CreateTransferNameSpace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"Transfer.{GetSubNamespaceFromInterface(type)}";
        }
        public IEnumerable<string> CreateAdapterFactory()
        {
            List<string> result = new List<string>();
            ContractsProject contractsProject = ContractsProject.Create(SolutionProperties);
            var types = contractsProject.PersistenceTypes.Union(contractsProject.BusinessTypes);
            var first = true;

            result.Add("public static partial class Factory");
            result.Add("{");
            result.Add("public static Contracts.Client.IAdapterAccess<I> Create<I>() where I : Contracts.IIdentifiable");
            result.Add("{");
            result.Add("Contracts.Client.IAdapterAccess<I> result = null;");
            result.Add("if (Adapter == AdapterType.Controller)");
            result.Add("{");
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                string entityName = CreateEntityNameFromInterface(type);
                string controllerNameSpace = CreateControllerNameSpace(type);

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Controller.GenericControllerAdapter<{type.FullName}>() as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("else if (Adapter == AdapterType.Service)");
            result.Add("{");

            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                string modelName = CreateEntityNameFromInterface(type);
                string modelNameSpace = CreateTransferNameSpace(type);

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Service.GenericServiceAdapter<{type.FullName}, {modelNameSpace}.{modelName}>(BaseUri, \"{modelName}\")");
                result.Add(" as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("return result;");
            result.Add("}");

            result.Add("public static Contracts.Client.IAdapterAccess<I> Create<I>(string sessionToken) where I : Contracts.IIdentifiable");
            result.Add("{");
            result.Add("Contracts.Client.IAdapterAccess<I> result = null;");
            result.Add("if (Adapter == AdapterType.Controller)");
            result.Add("{");

            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                string entityName = CreateEntityNameFromInterface(type);
                string controllerNameSpace = CreateControllerNameSpace(type);

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Controller.GenericControllerAdapter<{type.FullName}>(sessionToken) as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("else if (Adapter == AdapterType.Service)");
            result.Add("{");
            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                string modelName = CreateEntityNameFromInterface(type);
                string modelNameSpace = CreateTransferNameSpace(type);

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Service.GenericServiceAdapter<{type.FullName}, {modelNameSpace}.{modelName}>(sessionToken, BaseUri, \"{modelName}\") as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("return result;");
            result.Add("}");

            result.Add("}");
            return EnvelopeWithANamespace(result, AdapterNameSpace);
        }
        #endregion
    }
}
//MdEnd
