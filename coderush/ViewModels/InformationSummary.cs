using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class InformationSummary
    {
        public string Active { get; set; }
        public string ActivePercentage { get; set; }
        public string NotActive { get; set; }
        public string NotActivePercentage { get; set; }
        public string UsingLink { get; set; }
        public string UsingLinkPercentage { get; set; }
        public string NotUsingLink { get; set; }
        public string NotUsingLinkPercentage { get; set; }
    }
}
