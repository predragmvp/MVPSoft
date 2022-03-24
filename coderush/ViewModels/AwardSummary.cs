using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class AwardSummary
    {
        public string Approved { get; set; }
        public string ApprovedPercentage { get; set; }
        public string NotApproved { get; set; }
        public string NotApprovedPercentage { get; set; }
        public string Male { get; set; }
        public string MalePercentage { get; set; }
        public string Female { get; set; }
        public string FemalePercentage { get; set; }
    }
}
