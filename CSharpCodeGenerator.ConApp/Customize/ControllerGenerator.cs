//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;

namespace CSharpCodeGenerator.ConApp.Generation
{
    partial class ControllerGenerator
    {
        partial void CreateLogicControllerAttributes(Type type, List<string> codeLines)
        {
            if (type.FullName.EndsWith(".Business.Account.IAppAccess")
                || type.FullName.EndsWith(".Persistence.Account.IIdentity")
                || type.FullName.EndsWith(".Persistence.Account.IRole")
                || type.FullName.EndsWith(".Persistence.Account.IIdentityXRole")
                || type.FullName.EndsWith(".Persistence.Account.ILoginSession")
                )
            {
                codeLines.Add("[Logic.Modules.Security.Authorize(\"SysAdmin\")]");
            }
        }
    }
}
//MdEnd
