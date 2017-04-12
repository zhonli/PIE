using System;
using System.Collections.Generic;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined all planning info
    /// </summary>
    public class Plan : IComparable<Plan>
    {
        /// <summary>
        /// Plan ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Plan Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Plan Start Date
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Plan End Date
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Plan Workhours
        /// </summary>
        public float Workhours { get; set; }
        /// <summary>
        /// Component ID
        /// As, CID0010002
        /// </summary>
        public string ComponentID { get; set; }
        /// <summary>
        /// Component Name
        /// As, Out Of Box Experience (OOBE) UI
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// Component Path
        /// As, CID0010002/CID0010003/CID0010013
        /// </summary>
        public string ComponentPath { get; set; }
        /// <summary>
        /// Plan Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Plan Tags
        /// As, Bug Regression 
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// Plan Status
        /// </summary>
        public PlanStatus Status { get; set; }
        /// <summary>
        /// Plan Priority
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// Plan Creater
        /// As, REDMOND\v-fiwan
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// Plan Created date
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Plan Last Modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// Plan Last Modified date
        /// </summary>
        public DateTime? LastModeifiedTime { get; set; }
        /// <summary>
        /// Plan executed date
        /// </summary>
        public DateTime? ExecutedOn { get; set; }
        /// <summary>
        /// Plan Closed date (duplicate)
        /// </summary>
        public DateTime? ClosedOn { get; set; }
        /// <summary>
        /// Plan Type
        /// </summary>
        public PlanType Type { get; set; }
        /// <summary>
        /// [New Feature]
        /// </summary>
        public int? ProjectID { get; set; }
        /// <summary>
        /// [New Feature]
        /// </summary>
        public virtual Project Project { get; set; }
        /// <summary>
        /// Plan assignment list
        /// </summary>
        public virtual IList<Assignment> Assignments { get; set; }
        /// <summary>
        /// Task Link with VSO
        /// </summary>
        public virtual TaskLink TaskLink { get; set; }
        /// <summary>
        /// Executing realted info
        /// </summary>
        public virtual Process Process { get; set; }

        /// <summary>
        /// Test case realted info
        /// </summary>
        public virtual TestCollateral TestCollateral { get; set; }
        /// <summary>
        /// Iteration path in vso
        /// </summary>
        public string IterationPath { get; set; }
        /// <summary>
        /// Iteration ID in VSO
        /// </summary>
        public int? IterationID { get; set; }


        public int CompareTo(Plan other)
        {
            return this.ID.CompareTo(other.ID);
        }
    }
}
