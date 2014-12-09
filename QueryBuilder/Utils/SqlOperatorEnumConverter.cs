using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Utils
{
    public class SqlOperatorEnumConverter
    {
        public static string getComparisonOperatorString(ComparisonOperator CompOperator)
        {
            string _Return = string.Empty;

            switch (CompOperator)
            {
                case ComparisonOperator.Equal:
                    _Return = "=";
                    break;
                case ComparisonOperator.NotEqual:
                    _Return = "<>";
                    break;
                case ComparisonOperator.Greater:
                    _Return = ">";
                    break;
                case ComparisonOperator.Less:
                    _Return = "<";
                    break;
                case ComparisonOperator.GreaterEqual:
                    _Return = ">=";
                    break;
                case ComparisonOperator.LessEqual:
                    _Return = "<=";
                    break;
                case ComparisonOperator.Like:
                    _Return = "like ";
                    break;
                case ComparisonOperator.StartLike:
                    _Return = "like ";
                    break;
                case ComparisonOperator.EndLike:
                    _Return = "like ";
                    break;
                default:
                    break;
            }
            return _Return;
        }
    }
}
