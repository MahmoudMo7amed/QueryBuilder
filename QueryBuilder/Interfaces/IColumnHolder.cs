using QueryBuilder.Enums;
using System;

namespace QueryBuilder.Interfaces
{
    public interface IColumnHolder<T> : IField
    {
        /// <summary>
        /// Select All Columns
        /// </summary>
        /// <returns></returns>
        T Select();

        T Select(string ColName, string ColAlias = null);

        T SelectFunction(Func<string[], string> functionSql, params string[] parameters);

        T SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters);

        T SelectFunction(Func<string> functionSql, string alias = null);

        T SelectFunction(Function dbFunction);

        T Where(string ColName, NullValuesComparison NullComparison);

        T Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false);

        T Where(string ColName, Query InnerQuery);


        T Having(Function aggregateFunction, ComparisonOperator comparison, double value);
    }
}