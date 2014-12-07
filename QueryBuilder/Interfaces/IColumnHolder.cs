using QueryBuilder.Enums;
using System;
using System.Collections.Generic;

namespace QueryBuilder.Interfaces
{
    public interface IColumnHolder : IField
    {
        //List<WhereCondition> Conditions { get; }
        //List<IColumn> Columns { get; }
    }
    public interface IColumnHolder<T> : IColumnHolder
    {

        /// <summary>
        /// Select All Columns
        /// </summary>
        /// <returns></returns>
        ///

        T Select();

        T Select(string ColName);

        T Select(string ColName, string ColAlias);

        T Select(IColumn Col);

        T SelectFunction(Func<string[], string> functionSql, params string[] parameters);

        T SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters);

        T SelectFunction(Func<string> functionSql, string alias = null);

        T SelectFunction(Function dbFunction);

        T Where(IColumn Col, NullValuesComparison NullComparison);

        T Where(string ColName, NullValuesComparison NullComparison);

        T Where(IColumn Col, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false);

        T Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false);

        T Where(string ColName, Query InnerQuery);

        T Where(IColumn Col, Query InnerQuery);
    }
}
