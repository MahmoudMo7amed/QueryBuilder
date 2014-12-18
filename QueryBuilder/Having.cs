using QueryBuilder.Enums;
using QueryBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class Having
    {
        public Function AggregateFunction { get; set; }
        public ComparisonOperator Comparison { get; set; }
        public double Value { get; set; }
        public Having(Function aggregateFunction, ComparisonOperator comparison, double value)
        {
            this.AggregateFunction = aggregateFunction;
            this.Comparison = comparison;
            this.Value = value;
        }

        //public override string ToString()
        //{
        //    string _Return = string.Empty;
        //    if (Comparison==ComparisonOperator.Like
        //        || Comparison == ComparisonOperator.StartLike
        //        || Comparison == ComparisonOperator.EndLike
        //        )
        //    {
        //        throw new ArgumentException("havine clause could not have [like; operator");
        //    }

        //    _Return = string.Format("{0} {1} {2}", AggregateFunction.ToString(), SqlOperatorEnumConverter.getComparisonOperatorString(this.Comparison), Value);
     
        //    return _Return;
        //}

    }
}
