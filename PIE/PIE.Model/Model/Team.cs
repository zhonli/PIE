using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.OData.Builder;

namespace PIEM.Common.Model
{
    /// <summary>
    /// [New Feature]
    /// </summary>
    public class Team
    {
        /// <summary>
        /// Team ID
        /// </summary>
        public int TeamID { get; set; }
        /// <summary>
        /// Team Name
        /// </summary>
        public string TeamName { get; set; }
        /// <summary>
        /// Team Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// All the team members
        /// </summary>
        public virtual IList<Resource> Members { get; set; }
        /// <summary>
        /// All the project in the team
        /// </summary>
        public virtual IList<Project> Projects { get; set; }
    }
}