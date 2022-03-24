using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class SelfServiceAttendanceSummary
    {
        public string Presence { get; set; }
        public string PresencePercentage { get; set; }
        public string Absence { get; set; }
        public string AbsencePercentage { get; set; }
        public string ClockInTime { get; set; }
        public string ClockInDate { get; set; }
        public string ClockOutTime { get; set; }
        public string ClockOutDate { get; set; }
    }
}
