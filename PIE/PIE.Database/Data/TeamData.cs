using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    class TeamData
    {
        public static TeamData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly TeamData _instance = new TeamData();
        }


        public Team this[string teamName]
        {
            get
            {
                var team = Test().FirstOrDefault(t => t.TeamName.ToUpper() == teamName.ToUpper());
                return team;
            }
        }
        public static IList<Team> Basic()
        {
            IList<Team> teams = new List<Team>();
            var dm = new Team() { TeamID = 1, TeamName = "Desktop & Mobile" };

            teams.Add(dm);
            var sv = new Team() { TeamID = 2, TeamName = "Server" };
            teams.Add(sv);

            return teams;
        }

        public static IList<Team> Test()
        {
            IList<Team> teams = Basic();
            var dev = new Team() { TeamID = 3, TeamName = "Inner Dev" };
            /*
            dev.Members = new List<Resource>();
            dev.Members.Add(ResourceData.Current["v-zhongi"]);
            dev.Members.Add(ResourceData.Current["v-zhanfj"]);
            dev.Members.Add(ResourceData.Current["v-yangyang"]);
            dev.Members.Add(ResourceData.Current["v-lucyl"]);
            */
            teams.Add(dev);

            return teams;
        }
    }
}
