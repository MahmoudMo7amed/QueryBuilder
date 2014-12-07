using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
using QueryBuilder.Utils;
using System.Collections.Generic;

namespace QueryBuilder
{
    public class Query : IQuery, IQueryInternal
    {
        /// <summary>
        /// add config class for query building Configuration
        /// such as ValidateOnGeneration =True;
        /// /// </summary>

        protected List<Table> _lstTable = null;
        protected List<Join> _lstJoin = new List<Join>();
        protected List<IColumn> _lstGroupBy = new List<IColumn>();
        protected List<OrderBy> _lstOrderBy = new List<OrderBy>();
        protected List<Function> _lstAggregateFunctions = new List<Function>();

        internal bool IsGroupByContained = false;
        internal bool SelectAll = false;

        public Query(params Table[] tables)
        {
            _lstTable = new List<Table>();
            this._lstTable.AddRange(tables);
        }

        #region properties

        private QueryConfiguration _QueryConfiguration = new QueryConfiguration();

        public QueryConfiguration Configuration
        {
            get
            {
                return _QueryConfiguration;
            }
        }

        List<Join> IQueryInternal.Joins
        {
            get
            {
                return _lstJoin;
            }
        }

        List<IColumn> IQueryInternal.GroupByList
        {
            get
            {
                return _lstGroupBy;
            }
        }

        List<OrderBy> IQueryInternal.OrderByList
        {
            get
            {
                return _lstOrderBy;
            }
        }

        List<Function> IQueryInternal.AggregateFunctions
        {
            get
            {
                return _lstAggregateFunctions;
            }
        }

        List<Table> IQueryInternal.TableList
        {
            get
            {
                return _lstTable;
            }
        }

        #endregion properties

        #region public methods

        public IQuery Join(IColumn LeftTableCol, JoinTypes joinType, IColumn RightTableCol)
        {
            _lstJoin.Add(new Join(LeftTableCol, joinType, RightTableCol));
            return this;
        }

        public IQuery Join(Join join)
        {
            _lstJoin.Add(join);
            return this;
        }

        public IQuery GroupBy(IColumn GroupByColumn)
        {
            IsGroupByContained = true;
            _lstGroupBy.Add(GroupByColumn);
            return this;
        }

        public IQuery OrderBy(IColumn OrderByColumn, OrderByDirection OrderByDir = OrderByDirection.ASC)
        {
            _lstOrderBy.Add(new OrderBy(OrderByColumn, OrderByDir));
            return this;
        }

        public QuerySelector FromQuery()
        {
            QuerySelector _Qu = new QuerySelector(this);
            return _Qu;
        }

        public IQuery Count(string Alias = null)
        {
            addAggregateFunction(Function.AggregateFunctions.Count(Alias));
            return this;
        }

        public IQuery Count(IColumn Col, string Alias = null)
        {
            addAggregateFunction(Function.AggregateFunctions.Count(Col, Alias));
            return this;
        }

        public IQuery Sum(IColumn Col, string Alias = null)
        {
            addAggregateFunction(Function.AggregateFunctions.Sum(Col, Alias));
            return this;
        }

        public IQuery Avg(IColumn Col, string Alias = null)
        {
            addAggregateFunction(Function.AggregateFunctions.Avg(Col, Alias));
            return this;
        }

        public IQuery Max(IColumn Col, string Alias = null)
        {
            addAggregateFunction(Function.AggregateFunctions.Max(Col, Alias));
            return this;
        }

        public IQuery Min(IColumn Col, string Alias = null)
        {
            addAggregateFunction(Function.AggregateFunctions.Min(Col, Alias));
            return this;
        }

        /// <summary>
        /// the default is sqlserver formate.
        /// </summary>
        /// <returns></returns>
        public string GenerateSql()
        {
            SQLServerSqlBuilder _SqlBuilder = new SQLServerSqlBuilder(this);
            return GenerateSql(_SqlBuilder);
        }

        public string GenerateSql(ISqlBuilder builder)
        {
            return builder.GenerateSQL();
        }

        #endregion public methods

        #region Private Functions

        private void addAggregateFunction(Function Fun)
        {
            _lstAggregateFunctions.Add(Fun);
        }

        #endregion Private Functions
    }
}