//@QnSCodeCopy
//MdStart
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CommonBase.Extensions
{
    public static class ReflectionExtensions
    {
        public static MethodBase GetOriginal(this MethodBase method)
        {
            if (method != null 
                && method.DeclaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
            {
                var generatedType = method.DeclaringType;
                var originalType = generatedType.DeclaringType;
                return originalType.GetMethods(BindingFlags.Instance 
                                             | BindingFlags.Static 
                                             | BindingFlags.Public 
                                             | BindingFlags.NonPublic 
                                             | BindingFlags.DeclaredOnly)
                    .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
            }
            else
            {
                return method;
            }
        }
    }
}
//MdEnd
