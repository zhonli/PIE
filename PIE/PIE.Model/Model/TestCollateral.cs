using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.OData.Builder;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the task case scoped info
    /// </summary>
    public class TestCollateral
    {
        /// <summary>
        /// Test collateral ID, which equal with plan Id
        /// Maybe one-to-many relation with plan is better.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// realated Plan info
        /// </summary>
        public virtual Plan Plan { get; set; }
        /// <summary>
        /// The total workhours summated all the test case with languages
        /// </summary>
        public float Workhours { get; set; }
        /// <summary>
        /// The total test case count
        /// </summary>
        public int TestCaseCount { get; set; }
        /// <summary>
        /// The total languages 
        /// </summary>
        public int LanguageCount { get; set; }
        /// <summary>
        /// The detail info of test collateral
        /// </summary>
        [Contained]
        public virtual IList<TestCollateralItem> Items { get; set; }
    }
}
