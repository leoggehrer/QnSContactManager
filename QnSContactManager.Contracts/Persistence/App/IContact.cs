using System;
using System.Collections.Generic;
using System.Text;

namespace QnSContactManager.Contracts.Persistence.App
{
    public interface IContact : IIdentifiable, ICopyable<IContact>
    {
        string Name { get; set; }
        string Email { get; set; }
        string Addresse { get; set; }
        Modules.App.ContactType Type { get; set; }
    }
}
