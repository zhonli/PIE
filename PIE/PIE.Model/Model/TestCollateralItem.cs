using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the detail info for the test collateral
    /// </summary>
    public class TestCollateralItem
    {
        /// <summary>
        /// Test Collateral Item ID
        /// </summary>
        public int TestCollateralItemID { get; set; }
        /// <summary>
        /// Related language ID
        /// </summary>
        public int LanguageID { get; set; }
        /// <summary>
        /// Related language info
        /// </summary>
        public Language Language { get; set; }
        /// <summary>
        /// Realted test case ID
        /// </summary>
        public int TestCaseID { get; set; }

        /// <summary>
        /// WorkHour for one case with one languge
        /// </summary>
        public float? WorkHour { get; set; }
        /// <summary>
        /// Alternative for workhour, Specify weight for one case with one language, then we can get the workhour by case standard workhour
        /// </summary>
        public float? Weight { get; set; }
        /// <summary>
        /// Realted test collateral ID
        /// </summary>
        public int TestCollateralID { get; set; }
        /// <summary>
        /// Realted test collateral info
        /// </summary>
        public virtual TestCollateral TestCollateral { get; set; }
    }
}
