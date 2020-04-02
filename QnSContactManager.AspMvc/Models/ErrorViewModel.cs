//@QnSCodeCopy
//MdStart
using System;

namespace QnSContactManager.AspMvc.Models
{
    public partial class ErrorViewModel : ModelObject
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
//MdEnd
