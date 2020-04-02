//@QnSCodeCopy
//MdStart
using System;

namespace CommonBase.Helpers
{
    public class OperationNotFoundException : Exception
    {
        public OperationNotFoundException(string operationName) : base($"The operation called '{operationName}' was not found!")
        {
            
        }
    }
}
//MdEnd
