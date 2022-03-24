using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class TodoSummary
    {
        public string Done { get; set; }
        public string DonePercentage { get; set; }
        public string NotDone { get; set; }
        public string NotDonePercentage { get; set; }
        public string OneDay { get; set; }
        public string OneDayPercentage { get; set; }
        public string MoreThanOne { get; set; }
        public string MoreThanOnePercentage { get; set; }
    }
}
