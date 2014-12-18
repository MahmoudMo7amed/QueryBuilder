using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
using QueryBuilder.QueryBuilders;
using QueryBuilder.Utils;
using System;
using System.Collections.Generic;

namespace QueryBuilder
{
    public class Query : ColumnHolder, IQuery, IQueryInternal
    {
        /// <summary>
        /// add config class for query building Configuration
        /// such as ValidateOnGeneration =True;
        /// /// </summary>

        private Query _NestedQuery = null;
        private List<Table> _lstTable = null;
        private List<Join> _lstJoin = new List<Join>();
        private List<IColumn> _lstGroupBy = new List<IColumn>();
        private List<OrderBy> _lstOrderBy = new List<OrderBy>();
        private List<Function> _lstAggregateFunctions = new List<Function>();

        //  internal bool IsGroupByContained = false;

        public Query(params Table[] tables)
        {
            _lstTable = new List<Table>();
            this._lstTable.AddRange(tables);
        }

        public Query(Query q)
        {
            this._NestedQuery = q;
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

        // IQueryInternal.Joins
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

        private List<Having> _Having = new List<Having>();

        List<Having> IQueryInternal.HavingClause
        {
            get
            {
                return _Having;
            }
        }

        List<OrderBy> IQueryInternal.OrderByList
        {
            get
            {
                return _lstOrderBy;
            }
        }

        List<Function> IQueryInternal.NormalSelectFFunctions
        {
            get
            {
                return _lstNormalSelectFFunctions;
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

        Query IQueryInternal.NestedQuery
        {
            get
            {
                return _NestedQuery;
            }
        }

        private List<Union> _lstQueryUnion = new List<Union>();

        List<Union> IQueryInternal.Unions
        {
            get { return _lstQueryUnion; }
        }
        bool _bolDistinct;
        bool IQueryInternal.IsDistinct
        {
            get
            {
                return _bolDistinct;
            }
        }

        private bool IsGroupByContained
        {
            get
            {
                return _lstGroupBy.Count > 0;
            }
        }

        #endregion properties

        #region public methods

        public new IQuery Select()
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.Select();
            }
            else
            {
                base.Select();
            }

            return _Query;
        }

        public new IQuery Select(string ColName, string ColAlias = null)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.Select(ColName, ColAlias);
            }
            else
            {
                base.Select(ColName, ColAlias);
            }

            return _Query;
        }

        public new IQuery SelectFunction(Func<string[], string> functionSql, params string[] parameters)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.SelectFunction(functionSql, parameters);
            }
            else
            {
                base.SelectFunction(functionSql, parameters);
            }
            return _Query;
        }

        public new IQuery SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.SelectFunction(alias, functionSql, parameters);
            }
            else
            {
                base.SelectFunction(alias, functionSql, parameters);
            }
            return _Query;
        }

        public new IQuery SelectFunction(Func<string> functionSql, string alias = null)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.SelectFunction(functionSql, alias);
            }
            else
            {
                base.SelectFunction(functionSql, alias);
            }
            return _Query;
        }

        public new IQuery SelectFunction(Function dbFunction)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.SelectFunction(dbFunction);
            }
            else
            {
                base.SelectFunction(dbFunction);
            }
            return _Query;
        }

        public new IQuery Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.Where(ColName, ComparisonOperator, value, AcceptNullValue);
            }
            else
            {
                base.Where(ColName, ComparisonOperator, value, AcceptNullValue);
            }
            return _Query;
        }

        public new IQuery Where(string ColName, NullValuesComparison NullComparison)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.Where(ColName, NullComparison);
            }
            else
            {
                base.Where(ColName, NullComparison);
            }
            return _Query;
        }

        public new IQuery Where(string ColName, Query InnerQuery)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
                _Query.Where(ColName, InnerQuery);
            }
            else
            {
                base.Where(ColName, InnerQuery);
            }
            return _Query;
        }

        public IQuery Having(Function aggregateFunction, ComparisonOperator comparison, double value)
        {
            _Having.Add(new Having(aggregateFunction, comparison, value));
            return this;
        }

        public IQuery Distinct()
        {
            _bolDistinct = true;
            return this;
        }

        //public new IQuery Having(Function aggregateFunction, ComparisonOperator comparison, double value)
        //{
        //    Query _Query = this;
        //    //if (_NestedQuery == null)
        //    //{
        //    //    _Query = (Query)FromQuery();
        //    //    _Query.Having(aggregateFunction, comparison, value);
        //    //}
        //    //else
        //    //{
        //        base.Having(aggregateFunction, comparison, value);
        //    //}
        //    return _Query;
        //}

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
            //  IsGroupByContained = true;
            _lstGroupBy.Add(GroupByColumn);
            return this;
        }

        public IQuery OrderBy(IColumn OrderByColumn, OrderByDirection OrderByDir = OrderByDirection.ASC)
        {
            _lstOrderBy.Add(new OrderBy(OrderByColumn, OrderByDir));
            return this;
        }

        public IQuery Union(Query QueryToUnion, bool All = false)
        {
            _lstQueryUnion.Add(new Union(QueryToUnion, All));
            return this;
        }

        public IQuery FromQuery()
        {
            Query _Qu = new Query(this);
            _Qu.Name = _Qu.Alias = AliasUtil.GetNextAlias(this.Alias);
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

            return GenerateSql(SqlGenerationType.SqlServer);
        }

        public string GenerateSql(SqlGenerationType SqlType)
        {

            return SqlFactoryGenerator.GetSQLFactory(SqlType).GenerateSQL(this);
        }

        #endregion public methods

        #region private methods

        private void CheckForColumnExistance(string ColName)
        {
            if (ColumnsDictionary.Count > 0)
            {
                IColumn _Column;
                bool ColumnExist = ColumnsDictionary.TryGetValue(ColName, out _Column);

                throw new ArgumentException("Column " + ColName + " does not exist on " + this.Name + " Query ");
            }
        }

        private void addAggregateFunction(Function Fun)
        {
            _lstAggregateFunctions.Add(Fun);
        }

        #endregion private methods
    }
}