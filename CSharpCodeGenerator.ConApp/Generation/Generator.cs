//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonBase.Extensions;
using CSharpCodeGenerator.ConApp.Extensions;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class Generator
    {
        public SolutionProperties SolutionProperties { get; private set; }
        public Generator(SolutionProperties solutionProperties)
        {
            solutionProperties.CheckArgument(nameof(solutionProperties));

            SolutionProperties = solutionProperties;
        }

        public static string EntitiesLabel => "Entities";
        public static string ModulesLabel => "Modules";
        public static string BusinessLabel => "Business";
        public static string PersistenceLabel => "Persistence";

        internal static IEnumerable<string> EnvelopeWithANamespace(IEnumerable<string> source, string nameSpace, params string[] usings)
        {
            List<string> result = new List<string>();

            if (nameSpace.HasContent())
            {
                result.Add($"namespace {nameSpace}");
                result.Add("{");
                result.AddRange(usings);
            }
            result.AddRange(source);
            if (nameSpace.HasContent())
            {
                result.Add("}");
            }
            return result;
        }

        /// <summary>
        /// Diese Methode erstellt den Programmcode der Beziehungen zwischen den Entitaeten aus den Schnittstellen-Typen.
        /// </summary>
        /// <param name="type">Der Schnittstellen-Typ.</param>
        /// <param name="types">Die Schnittstellen-Typen.</param>
        /// <param name="mapPropertyName">Ein Lambda-Ausdruck zum konvertieren des Eigenschaftsnamen.</param>
        /// <returns>Die referenzierten Typen.</returns>
        internal static IEnumerable<Type> CreateTypeToTypeFromContracts(Type type, IEnumerable<Type> types, Func<string, string> mapPropertyName)
        {
            type.CheckArgument(nameof(type));
            types.CheckArgument(nameof(types));

            var result = new List<Type>();
            var typeName = Generator.CreateEntityNameFromInterface(type);

            foreach (var other in types)
            {
                var otherName = Generator.CreateEntityNameFromInterface(other);

                foreach (var pi in other.GetProperties())
                {
                    if (pi.Name.Equals($"{typeName}Id"))
                    {
                        result.Add(other);
                    }
                }
            }
            return result;
        }

        #region Helpers
        /// <summary>
        /// Diese Methode konvertiert den Eigenschaftstyp in eine Zeichenfolge.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <returns>Der Eigenschaftstyp als Zeichenfolge.</returns>
        internal static string GetPropertyType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.GetCodeDefinition();
        }

        /// <summary>
        /// Diese Methode ermittelt den Solutionname aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Schema der Entitaet.</returns>
        internal static string GetSolutionNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;
            var data = type.GetTypeInfo().Namespace.Split('.');

            if (data.Length > 0)
            {
                result = data[0];
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Modulenamen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Schema der Entitaet.</returns>
        internal static string GetModuleNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;
            var data = type.GetTypeInfo().Namespace.Split('.');
            var idx = data.FindIndex(i => i.Equals(BusinessLabel));

            if (idx == -1)
            {
                idx = data.FindIndex(i => i.Equals(PersistenceLabel));
            }

            if (idx == -1)
            {
                idx = data.FindIndex(i => i.Equals(ModulesLabel));
            }

            if (idx > -1)
            {
                var separator = string.Empty;

                for (var i = idx + 1; i < data.Length; i++)
                {
                    result = $"{result}{separator}{data[i]}";
                    separator = ".";
                }
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Teilnamensraum aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Schema der Entitaet.</returns>
        internal static string GetSubNamespaceFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;
            var data = type.GetTypeInfo().Namespace.Split('.');
            var idx = data.FindIndex(i => i.Equals(BusinessLabel));

            for (var i = 2; i < data.Length; i++)
            {
                if (string.IsNullOrEmpty(result))
                {
                    result = $"{data[i]}";
                }
                else
                {
                    result = $"{result}.{data[i]}";
                }
            }
            return result;
        }

        /// <summary>
        /// Diese Methode ermittelt den Schema Namen aus seinem Schnittstellen Typ und seinem Namespace.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Schema der Entitaet.</returns>
        internal static string GetSchemaNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.GetTypeInfo().IsInterface)
            {
                var data = type.GetTypeInfo().Namespace.Split('.');

                if (data.Contains(PersistenceLabel))
                {
                    if (data[^1].Equals(PersistenceLabel) == false)
                    {
                        result = data[^1];
                    }
                    else
                    {
                        result = "dbo";
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Entity Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>MethodName der Entitaet.</returns>
        public static string CreateEntityNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.GetTypeInfo().IsInterface)
            {
                result = type.Name.Substring(1);
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Entity Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>MethodName der Entitaet.</returns>
        public static string CreateEntityFullNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.GetTypeInfo().IsInterface)
            {
                var entityName = type.Name.Substring(1);

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".Logic.Entities");
            }
            return result;
        }

        /// <summary>
        /// Diese Methode ermittelt den Feldnamen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <param name="preFix">Prefix der dem Namen vorgestellt ist.</param>
        /// <param name="postfix">Postfix der dem Namen nachgestellt ist.</param>
        /// <returns>Der Feldname als Zeichenfolge.</returns>
        public static string CreateFieldName(Type type, string preFix = "", string postFix = "")
        {
            CheckInterfaceType(type);

            return $"{preFix}{char.ToLower(type.Name.Skip(1).First())}{type.Name.Substring(2)}{postFix}";
        }
        /// <summary>
        /// Diese Methode ermittelt den Feldnamen der Eigenschaft.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <param name="prefix">Prefix der dem Namen vorgestellt ist.</param>
        /// <returns>Der Feldname als Zeichenfolge.</returns>
        public static string CreateFieldName(PropertyInfo propertyInfo, string prefix)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            return $"{prefix}{char.ToLower(propertyInfo.Name.First())}{propertyInfo.Name.Substring(1)}";
        }

        /// <summary>
        /// Diese Methode ermittelt den Feldnamen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <param name="postfix">Postfix der dem Namen nachgestellt ist.</param>
        /// <returns>Der Feldname als Zeichenfolge.</returns>
        public static string CreatePropertyName(Type type, string postFix = "")
        {
            CheckInterfaceType(type);

            return $"{type.Name.Substring(1)}{postFix}";
        }
        /// <summary>
        /// Diese Methode ueberprueft, ob der Typ ein Schnittstellen-Typ ist. Wenn nicht,
        /// dann wirft die Methode eine Ausnahme.
        /// </summary>
        /// <param name="type">Der zu ueberpruefende Typ.</param>
        internal static void CheckInterfaceType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.GetTypeInfo().IsInterface == false)
                throw new ArgumentException($"The parameter '{nameof(type)}' must be an interface.");
        }

        internal static PropertyInfo[] GetPublicProperties(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.GetTypeInfo().IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var queue = new Queue<Type>();
                var considered = new List<Type>
                {
                    type
                };
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(
                BindingFlags.FlattenHierarchy
                | BindingFlags.Public
                | BindingFlags.Instance);
        }
        #endregion Helpers
    }
}
//MdEnd
