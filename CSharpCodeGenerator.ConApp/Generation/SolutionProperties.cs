//@QnSCodeCopy
//MdStart
using System;
using System.IO;
using System.Linq;
using CommonBase.Extensions;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class SolutionProperties
    {
        public static string SolutionExtension => ".sln";
        public static string ProjectExtension => ".csproj";
        public static string AssemblyInfo => "AssemblyInfo.cs";
        public static string GeneratedCodeFileName => "_GeneratedCode.cs";

        #region Project-postfixes
        public static string ContractsPostfix => ".Contracts";
        public static string LogicPostfix => ".Logic";
        public static string TransferPostfix => ".Transfer";
        public static string WebApiPostfix => ".WebApi";
        public static string AdaptersPostfix => ".Adapters";
        public static string AspMvcPostfix => ".AspMvc";
        #endregion Project-postfixes

        #region Folders
        public static string ModulesFolder => "Modules";
        public static string BusinessFolder => "Business";
        public static string PersistenceFolder => "Persistence";
        public static string WebApiControllersFolder => "Controllers";
        #endregion Folders

        #region ProjectNames
        public string ContractsProjectName => $"{SolutionName}{ContractsPostfix}";
        public string LogicProjectName => $"{SolutionName}{LogicPostfix}";
        public string TransferProjectName => $"{SolutionName}{TransferPostfix}";
        public string WebApiProjectName => $"{SolutionName}{WebApiPostfix}";
        public string AdaptersProjectName => $"{SolutionName}{AdaptersPostfix}";
        public string AspMvcProjectName => $"{SolutionName}{AspMvcPostfix}";
        #endregion ProjectNames

        #region Entities
        public static string EntitiesFolder => "Entities";
        public string EntitiesSubPath => EntitiesFolder;
        public string EntitiesModulesPath => Path.Combine(LogicPath, EntitiesSubPath, ModulesFolder);
        public string EntitiesBusinessPath => Path.Combine(LogicPath, EntitiesSubPath, BusinessFolder);
        public string EntitiesPersistencePath => Path.Combine(LogicPath, EntitiesSubPath, PersistenceFolder);
        public string EntitiesModulesFilePath => Path.Combine(EntitiesModulesPath, GeneratedCodeFileName);
        public string EntitiesBusinessFilePath => Path.Combine(EntitiesBusinessPath, GeneratedCodeFileName);
        public string EntitiesPersistenceFilePath => Path.Combine(EntitiesPersistencePath, GeneratedCodeFileName);
        #endregion Entities

        #region DataContext
        public static string DataContextFolder => "DataContext";
        public string DataContextSubPath => DataContextFolder;
        public string DbDataContextFolder => "Db";
        public string DataContextPersistencePath => Path.Combine(LogicPath, DataContextSubPath);
        public string DataContextPersistenceFilePath => Path.Combine(DataContextPersistencePath, GeneratedCodeFileName);
        #endregion DataContext

        #region Controllers
        public static string ControllersFolder => "Controllers";
        public string ControllersSubPath => ControllersFolder;
        public string ControllersPersistencePath => Path.Combine(LogicPath, ControllersSubPath, PersistenceFolder);
        public string ControllersBusinessPath => Path.Combine(LogicPath, ControllersSubPath, BusinessFolder);
        public string ControllersPersistenceFilePath => Path.Combine(ControllersPersistencePath, GeneratedCodeFileName);
        public string ControllersBusinessFilePath => Path.Combine(ControllersBusinessPath, GeneratedCodeFileName);
        #endregion Controllers

        #region Logic-Factory
        public string LogicFactoryPath => Path.Combine(LogicPath);
        public string LogicFactoryFilePath => Path.Combine(LogicFactoryPath, GeneratedCodeFileName);
        #endregion Logic-Factory

        #region Transfer
        public string TransferModulesPath => Path.Combine(TransferPath, ModulesFolder);
        public string TransferBusinessPath => Path.Combine(TransferPath, BusinessFolder);
        public string TransferPersistencePath => Path.Combine(TransferPath, PersistenceFolder);
        public string TransferModulesFilePath => Path.Combine(TransferModulesPath, GeneratedCodeFileName);
        public string TransferBusinessFilePath => Path.Combine(TransferBusinessPath, GeneratedCodeFileName);
        public string TransferPersistenceFilePath => Path.Combine(TransferPersistencePath, GeneratedCodeFileName);
        #endregion Transfer

        #region WebApi
        public string WebApiControllersSubPath => WebApiControllersFolder;
        public string WebApiControllersPath => Path.Combine(WebApiPath, WebApiControllersSubPath);
        public string WebApiControllersFilePath => Path.Combine(WebApiControllersPath, GeneratedCodeFileName);
        #endregion WebApi

        #region Adapters-Factory
        public string AdaptersFactoryPath => Path.Combine(AdaptersPath);
        public string AdaptersFactoryFilePath => Path.Combine(AdaptersFactoryPath, GeneratedCodeFileName);
        #endregion Adapters-Factory

        #region AspMvc
        public static string AspMvcModelsFolder => "Models";
        public string AspMvcModulesPath => Path.Combine(AspMvcPath, AspMvcModelsFolder, ModulesFolder);
        public string AspMvcBusinessPath => Path.Combine(AspMvcPath, AspMvcModelsFolder, BusinessFolder);
        public string AspMvcPersistencePath => Path.Combine(AspMvcPath, AspMvcModelsFolder, PersistenceFolder);
        public string AspMvcModulesFilePath => Path.Combine(AspMvcModulesPath, GeneratedCodeFileName);
        public string AspMvcBusinessFilePath => Path.Combine(AspMvcBusinessPath, GeneratedCodeFileName);
        public string AspMvcPersistenceFilePath => Path.Combine(AspMvcPersistencePath, GeneratedCodeFileName);
        #endregion AspMvc

        public static string[] BuildSubDirectories => new string[]
        {
            "//bin//", "//obj//", "\\bin\\", "\\obj\\", "//wwwroot//lib//", "\\wwwroot\\lib\\"
        };

        public virtual string SolutionPath { get; }
        public virtual string SolutionName => GetSolutionName(SolutionPath);
        public virtual string[] ProjectPaths => GetProjectPaths(SolutionPath);
        public virtual string[] ProjectNames => GetProjectNames(SolutionPath);
        public virtual string ContractsPath => ProjectPaths.FirstOrDefault(i => i.EndsWith(ContractsPostfix));
        public virtual string LogicPath => ProjectPaths.FirstOrDefault(i => i.EndsWith(LogicPostfix));
        public virtual string TransferPath => ProjectPaths.FirstOrDefault(i => i.EndsWith(TransferPostfix));
        public virtual string WebApiPath => ProjectPaths.FirstOrDefault(i => i.EndsWith(WebApiPostfix));
        public virtual string AdaptersPath => ProjectPaths.FirstOrDefault(i => i.EndsWith(AdaptersPostfix));
        public virtual string AspMvcPath => ProjectPaths.FirstOrDefault(i => i.EndsWith(AspMvcPostfix));

        protected SolutionProperties(string solutionPath)
        {
            solutionPath.CheckArgument(nameof(solutionPath));

            SolutionPath = solutionPath;
        }
        internal static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory.IndexOf($"{nameof(CSharpCodeGenerator)}", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory.Substring(0, endPos);
        }

        public static string GetProjectNameFromPath(string solutionPath, string projectPath)
        {
            solutionPath.CheckArgument(nameof(solutionPath));
            projectPath.CheckArgument(nameof(projectPath));

            string result = string.Empty;
            string fullNameWithoutSolutionPath = projectPath.Replace(solutionPath, string.Empty);
            string[] data = fullNameWithoutSolutionPath.Split("\\");

            if (data.Length > 0)
            {
                result = data[0];
            }
            return result;
        }
        public static string GetProjectNameFromFile(string solutionPath, string fullName)
        {
            solutionPath.CheckArgument(nameof(solutionPath));
            fullName.CheckArgument(nameof(fullName));

            string result = string.Empty;
            string fullNameWithoutSolutionPath = fullName.Replace(solutionPath, string.Empty);
            string[] data = fullNameWithoutSolutionPath.Split("\\")
                                                       .Where(d => d.IsNullOrEmpty() == false)
                                                       .ToArray();

            if (data.Length > 0)
            {
                result = data[0];
            }
            return result;
        }
        public static string GetProjectSubDirectoryFromFile(string solutionPath, string fullName)
        {
            solutionPath.CheckArgument(nameof(solutionPath));
            fullName.CheckArgument(nameof(fullName));

            bool found = false;
            string result = string.Empty;
            string projectName = GetProjectNameFromFile(solutionPath, fullName);
            string[] data = Path.GetDirectoryName(fullName).Split("\\")
                                                           .Where(d => d.IsNullOrEmpty() == false)
                                                           .ToArray();

            foreach (var item in data)
            {
                if (item.Equals(projectName, StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    found = true;
                }
                else if (found == true)
                {
                    result = Path.Combine(result, item);
                }
            }
            return result;
        }

        private static string GetSolutionName(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles().SingleOrDefault(f => f.Extension.Equals(".sln", StringComparison.CurrentCultureIgnoreCase));

            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
        private static string[] GetProjectNames(string solutionPath)
        {
            return GetProjectPaths(solutionPath).Select(p => new DirectoryInfo(p).Name).ToArray();
        }
        private static string[] GetProjectPaths(string solutionPath)
        {
            var files = new DirectoryInfo(solutionPath)
                .GetFiles("*.*", SearchOption.AllDirectories)
                .Where(f => f.Extension.Equals(ProjectExtension, StringComparison.CurrentCultureIgnoreCase))
                .Select(f => Path.GetDirectoryName(f.FullName));

            return files.ToArray();
        }

        internal static SolutionProperties Create()
        {
            return new SolutionProperties(GetCurrentSolutionPath());
        }
        internal static SolutionProperties Create(string solutionPath)
        {
            return new SolutionProperties(solutionPath);
        }
    }
}
//MdEnd
