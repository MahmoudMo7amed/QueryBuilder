using QueryBuilder.Enums;
using System.Collections.Generic;

namespace QueryBuilder.Interfaces
{
    public interface IQuery : IColumnHolder<IQuery>
    {
        

        IQuery Join(Join join);

        IQuery Join(IColumn LeftTableCol, JoinTypes joinType, IColumn RightTableCol);

        IQuery Union(Query QueryToUnion, bool All = false);

        IQuery GroupBy(IColumn GroupByColumn);

        IQuery Having(Function aggregateFunction, ComparisonOperator comparison, double value);

        IQuery Distinct();

        IQuery OrderBy(IColumn OrderByColumn, OrderByDirection OrderByDir = OrderByDirection.ASC);

        IQuery FromQuery();

        IQuery Count(string Alias = null);

        IQuery Count(IColumn columnName, string Alias = null);

        IQuery Sum(IColumn columnName, string Alias = null);

        IQuery Avg(IColumn columnName, string Alias = null);

        IQuery Max(IColumn columnName, string Alias = null);

        IQuery Min(IColumn columnName, string Alias = null);

        string GenerateSql();
    }
}