using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace QueryBuilder.Utils.Tests
{
    [TestClass()]
    public class AliasGeneratorTests
    {
        [TestMethod()]
        public void GetNextAliasTest()
        {
            string AliasParam = "Alias1";
            string Actual = AliasUtil.GetNextAlias(AliasParam);
            string Expected = "Alias2";
            Assert.AreEqual(Expected, Actual);
        }
    }
}
