using QueryBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryBuilder
{
    internal class SQLServerSqlBuilder : ISqlBuilder
    {
        private Query _Query;
        private List<Table> _lstTable = null;
        private Query _NestedQuery = null;
        private List<Join> _lstJoin = null;
        private List<IColumn> _lstGroupBy = null;
        private List<OrderBy> _lstOrderBy = null;
        internal List<Function> _lstNormalSelectFFunctions = null;
        private List<Function> _lstAggregateFunctions = null;
        private List<Union> _lstQueryUnion = new List<Union>();
        bool IsGroupByContained
        {
            get
            {

                return _lstGroupBy.Count > 0;
            }
        }

        public SQLServerSqlBuilder(Query query)
        {
            this._Query = query;

            this._lstTable = query.TableList;
            this._NestedQuery = query. NestedQuery;
            this._lstJoin = query.Joins;
            this._lstGroupBy = query.GroupByList;
            this._lstOrderBy = query.OrderByList;
            this._lstAggregateFunctions = query.AggregateFunctions;
            this._lstNormalSelectFFunctions = query.NormalSelectFFunctions;
            this._lstQueryUnion = query.Unions;

            //IQueryInternal _IQueryInternal = (IQueryInternal)query;
            //this._lstTable = _IQueryInternal.TableList;
            //this._NestedQuery = _IQueryInternal.NestedQuery;
            //this._lstJoin = _IQueryInternal.Joins;

            //this._lstGroupBy = _IQueryInternal.GroupByList;
            //this._lstOrderBy = _IQueryInternal.OrderByList;
            //this.IsGroupByContained = query.IsGroupByContained;
            //this._lstAggregateFunctions = _IQueryInternal.AggregateFunctions;
            //this._lstNormalSelectFFunctions = _IQueryInternal.NormalSelectFFunctions;
            //this._lstQueryUnion = _IQueryInternal.Unions;


        }

        public string GenerateSQL()
        {
            string _return = GetSelectColumns() + Environment.NewLine +
             GetFrom() + Environment.NewLine + GetWhereCondition() +
                  Environment.NewLine + GetGroupBy() + Environment.NewLine + GetHavingClause() + Environment.NewLine + GetOrderBy();
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

        private string GetSelectColumns()
        {
            string _Return = string.Empty;
            if (_lstTable == null && _NestedQuery == null && _lstAggregateFunctions == null) throw new InvalidOperationException("no select columns suppliedto the query ");
            if (_lstTable != null) //so this is Query from Table
            {
                _Return = "select " + getDistinct();
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

                if (_Query.SelectAll)
                {
                    _Return += "select "+ getDistinct()+" * ";
                }
                string Result = getSelectColumnsString(_Query.Columns);
                if (!string.IsNullOrEmpty(Result))
                {
                    _Return += "select " + getDistinct()+ Result;
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
                            _Return = "select " + getDistinct();
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


        private string getDistinct()
        {
            string _Return = string.Empty;
            if (_Query.IsDistinct)
            {
                _Return = " distinct ";
            }
            return _Return;
        }

        private string GetFrom()
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
                            _Return += item.ToString();
                        }
                        else
                        {
                            _Return += item.getJoinBeginingFromJoinType();
                        }
                    }
                }
            }
            else if (_NestedQuery != null)
            {
                _Return += "(" + _NestedQuery.GenerateSql() + ")" + _Query.Alias;
            }
            return _Return;
        }

        private string GetWhereCondition()
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
                _StringBuilder.Append(getWhereConditionString(_Query.Conditions));
            }

            string _str = _StringBuilder.ToString();
            if (!string.IsNullOrEmpty(_str))
            {
                _str = " Where " + _str;
            }
            return _str;
        }

        private string GetHavingClause()
        {
            StringBuilder _StringBuilder = new StringBuilder();

            //if (_lstTable != null)
            //{
            //    foreach (var item in _lstTable)
            //    {
            //        _StringBuilder.Append(getHavingClauseString(item.HavingClause));
            //    }
            //}
            //else if (_NestedQuery != null)
            //{
                _StringBuilder.Append(getHavingClauseString(_Query.HavingClause));
            //}

            string _str = _StringBuilder.ToString();
            if (!string.IsNullOrEmpty(_str))
            {
                _str = " Having " + _str;
            }
            return _str;
        }

        private string GetGroupBy()
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

        private string GetOrderBy()
        {
            string _Return = string.Empty;
            if (_lstOrderBy.Count > 0)
            {
                _Return = " order by ";
                foreach (var item in _lstOrderBy)
                {
                    if (item == _lstOrderBy.First())
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


        private string getSelectColumnsString(List<IColumn> Cols)
        {
            StringBuilder _StringBuilder = new StringBuilder();
            foreach (var col in Cols)
            {
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
                _Sp.Append(Conditions[i].ToString() + " and ");
            }
            if (Count >= 0)//count could be -1 if the table has no columns to select .
            {
                _Sp.Append(Conditions[Count].ToString());
            }
            return _Sp.ToString();
        }

        private string getHavingClauseString(List<Having> Havings)
        {
            StringBuilder _Sp = new StringBuilder();
            int Count = Havings.Count - 1;

            for (int i = 0; i < Count; i++)
            {
                _Sp.Append(Havings[i].ToString() + " and ");
            }
            if (Count >= 0)//count could be -1 if the table has no columns to select .
            {
                _Sp.Append(Havings[Count].ToString());
            }
            return _Sp.ToString();
        }
    }
}