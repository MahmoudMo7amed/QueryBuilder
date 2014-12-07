using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilderTests;
using QueryBuilder.Enums;
namespace QueryBuilder.Tests
{
    [TestClass()]
    public class QueryTests
    {
        [TestMethod()]
        public void GenerateSqlTest()
        {
            GRemDataToSearchWithStatus _DataToSearch = new GRemDataToSearchWithStatus();
            _DataToSearch.GRemNumber = "MahMON";
            _DataToSearch.DateFrom = DateTime.Now;
            _DataToSearch.DateTo = DateTime.Now;

            string sql = getQuery(_DataToSearch).GenerateSql();
            Assert.Fail();
        }

        [TestMethod()]
        public void NestedQueryWithAggregateFunctions()
        {
            GRemDataToSearchWithStatus _DataToSearch = new GRemDataToSearchWithStatus();
            _DataToSearch.GRemNumber = "MahMON";
            _DataToSearch.DateFrom = DateTime.Now;
            _DataToSearch.DateTo = DateTime.Now;
            string sql = getQuery(_DataToSearch)
                .Count("count")
                .Sum(new Column("Id", "GRemGUIDsStatus"), "Sum")
                .FromQuery()
                .Select()
                .GenerateSql();

            Assert.Fail();
        }


        [TestMethod()]
        public void AggregateFunctionTest()
        {
            GRemDataToSearchWithStatus _DataToSearch = new GRemDataToSearchWithStatus();
            _DataToSearch.GRemNumber = "MahMON";
            _DataToSearch.DateFrom = DateTime.Now;
            _DataToSearch.DateTo = DateTime.Now;
            string sql = getQuery(_DataToSearch).Count("count").Sum(new Column("Id", "GRemGUIDsStatus"), "Sum").GenerateSql();
            Assert.Fail();
        }


        [TestMethod()]
        public void NormalFunctionTest()
        {
            GRemDataToSearchWithStatus _DataToSearch = new GRemDataToSearchWithStatus();
            _DataToSearch.GRemNumber = "MahMON";
            _DataToSearch.DateFrom = DateTime.Now;
            _DataToSearch.DateTo = DateTime.Now;
           
            //string sql = getQuery(_DataToSearch)
            //             .FromQuery()
            //             .SelectFunction(
            //             (param) =>
            //             { return string.Format("ROW_NUMBER() OVER(ORDER BY {0} " + param[1] + ") ", param[0]); }
            //             ,"Id",OrderByDirection.ASC.ToString()
            //             )
            //             .FromQuery()
            //             .Select()
            //             .GenerateSql();

            string sql = getQuery(_DataToSearch)
                         .FromQuery()
                         .SelectFunction(Function.PagingFunctions.GetRowNumber("Id","Alis"))
                         .FromQuery()
                         .Select()
                         .GenerateSql();
            Assert.Fail();
        }



        [TestMethod()]
        public void OderByTest()
        {
            GRemDataToSearchWithStatus _DataToSearch = new GRemDataToSearchWithStatus();
            _DataToSearch.GRemNumber = "MahMON";
            _DataToSearch.DateFrom = DateTime.Now;
            _DataToSearch.DateTo = DateTime.Now;
            string sql = getQuery(_DataToSearch)

                         .OrderBy(new Column("GremNum", "GRemGUIDs"))
                         .OrderBy(new Column("Name", "Authority"))
                         .GenerateSql();
            Assert.Fail();
        }






        [TestMethod()]
        public void GroupByTest()
        {
            GRemDataToSearchWithStatus _DataToSearch = new GRemDataToSearchWithStatus();
            _DataToSearch.GRemNumber = "MahMON";
            _DataToSearch.DateFrom = DateTime.Now;
            _DataToSearch.DateTo = DateTime.Now;
            string sql = getQuery(_DataToSearch)
                         .GroupBy(new Column("GremNum", "GRemGUIDs"))
                         .OrderBy(new Column("GremNum", "GRemGUIDs"))
                         .OrderBy(new Column("Name", "Authority"))
                         .Count()
                         .GenerateSql();
            Assert.Fail();
        }

        Query getQuery(GRemDataToSearchWithStatus DataToSearch)
        {
            Table _GremGUID = new Table("GRemGUIDs", "GremNum", "CreationDate");
            IColumn f = _GremGUID["GremNum"];
            _GremGUID["GremNum"] = new Column("fk_AuthorityId", _GremGUID);
            _GremGUID.Where("fk_AuthorityId", ComparisonOperator.Equal, DataToSearch.AuthID)
                     .Where("CreationDate", ComparisonOperator.GreaterEqual, DataToSearch.DateFrom)
                     .Where("CreationDate", ComparisonOperator.LessEqual, DataToSearch.DateTo)
                     .Where("GremNum", ComparisonOperator.Equal, DataToSearch.GRemNumber)
                     .Where("Status", ComparisonOperator.Equal, DataToSearch.Status);
            Table _Authority = new Table("Authority", "Name 'AuthorityName'");
            Table _GRemGUIDsFileDetails = new Table("GRemGUIDsFileDetails", "FileName");
            Table _GRemGUIDsStatus = new Table("GRemGUIDsStatus", "Name 'Status'");
            Query _Query = new Query(_GremGUID, _Authority, _GRemGUIDsFileDetails, _GRemGUIDsStatus);
            _Query.Join(new Column("fk_AuthorityId", _GremGUID), Enums.JoinTypes.InnerJoin, new Column("Id", _Authority))
              .Join(new Column("Id", _GremGUID), Enums.JoinTypes.InnerJoin, new Column("Id", _GRemGUIDsFileDetails))
              .Join(new Column("Status", _GremGUID), Enums.JoinTypes.InnerJoin, new Column("Id", _GRemGUIDsStatus));
            return _Query;
        }

        void MyQuery()
        {
            Table _GremGUID = new Table("GRemGUIDs", "GremNum", "CreationDate");
            IColumn f = _GremGUID["GremNum"];
            _GremGUID["GremNum"] = new Column("fk_AuthorityId", _GremGUID);
            Query _MyQ = new Query(_GremGUID);

        }
    }
}
