//@QnSCodeCopy
//MdStart
using System;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class FactoryGenerator
    {
        partial void CanCreateLogicAccess(Type type, ref bool create)
        {
            if (type.FullName.EndsWith(".Persistence.Account.IActionLog")
                || type.FullName.EndsWith(".Persistence.Account.IIdentity")
                || type.FullName.EndsWith(".Persistence.Account.IIdentityXRole")
                || type.FullName.EndsWith(".Persistence.Account.ILoginSession")
                )
            {
                create = false;
            }
        }
    }
}
//MdEnd
