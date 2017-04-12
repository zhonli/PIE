using System;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined task assignment info
    /// </summary>
    public class Assignment
    {
        /// <summary>
        /// Assignment ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Related Plan ID
        /// </summary>
        public int PlanID { get; set; }
        /// <summary>
        /// Related Plan Info
        /// Note, many-to-one relationship with plan
        /// </summary>
        public virtual Plan Plan { get; set; }
        /// <summary>
        /// Resource ID
        /// </summary>
        public int ResourceID { get; set; }
        /// <summary>
        /// Resource info
        /// </summary>
        public virtual Resource Resource { get; set; }
        /// <summary>
        /// Percent in the task assignment
        /// Note, the value is percent * 100
        /// </summary>
        public float Units { get; set; }
        /// <summary>
        /// The user who assgined the task 
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// assignment created  time
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// The user who modified the assginment of the task 
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// assignment last modified time
        /// </summary>
        public DateTime? LastModeifiedTime { get; set; }
    }
}