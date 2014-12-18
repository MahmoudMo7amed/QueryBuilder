using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
using QueryBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryBuilder
{
    internal class SQLServerFactory : ISqlFactory
    {
        public SQLServerFactory()
        {
        }

        #region public methods

        public string GenerateSQL(Query query)
        {
            IQueryInternal _IQueryInternal = (IQueryInternal)query;
            List<Table> _lstTable = _IQueryInternal.TableList;
            Query _NestedQuery = _IQueryInternal.NestedQuery;
            List<Join> _lstJoin = _IQueryInternal.Joins;
            List<IColumn> _lstGroupBy = _IQueryInternal.GroupByList;
            List<OrderBy> _lstOrderBy = _IQueryInternal.OrderByList;
            List<Function> _lstAggregateFunctions = _IQueryInternal.AggregateFunctions;
            List<Function> _lstNormalSelectFFunctions = _IQueryInternal.NormalSelectFFunctions;
            List<Union> _lstQueryUnion = _IQueryInternal.Unions;
            List<Having> _Having = _IQueryInternal.HavingClause;
            bool _bolDistinct = _IQueryInternal.IsDistinct;

            string _return = GetSelectColumns(_lstTable, _NestedQuery, _lstAggregateFunctions
                                              , _lstNormalSelectFFunctions
                                              , query.Columns
                                              , query.SelectAll
                                              , _lstGroupBy.Count() > 0
                                              , Distinct: _bolDistinct)
           + Environment.NewLine +
                            GetFrom(_lstTable, _lstJoin, _NestedQuery, query.Alias)
           + Environment.NewLine +
                            GetWhereCondition(_lstTable, _NestedQuery, query.Conditions)
           + Environment.NewLine +
                            GetGroupBy(_lstGroupBy)
           + Environment.NewLine +
                            GetHavingClause(_Having)
           + Environment.NewLine +
                            GetOrderBy(_lstOrderBy);
            string UnionClause = "union ";
            foreach (var item in _lstQueryUnion)
            {
                if (item.UnionAll)
                {
                    UnionClause += "all";
                }
                _return += UnionClause + Environment.NewLine + Environment.NewLine +
                    item.QueryToUnion.GenerateSql();
            }
            return _return;
        }

        public string GenerateSQL(Having havingClause)
        {
            string _Return = string.Empty;
            if (havingClause.Comparison == ComparisonOperator.Like
                || havingClause.Comparison == ComparisonOperator.StartLike
                || havingClause.Comparison == ComparisonOperator.EndLike
                )
            {
                throw new ArgumentException("havine clause could not have [like; operator");
            }

            _Return = string.Format("{0} {1} {2}", havingClause.AggregateFunction.ToString(), SqlOperatorEnumConverter.getComparisonOperatorString(havingClause.Comparison), havingClause.Value);

            return _Return;
        }

        public string GenerateSQL(Join join)
        {
            string _Return = string.Empty;

            _Return = string.Format("{0} {1} {2} on {3} = {4}", getTableName(join.LeftColumn.Container)
                                        , GetJoinString(join.JoinType)
                                        , getTableName(join.RightColumn.Container)
                                        , join.LeftColumn.GetFullyQualifiedName()
                                        , join.RightColumn.GetFullyQualifiedName()
                  );
            return _Return;
        }

        public string GenerateSQL(OrderBy ordrby)
        {
            return ordrby.OrderByColumn.ToString() + " " + ordrby.Direction.ToString();
        }

        public string GenerateSQL(WhereCondition condition)
        {
            string _Return = string.Empty;
            if (condition.CompOperator != null)
            {
                if (condition.CompOperator.Value == ComparisonOperator.Like)
                {
                    _Return = string.Format("{0} {1} '%{2}%'", condition.Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(condition.CompOperator.Value), getValue(condition.Value));
                }
                else if (condition.CompOperator.Value == ComparisonOperator.StartLike)
                {
                    _Return = string.Format("{0} {1}  '{2}%'", condition.Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(condition.CompOperator.Value), getValue(condition.Value));
                }
                else if (condition.CompOperator.Value == ComparisonOperator.EndLike)
                {
                    _Return = string.Format("{0} {1} '%{2}'", condition.Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(condition.CompOperator.Value), getValue(condition.Value));
                }
                else
                {
                    if (condition.Value is string)
                    {
                        _Return = string.Format("{0} {1} '{2}'", condition.Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(condition.CompOperator.Value), condition.Value.ToString());
                    }
                    else
                    {
                        _Return = string.Format("{0} {1} {2}", condition.Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(condition.CompOperator.Value), getValue(condition.Value));
                    }
                }
            }
            else if (condition.NullValComparison != null)
            {
                _Return = string.Format("{0} {1}", condition.Column.ToString(), getNullValuesComparisonString(condition.NullValComparison.Value));
            }
            else if (condition.QueryValue != null)
            {
                _Return = string.Format("{0} in ({1})", condition.Column.ToString(), condition.QueryValue.GenerateSql());
            }
            return _Return;
        }

        #endregion public methods

        #region private methods

        private string getDistinct(bool _bolDistinct)
        {
            string _Return = string.Empty;
            if (_bolDistinct)
            {
                _Return = " distinct ";
            }
            return _Return;
        }

        private string GetSelectColumns(List<Table> _lstTable, Query _NestedQuery
       , List<Function> _lstAggregateFunctions, List<Function> _lstNormalSelectFFunctions
        , List<IColumn> querySelectColumns, bool SelectAll, bool IsGroupByContained, bool Distinct)
        {
            string _Return = string.Empty;
            if (_lstTable == null && _NestedQuery == null && _lstAggregateFunctions == null) throw new InvalidOperationException("no select columns suppliedto the query ");
            if (_lstTable != null) //so this is Query from Table
            {
                _Return = "select " + getDistinct(Distinct);
                foreach (var item in _lstTable)
                {
                    if (item == _lstTable.First())
                    {
                        if (item.SelectAll)
                        {
                            _Return += item.ToString() + ".* ";
                        }
                        else
                        {
                            _Return += getSelectColumnsString(item.Columns);
                        }
                    }
                    else
                    {
                        if (item.SelectAll)
                        {
                            _Return += item.ToString() + ".* ";
                        }
                        else if (item.Columns.Count > 0)
                        {
                            _Return += "," + Environment.NewLine + getSelectColumnsString(item.Columns);
                        }
                    }
                }
            }
            else if (_NestedQuery != null) //so this is select from nested query
            {
                if (SelectAll)
                {
                    _Return += "select " + getDistinct(Distinct) + " * ";
                }
                string Result = getSelectColumnsString(querySelectColumns);
                if (!string.IsNullOrEmpty(Result))
                {
                    _Return += "select " + getDistinct(Distinct) + Result;
                }
            }
            if (_lstNormalSelectFFunctions.Count() > 0)
            {
                foreach (var item in _lstNormalSelectFFunctions)
                {
                    if (item == _lstNormalSelectFFunctions.First())
                    {
                        if (string.IsNullOrEmpty(_Return))
                        {
                            _Return = "select " + getDistinct(Distinct);
                            _Return += item.ToString();
                        }
                        else
                        {
                            _Return += "," + Environment.NewLine + item.ToString();
                        }
                    }
                    else
                    {
                        _Return += "," + Environment.NewLine + item.ToString();
                    }
                }
            }
            if (_lstAggregateFunctions.Count() > 0)
            {
                if (!IsGroupByContained)
                {
                    _Return = string.Empty;
                }
                foreach (var item in _lstAggregateFunctions)
                {
                    if (item == _lstAggregateFunctions.First())
                    {
                        if (string.IsNullOrEmpty(_Return))
                        {
                            _Return = "select ";
                            _Return += item.ToString();
                        }
                        else
                        {
                            _Return += "," + Environment.NewLine + item.ToString();
                        }
                    }
                    else
                    {
                        _Return += "," + Environment.NewLine + item.ToString();
                    }
                }
            }

            return _Return;
        }

        private string getHavingClauseString(List<Having> Havings)
        {
            StringBuilder _Sp = new StringBuilder();
            int Count = Havings.Count - 1;

            for (int i = 0; i < Count; i++)
            {
                _Sp.Append(GenerateSQL(Havings[i]) + " and ");
            }
            if (Count >= 0)//count could be -1 if the table has no columns to select .
            {
                _Sp.Append(GenerateSQL(Havings[Count]));
            }
            return _Sp.ToString();
        }

        private string GetFrom(List<Table> _lstTable, List<Join> _lstJoin, Query _NestedQuery, string QueryAlias)
        {
            string _Return = " From ";
            if (_lstTable != null)
            {
                if (_lstJoin.Count == 0)
                {
                    if (_lstTable.Count == 1)
                    {
                        _Return += _lstTable[0].ToString();
                    }
                    else
                    {
                        throw new InvalidOperationException("many tables to select with no join clauses");
                    }
                }
                else
                {
                    foreach (var item in _lstJoin)
                    {
                        if (item == _lstJoin.First())
                        {
                            _Return += GenerateSQL(item);
                        }
                        else
                        {
                            _Return += getJoinBeginingFromJoinType(item);
                        }
                    }
                }
            }
            else if (_NestedQuery != null)
            {
                _Return += "(" + _NestedQuery.GenerateSql() + ")" + QueryAlias;//_Query.Alias;
            }
            return _Return;
        }

        private string getJoinBeginingFromJoinType(Join join)
        {
            return string.Format("  {0} {1} on {2} = {3}"
                                   , GetJoinString(join.JoinType)
                                   , getTableName(join.RightColumn.Container)
                                   , join.LeftColumn.GetFullyQualifiedName()
                                   , join.RightColumn.GetFullyQualifiedName()

             );
        }

        private string GetWhereCondition(List<Table> _lstTable, Query _NestedQuery, List<WhereCondition> Conditions)
        {
            StringBuilder _StringBuilder = new StringBuilder();

            if (_lstTable != null)
            {
                foreach (var item in _lstTable)
                {
                    _StringBuilder.Append(getWhereConditionString(item.Conditions));
                }
            }
            else if (_NestedQuery != null)
            {
                _StringBuilder.Append(getWhereConditionString(Conditions));
            }

            string _str = _StringBuilder.ToString();
            if (!string.IsNullOrEmpty(_str))
            {
                _str = " Where " + _str;
            }
            return _str;
        }

        private string GetHavingClause(List<Having> _Having)
        {
            StringBuilder _StringBuilder = new StringBuilder();

            _StringBuilder.Append(getHavingClauseString(_Having));

            string _str = _StringBuilder.ToString();
            if (!string.IsNullOrEmpty(_str))
            {
                _str = " Having " + _str;
            }
            return _str;
        }

        private string GetGroupBy(List<IColumn> _lstGroupBy)
        {
            string _Return = string.Empty;
            if (_lstGroupBy.Count > 0)
            {
                _Return = " group by ";
                foreach (var item in _lstGroupBy)
                {
                    if (item == _lstGroupBy.First())
                    {
                        _Return += item.ToString();
                    }
                    else
                    {
                        _Return += " , " + item.ToString();
                    }
                }
            }
            return _Return;
        }

        private string GetOrderBy(List<OrderBy> _lstOrderBy)
        {
            string _Return = string.Empty;
            if (_lstOrderBy.Count > 0)
            {
                _Return = " order by ";
                foreach (var item in _lstOrderBy)
                {
                    if (item == _lstOrderBy.First())
                    {
                        _Return += GenerateSQL(item);
                    }
                    else
                    {
                        _Return += " , " + GenerateSQL(item);
                    }
                }
            }
            return _Return;
        }

        private string getSelectColumnsString(List<IColumn> Cols)
        {
            StringBuilder _StringBuilder = new StringBuilder();
            foreach (var col in Cols)
            {
                //  _StringBuilder.Append(col.ToString() + ",");
                _StringBuilder.Append(col.ToString() + ",");
            }
            if (Cols.Count > 0)
            {
                _StringBuilder = _StringBuilder.Remove(_StringBuilder.Length - 1, 1);
            }

            return _StringBuilder.ToString();
        }

        private string getWhereConditionString(List<WhereCondition> Conditions)
        {
            StringBuilder _Sp = new StringBuilder();
            int Count = Conditions.Count - 1;

            for (int i = 0; i < Count; i++)
            {
                _Sp.Append(GenerateSQL(Conditions[i]) + " and ");
            }
            if (Count >= 0)//count could be -1 if the table has no columns to select .
            {
                _Sp.Append(GenerateSQL(Conditions[Count]));
            }
            return _Sp.ToString();
        }

        private string getTableName(IField ColHolder)
        {
            string _Return;
            if (string.IsNullOrEmpty(ColHolder.Alias))
            {
                _Return = ColHolder.Name;
            }
            else
            {
                _Return = string.Format("{0} {1}", ColHolder.Name, ColHolder.Alias);
            }
            return _Return;
        }

        private string GetJoinString(JoinTypes joinType)
        {
            string _return;
            switch (joinType)
            {
                case JoinTypes.InnerJoin:
                    _return = " inner join ";
                    break;

                case JoinTypes.LeftOuterJoin:
                    _return = " left outer join ";
                    break;

                case JoinTypes.RightOuterJoin:
                    _return = " left outer join ";
                    break;

                case JoinTypes.FullOuterJoin:
                    _return = " full outer join ";
                    break;

                default:
                    _return = string.Empty;
                    break;
            }

            return Environment.NewLine + _return;
        }

        private string getValue(object value)
        {
            string _Return = null;

            if (value is bool)
            {
                _Return = ((bool)value) == true ? "1" : "0";
            }
            else if (value is DateTime)
            {
                _Return = string.Format("'{0}'", ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                if (value != null)
                {
                    _Return = value.ToString();
                }
                else
                {
                    _Return = "null";
                }
            }
            return _Return;
        }

        private string getNullValuesComparisonString(NullValuesComparison nulComp)
        {
            string _Return = string.Empty;

            switch (nulComp)
            {
                case NullValuesComparison.IsNull:
                    _Return = " is null";
                    break;

                case NullValuesComparison.IsNotNull:
                    _Return = " is not null";
                    break;

                default:
                    break;
            }
            return _Return;
        }

        #endregion private methods
    }
}