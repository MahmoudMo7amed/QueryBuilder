using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Interfaces
{
    internal interface IQueryInternal
    {
        #region Properties

        List<Join> Joins { get; }

        List<Table> TableList { get; }

        List<IColumn> GroupByList { get; }

        List<OrderBy> OrderByList { get; }

        List<Function> NormalSelectFFunctions { get; }

        List<Function> AggregateFunctions { get; }

        List<Union> Unions { get; }

        Query NestedQuery { get; }

        List<Having> HavingClause { get; }

        bool IsDistinct { get; }

        QueryConfiguration Configuration { get; }

        #endregion Properties



    }
}
