using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    class TestCollateralData
    {
        public static TestCollateralData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly TestCollateralData _instance = new TestCollateralData();
        }

        public static IList<TestCollateral> Test()
        {
            IList<TestCollateral> testCollaterals = new List<TestCollateral>();
            TestCollateral tc1 = new TestCollateral() { ID = 1, Workhours = 30, TestCaseCount = 10, LanguageCount = 3 };
            TestCollateral tc2 = new TestCollateral() { ID = 2, Workhours = 50, TestCaseCount = 10, LanguageCount = 5 };
            TestCollateral tc3 = new TestCollateral() { ID = 3, Workhours = 25, TestCaseCount = 5, LanguageCount = 5 };
            TestCollateral tc4 = new TestCollateral() { ID = 4, Workhours = 60, TestCaseCount = 10, LanguageCount = 6 };
            TestCollateral tc5 = new TestCollateral() { ID = 5, Workhours = 40, TestCaseCount = 10, LanguageCount = 4 };
            TestCollateral tc6 = new TestCollateral() { ID = 6, Workhours = 30, TestCaseCount = 10, LanguageCount = 3 };
            TestCollateral tc7 = new TestCollateral() { ID = 7, Workhours = 28, TestCaseCount = 10, LanguageCount = 3 };
            TestCollateral tc8 = new TestCollateral() { ID = 8, Workhours = 60, TestCaseCount = 10, LanguageCount = 6 };

            return testCollaterals;
        }
    }
}
