using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    class ResourceData
    {
        public static ResourceData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly ResourceData _instance = new ResourceData();
        }

        public Resource this[string name]
        {
            get
            {
                var resource = Test().FirstOrDefault(t => t.Alias.ToUpper() == name.ToUpper());
                return resource;
            }
        }

        public static IList<Resource> Test()
        {

            IList<Resource> resources = new List<Resource>();
            var zhongi = new Resource() { ResourceID = 1, Alias = "FAREAST\\v-zhongi", FirstName = "Zhongkui", LastName = "Li", FullName = "Zhongkui Li", Email = "v-zhongi@microsoft.com" };
            //zhongi.Teams = new List<Team>() { TeamData.Current["Inner Dev"] };
            resources.Add(zhongi);

            var zhanfj = new Resource() { ResourceID = 2, Alias = "FAREAST\\v-zhanfj", FirstName = "Zhanfeng", LastName = "Jiang", FullName = "Zhanfeng Jiang", Email = "v-zhanfj@microsoft.com" };
            //zhanfj.Teams = new List<Team>() { TeamData.Current["Inner Dev"] };
            resources.Add(zhanfj);

            var yang = new Resource() { ResourceID = 3, Alias = "FAREAST\\v-yany", FirstName = "Yang", LastName = "Yang", FullName = "Yang Yang", Email = "v-yany@microsoft.com" };
            //yang.Teams = new List<Team>() { TeamData.Current["Inner Dev"] };
            resources.Add(yang);

            var lucyl = new Resource() { ResourceID = 4, Alias = "FAREAST\\v-lucyl", FirstName = "Lucy", LastName = "Li", FullName = "Lucy Li", Email = "v-lucyl@microsoft.com" };
            //lucyl.Teams = new List<Team>() { TeamData.Current["Inner Dev"] };
            resources.Add(lucyl);

            return resources;
        }

    }
}
