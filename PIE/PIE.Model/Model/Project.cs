using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIEM.Common.Model
{
    /// <summary>
    /// [New Feature]
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Project ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Project name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Project Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Project creater
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// The time of project creation
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// User who modifies the project
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// The last modified time of project 
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }
        /// <summary>
        /// All the plan in the project
        /// </summary>
        public virtual IList<Plan> Plans { get; set; }
        /// <summary>
        /// All the team in the project
        /// </summary>
        public virtual IList<Team> Teams { get; set; }
    }
}
