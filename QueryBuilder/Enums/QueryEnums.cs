using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Enums
{

    public enum OrderByDirection
    {
        ASC,
        DESC
    }
    public enum JoinTypes
    {
        InnerJoin,
        LeftOuterJoin,
        RightOuterJoin,
        FullOuterJoin
    }
    public enum ConditionOperator
    {
        And,
        Or
    }
    public enum ComparisonOperator
    {
        Equal,
        NotEqual,
        Greater,
        Less,
        GreaterEqual,
        LessEqual,
        Like,
        StartLike,
        EndLike
    }
    public enum NullValuesComparison
    {
        IsNull,
        IsNotNull 
 
    }

}
