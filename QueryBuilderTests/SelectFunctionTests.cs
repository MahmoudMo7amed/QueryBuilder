using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace QueryBuilder.Tests
{
    [TestClass()]
    public class SelectFunctionTests
    {
        [TestMethod()]
        public void GetResourceTextFileTest()
        {
            //string _str = SelectFunction.GetResourceTextFile( );
            Assert.Fail();
        }

        [TestMethod()]
        public void CountTest()
        {
            Function _Sel= Function.AggregateFunctions.Count();

            Assert.Fail();
        }
    }
}
