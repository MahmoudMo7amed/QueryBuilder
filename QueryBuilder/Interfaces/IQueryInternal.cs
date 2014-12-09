using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Interfaces
{
    internal interface IQueryInternal
    {
        List<Join> Joins { get; }
        List<Table> TableList { get; }
        List<IColumn> GroupByList { get; }
        List<OrderBy> OrderByList { get; }
        List<Function> NormalSelectFFunctions { get; }
        List<Function> AggregateFunctions { get; }

        Query NestedQuery { get; }
    }
}
