using System;
using System.Collections.Generic;
using System.Text;

namespace QnSContactManager.Contracts.Modules.Account
{
    public interface ILoginUser : ILogin
    {
        public string Name { get; set; }
        public Contracts.Modules.Common.State State { get; set; }
    }
}
