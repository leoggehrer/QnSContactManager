//@QnSCodeCopy
//MdStart

namespace QnSContactManager.AspMvc.Models
{
    public class SubmitBackCmd
    {
        public string SubmitText { get; set; } = "Save";
        public string SubmitCss { get; set; } = "btn btn-primary";
        public string SubmitStyle { get; set; } = "min-width: 8em;";

        public string BackText { get; set; } = "Back to List";
        public string BackAction { get; set; } = "Index";
        public string BackController { get; set; }
        public string BackCss { get; set; } = "btn btn-outline-dark";
        public string BackStyle { get; set; } = "min-width: 8em;";
    }
}
//MdEnd
