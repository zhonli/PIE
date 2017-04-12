using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIEM.Common.Model
{
    /// <summary>
    /// [Not Using]
    /// </summary>
    public class Sprint
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }
}
