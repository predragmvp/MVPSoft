using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class TicketSummary
    {
        public string Solve { get; set; }
        public string SolvePercentage { get; set; }
        public string NotSolve { get; set; }
        public string NotSolvePercentage { get; set; }
        public string Recurring { get; set; }
        public string RecurringPercentage { get; set; }
        public string NotRecurring { get; set; }
        public string NotRecurringPercentage { get; set; }
    }
}
