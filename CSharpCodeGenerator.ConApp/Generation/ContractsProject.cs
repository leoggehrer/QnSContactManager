//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using CommonBase.Extensions;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class ContractsProject
    {
        private ContractsProject()
        {

        }
        public static ContractsProject Create(SolutionProperties solutionProperties)
        {
            solutionProperties.CheckArgument(nameof(solutionProperties));

            ContractsProject result = new ContractsProject();

            result.SolutionProperties = solutionProperties;
            return result;
        }
        public SolutionProperties SolutionProperties { get; private set; }
        public string ProjectName => $"{SolutionProperties.SolutionName}.Contracts";
        public string ProjectPath => Path.Combine(SolutionProperties.SolutionPath, ProjectName);

        public string _contractsPath = null;
        public string ContractsPath
        {
            get
            {
                if (_contractsPath.IsNullOrEmpty())
                {
                    var binPath = Path.Combine(ProjectPath, "bin");

                    if (Directory.Exists(binPath))
                    {
                        var fileName = $"{ProjectName}.dll";
                        var fileInfos = new DirectoryInfo(binPath).GetFiles(fileName, SearchOption.AllDirectories)
                                                                  .Where(f => f.FullName.EndsWith(fileName))
                                                                  .OrderByDescending(f => f.LastWriteTime);

                        var fileInfo = fileInfos.FirstOrDefault();

                        if (fileInfo != null)
                        {
                            _contractsPath = Path.GetDirectoryName(fileInfo.FullName);
                            Logger(fileInfo.FullName);
                        }
                        else
                        {
                            ErrorLogger("No interfaces available.");
                        }
                    }
                    else
                    {
                        ErrorLogger("No interfaces available.");
                    }
                }
                return _contractsPath;
            }
            set => _contractsPath = value;
        }
        public string ContractsFilePath
        {
            get
            {
                string result = string.Empty;

                if (ContractsPath.IsNullOrEmpty() == false)
                {
                    var fileName = $"{ProjectName}.dll";

                    result = Path.Combine(ContractsPath, fileName);
                }
                return result;
            }
        }

        public IEnumerable<Type> InterfaceTypes
        {
            get
            {
                List<Type> result = new List<Type>();

                if (ContractsFilePath.HasContent())
                {
                    result.AddRange(AssemblyLoadContext
                                        .Default
                                        .LoadFromAssemblyPath(ContractsFilePath)
                                        .GetTypes()
                                        .Where(t => t.IsInterface));
                }
                return result;
            }
        }
        public IEnumerable<Type> ModuleTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(".Modules."));
            }
        }
        public IEnumerable<Type> BusinessTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(".Business."));
            }
        }
        public IEnumerable<Type> PersistenceTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(".Persistence."));
            }
        }

        #region Helpers
        private static void Logger(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
        private static void ErrorLogger(string message)
        {
            var saveColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ForegroundColor = saveColor;
            Console.WriteLine(message);
        }
        #endregion Helpers
    }
}
//MdEnd
