using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Services.App
{
    public static class MaritalStatusList
    {
        public static List<String> GetAll()
        {
            List<string> all = new List<string>();

            all.Add("Single");
            all.Add("Married");
            all.Add("Remarried");
            all.Add("Separated");
            all.Add("Divorced");
            all.Add("Widowed");

            return all;
        }
    }
}
