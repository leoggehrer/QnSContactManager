using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnSContactManager.AspMvc.Models.Modules.Export
{
    public class ImportModel
    {
        public class LogInfo
        {
            public bool IsError { get; set; }
            public string Prefix { get; set; }
            public string Text { get; set; }
        }

        public string FilePath { get; set; }
        public IEnumerable<LogInfo> LogInfos { get; set; } = new LogInfo[0];
    }
}
