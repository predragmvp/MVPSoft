using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class SelfServiceExpenseSummary
    {
        public string Approved { get; set; }
        public string ApprovedPercentage { get; set; }
        public string NotApproved { get; set; }
        public string NotApprovedPercentage { get; set; }
        public string CashAdvance { get; set; }
        public string CashAdvancePercentage { get; set; }
        public string Reimbursement { get; set; }
        public string ReimbursementPercentage { get; set; }
    }
}
