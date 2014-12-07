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
    public class ColumnTests
    {
        [TestMethod()]
        public void ToStringTestTableWithoutAliasANDColumnWithoutAlias()
        {
            Table _Table=new Table("Posting");
            Column _Col = new Column("Col", _Table);
            string Actual = _Col.ToString();
            string Expected = "Posting.Col";
            Assert.AreEqual(Expected, Actual);
        }
        [TestMethod()]
        public void ToStringTestTableWithAliasANDColumnWithoutAlias()
        {
            Table _Table = new Table("Posting alias");
            Column _Col = new Column("Col", _Table);
            string Actual = _Col.ToString();
            string Expected = "alias.Col";
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod()]
        public void ToStringTestTableWithAliasANDColumnWithAlias()
        {
            Table _Table = new Table("Posting alias");
            Column _Col = new Column("Col", _Table,"ColAlias");
            string Actual = _Col.ToString();
            string Expected = "alias.Col ColAlias";
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod()]
        public void ToStringTestTableWithAliasANDColumnStringAlias()
        {
            Table _Table = new Table("Posting alias");
            Column _Col = new Column("Col ColAlias", _Table);
            string Actual = _Col.ToString();
            string Expected = "alias.Col ColAlias";
            Assert.AreEqual(Expected, Actual);
        }
        [TestMethod()]
        public void ToStringTestTableWithoutNameANDColumnStringAlias()
        {
            Table _Table = new Table("Posting");
            _Table.Name = "";
            Column _Col = new Column("Col ColAlias", _Table);
            string Actual = _Col.ToString();
            string Expected = "Col ColAlias";
            Assert.AreEqual(Expected, Actual);
        }

    }
}
