//@QnSCodeCopy
//MdStart
using CommonBase.Extensions;
using System.Linq;

namespace QnSContactManager.Transfer.InvokeTypes
{
    public partial class InvokeParam
    {
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public string Separator { get; set; } = ";";

        public void SetParameters(params object[] parameters)
        {
            parameters.CheckArgument(nameof(parameters));

            for (int i = 0; parameters != null && i < parameters.Length; i++)
            {
                Parameters += $"{(i > 0 ? Separator : string.Empty)}{parameters[i]}";
            }
        }
        public string[] GetParameters()
        {
            string[] result;

            if (string.IsNullOrEmpty(Parameters))
            {
                result = new string[0];
            }
            else
            {
                result = Parameters.Split(Separator.ToCharArray()).ToArray();
            }
            return result;
        }
    }
}
//MdEnd
