using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Common.Model
{

    /// <summary>
    /// Defined the VSO task info
    /// </summary>
    public class VSOTask 
    {
        /// <summary>
        /// VSO Task Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// VSO Task Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// VSO task creater
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// VSO task created date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// VSO task description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// User who changed the VSO task
        /// </summary>
        public string ChangedBy { get; set; }
        /// <summary>
        /// the date when changed the VSO task
        /// </summary>
        public DateTime ChangedDate { get; set; }
        /// <summary>
        /// Iteration ID of VSO task
        /// </summary>
        public int IterationId { get; set; }
        /// <summary>
        /// Iteration path of VSO task
        /// </summary>
        public string IterationPath { get; set; }
        /// <summary>
        /// Area ID of VSO task
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// Area path of VSO task
        /// </summary>
        public string AreaPath { get; set; }
        /// <summary>
        /// The task group of VSO task 
        /// </summary>
        public int? TaskGroupID { get; set; }
        /// <summary>
        /// The product family related on VSO task
        /// </summary>
        public string ProductFamily { get; set; }
        /// <summary>
        /// The product realted on VSO Task
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// The release of VSO task
        /// </summary>
        public string Release { get; set; }
        /// <summary>
        /// VSO task tags.
        /// </summary>
        public string Tags { get; set; }
    }
}
