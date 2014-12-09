using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
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
        private List<Function> _lstNormalSelectFFunctions = new List<Function>();

        internal bool IsGroupByContained = false;

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
        public List<Union> Unions
        {
            get { return _lstQueryUnion; }
        }
        #endregion properties

        #region public methods

        internal bool SelectAll = true;

        public IQuery Select()
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();// new Query(this);
            }
            _Query.SelectAll = true;
            return _Query;
        }

        public IQuery Select(string ColName, string ColAlias = null)
        {
            Query _Query = this;
            if (_NestedQuery == null)
            {
                _Query = (Query)FromQuery();
            }
            _Query.SelectAll = false;
            Column Col = new Column(ColName, _Query, ColAlias);
            _Query.ColumnsDictionary.Add(Col.Name, Col);
            return _Query;
        }

        public IQuery SelectFunction(Func<string[], string> functionSql, params string[] parameters)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(new Function(functionSql, parameters));
            return this;
        }

        public IQuery SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(new Function(alias, functionSql, parameters));
            return this;
        }

        public IQuery SelectFunction(Func<string> functionSql, string alias = null)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(new Function(functionSql, alias));
            return this;
        }

        public IQuery SelectFunction(Function dbFunction)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(dbFunction);
            return this;
        }

        public IQuery Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        {
            //  CheckForColumnExistance(ColName);

            //if (value == null && AcceptNullValue)
            //{
            //    Conditions.Add(new WhereCondition(ColumnsDictionary[ColName], ComparisonOperator, value));
            //}
            //else if (value != null)
            //{
            //    Conditions.Add(new WhereCondition(ColumnsDictionary[ColName], ComparisonOperator, value));
            //}

            if (value == null && AcceptNullValue)
            {
                Conditions.Add(new WhereCondition(getColumnOrCreateIfNotExist(ColName), ComparisonOperator, value));
            }
            else if (value != null)
            {
                Conditions.Add(new WhereCondition(getColumnOrCreateIfNotExist(ColName), ComparisonOperator, value));
            }
            return this;
        }

        public IQuery Where(string ColName, NullValuesComparison NullComparison)
        {
            // CheckForColumnExistance(ColName);

            //  Conditions.Add(new WhereCondition(ColumnsDictionary[ColName], NullComparison));
            Conditions.Add(new WhereCondition(getColumnOrCreateIfNotExist(ColName), NullComparison));

            return this;
        }

        public IQuery Where(string ColName, Query InnerQuery)
        {

            Conditions.Add(new WhereCondition(getColumnOrCreateIfNotExist(ColName), InnerQuery));

            return this;
        }
        public IQuery Having(Function aggregateFunction, ComparisonOperator comparison, double value)
        {
            HavingClause.Add(new Having(aggregateFunction, comparison, value));
            return this;
        }
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
            //if (ColumnsDictionary.Count == 0 && SelectAll == false)
            //{
            //    SelectAll = true;
            //}
            SQLServerSqlBuilder _SqlBuilder = new SQLServerSqlBuilder(this);
            return GenerateSql(_SqlBuilder);
        }

        public string GenerateSql(ISqlBuilder builder)
        {
            return builder.GenerateSQL();
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


        IColumn getColumnOrCreateIfNotExist(string ColName)
        {
            IColumn _Column;
            bool ColumnExist = ColumnsDictionary.TryGetValue(ColName, out _Column);
            if (!ColumnExist)
            {
                _Column = new Column(ColName, this);
            }
            return _Column;
        }
        private void addAggregateFunction(Function Fun)
        {
            _lstAggregateFunctions.Add(Fun);
        }

        #endregion Private Functions









    }
}