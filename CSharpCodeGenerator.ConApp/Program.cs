//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpCodeGenerator.ConApp.Generation;

namespace CSharpCodeGenerator.ConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SolutionProperties solutionProperties = SolutionProperties.Create();
            EntityGenerator entityGenerator = EntityGenerator.Create(solutionProperties);
            DataContextGenerator dataContextGenerator = DataContextGenerator.Create(solutionProperties);
            ControllerGenerator controllerGenerator = ControllerGenerator.Create(solutionProperties);
            FactoryGenerator factoryGenerator = FactoryGenerator.Create(solutionProperties);
            TransferGenerator transferGenerator = TransferGenerator.Create(solutionProperties);
            AspMvcGenerator aspMvcGenerator = AspMvcGenerator.Create(solutionProperties);

            List<string> lines = new List<string>();

            Console.WriteLine("Create Modules-Entities...");
            lines.Clear();
            lines.AddRange(entityGenerator.CreateModulesEntities());
            WriteAllLines(solutionProperties.EntitiesModulesFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Business-Entities...");
            lines.Clear();
            lines.AddRange(entityGenerator.CreateBusinesssEntities());
            WriteAllLines(solutionProperties.EntitiesBusinessFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Persistence-Entities...");
            lines.Clear();
            lines.AddRange(entityGenerator.CreatePersistenceEntities());
            WriteAllLines(solutionProperties.EntitiesPersistenceFilePath, FormatCSharp(lines));

            Console.WriteLine("Create DataContext-DbContext...");
            lines.Clear();
            lines.AddRange(dataContextGenerator.CreateDbContext());
            WriteAllLines(solutionProperties.DataContextPersistenceFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Persistence-Controllers...");
            lines.Clear();
            lines.AddRange(controllerGenerator.CreatePersistenceControllers());
            WriteAllLines(solutionProperties.ControllersPersistenceFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Business-Controllers...");
            lines.Clear();
            lines.AddRange(controllerGenerator.CreateBusinessControllers());
            WriteAllLines(solutionProperties.ControllersBusinessFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Factory...");
            lines.Clear();
            lines.AddRange(factoryGenerator.CreateLogicFactory());
            WriteAllLines(solutionProperties.LogicFactoryFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Transfer-Modules...");
            lines.Clear();
            lines.AddRange(transferGenerator.CreateModulesTransfers());
            WriteAllLines(solutionProperties.TransferModulesFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Transfer-Business...");
            lines.Clear();
            lines.AddRange(transferGenerator.CreateBusinessTransfers());
            WriteAllLines(solutionProperties.TransferBusinessFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Transfer-Persistence...");
            lines.Clear();
            lines.AddRange(transferGenerator.CreatePersistenceTransfers());
            WriteAllLines(solutionProperties.TransferPersistenceFilePath, FormatCSharp(lines));

            Console.WriteLine("Create WebApi-Controllers...");
            lines.Clear();
            lines.AddRange(controllerGenerator.CreateWebApiControllers());
            WriteAllLines(solutionProperties.WebApiControllersFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Adapters...");
            lines.Clear();
            lines.AddRange(factoryGenerator.CreateAdapterFactory());
            WriteAllLines(solutionProperties.AdaptersFactoryFilePath, FormatCSharp(lines));

            Console.WriteLine("Create AspMvc-Modules...");
            lines.Clear();
            lines.AddRange(aspMvcGenerator.CreateModulesModels());
            WriteAllLines(solutionProperties.AspMvcModulesFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Business-AspMvc...");
            lines.Clear();
            lines.AddRange(aspMvcGenerator.CreateBusinessModels());
            WriteAllLines(solutionProperties.AspMvcBusinessFilePath, FormatCSharp(lines));

            Console.WriteLine("Create Persistence-AspMvc...");
            lines.Clear();
            lines.AddRange(aspMvcGenerator.CreatePersistenceModels());
            WriteAllLines(solutionProperties.AspMvcPersistenceFilePath, FormatCSharp(lines));
        }

        private static IEnumerable<string> FormatCSharp(IEnumerable<string> source)
        {
            return Extensions.CSharpFormatterExtensions.FormatCSharpCode(source);
        }
        private static void WriteAllLines(string filePath, IEnumerable<string> lines)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (lines.Any() == false)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            else if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            if (lines.Any())
            {
                File.WriteAllLines(filePath, lines);
            }
        }
    }
}
//MdEnd
