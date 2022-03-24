using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class AssetSummary
    {
        public string Used { get; set; }
        public string UsedPercentage { get; set; }
        public string NotUsed { get; set; }
        public string NotUsedPercentage { get; set; }
        public string UsedValue { get; set; }
        public string UsedValuePercentage { get; set; }
        public string NotUsedValue { get; set; }
        public string NotUsedValuePercentage { get; set; }
    }
}
