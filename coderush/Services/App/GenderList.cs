using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Services.App
{
    public static class GenderList
    {
        public static List<String> GetAll()
        {
            List<string> all = new List<string>();

            all.Add("Male");
            all.Add("Female");

            return all;
        }
    }
}
