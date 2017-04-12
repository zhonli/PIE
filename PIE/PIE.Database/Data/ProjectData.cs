using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    class ProjectData
    {
        public static ProjectData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly ProjectData _instance = new ProjectData();
        }

        public static IList<Project> Test()
        {
            IList<Project> projects = new List<Project>();

            var pie = new Project() { ID = 1, Name = "PIE", CreateBy = "v-zhongi", CreateTime = DateTime.Now, Privacy = Privacy.Internal };
            //pie.Teams = new List<Team>();
            //pie.Teams.Add(TeamData.Current["Inner Dev"]);
            projects.Add(pie);

            var rs1 = new Project() { ID = 2, Name = "RS1 RTM Core", CreateBy = "v-zhongi", CreateTime = DateTime.Now, Privacy = Privacy.Public };
            //rs1.Teams = new List<Team>();
            //rs1.Teams.Add(TeamData.Current["Desktop & Mobile"]);
            projects.Add(rs1);

            var nano = new Project() { ID = 3, Name = "Nano Server Core", CreateBy = "v-zhongi", CreateTime = DateTime.Now, Privacy = Privacy.Protected };
            //nano.Teams = new List<Team>();
            //nano.Teams.Add(TeamData.Current["Server"]);
            projects.Add(nano);

            

            return projects;
        }


    }
}
