using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    class FeatureData
    {
        public static FeatureData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly FeatureData _instance = new FeatureData();
        }

        public static IList<Feature> Test()
        {
            IList<Feature> projects = new List<Feature>();

            return projects;
        }


    }
}
