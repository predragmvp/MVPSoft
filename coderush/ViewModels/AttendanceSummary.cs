using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class AttendanceSummary
    {
        public string Presence { get; set; }
        public string PresencePercentage { get; set; }
        public string Absence { get; set; }
        public string AbsencePercentage { get; set; }
        public string PaidLeave { get; set; }
        public string PaidLeavePercentage { get; set; }
        public string UnpaidLeave { get; set; }
        public string UnpaidLeavePercentage { get; set; }
    }
}
