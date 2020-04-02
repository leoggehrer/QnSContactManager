//@QnSCodeCopy
//MdStart
using System.Collections.Generic;
using CommonBase.Extensions;

namespace QnSContactManager.Logic.Configuration
{
    public static class Configurator
    {
        static Configurator()
        {
            Dictionary = new Dictionary<string, string>();
        }
        private static Dictionary<string, string> Dictionary { get; }

        public static bool Contains(string key)
        {
            return Dictionary.ContainsKey(key);
        }

        public static void Set(string key, string value)
        {
            key.CheckArgument(nameof(key));

            if (Dictionary.ContainsKey(key) == false)
            {
                Dictionary.Add(key, value);
            }
            else
            {
                Dictionary[key] = value;
            }
        }

        public static string Get(string key)
        {
            return Dictionary[key];
        }
    }
}
//MdEnd
