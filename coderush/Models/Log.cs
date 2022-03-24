using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class Log
    {
        public Int64 LogId { get; set; }
        public string OperationType { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; }

    }
}
