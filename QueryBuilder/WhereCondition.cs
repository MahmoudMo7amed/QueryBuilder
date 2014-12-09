using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
using QueryBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class WhereCondition
    {

        private object _ObjValue;
        ComparisonOperator? _ComparisonOperator = null;
        NullValuesComparison? _NullValuesComparison = null;
        IColumn _Column = null;
        IQuery _Query;
        public IColumn Column { get { return _Column; } }
        public object Value { get { return _ObjValue; } }
        public ComparisonOperator? CompOperator { get { return _ComparisonOperator; } }
        public NullValuesComparison? NullValComparison { get { return _NullValuesComparison; } }
        public WhereCondition(IColumn Col, ComparisonOperator comOperator, object value)
        {
            this._ObjValue = value;
            this._ComparisonOperator = comOperator;
            this._Column = Col;
        }
        public WhereCondition(IColumn Col, NullValuesComparison nullValueCom)
        {
            this._NullValuesComparison = nullValueCom;
            this._Column = Col;
        }
        public WhereCondition(IColumn Col, IQuery query)
        {
            this._Column = Col;
            this._Query = query;
        }
        public override string ToString()
        {
            string _Return = string.Empty;
            if (this._ComparisonOperator != null)
            {
                if (this._ComparisonOperator.Value == ComparisonOperator.Like)
                {
                    _Return = string.Format("{0} {1} '%{2}%'", this._Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(this._ComparisonOperator.Value), getValue(this._ObjValue));
                }
                else if (this._ComparisonOperator.Value == ComparisonOperator.StartLike)
                {
                    _Return = string.Format("{0} {1}  '{2}%'", this._Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(this._ComparisonOperator.Value), getValue(this._ObjValue));
                }
                else if (this._ComparisonOperator.Value == ComparisonOperator.EndLike)
                {
                    _Return = string.Format("{0} {1} '%{2}'", this._Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(this._ComparisonOperator.Value), getValue(this._ObjValue));
                }

                else
                {
                    if (this._ObjValue is string)
                    {
                        _Return = string.Format("{0} {1} '{2}'", this._Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(this._ComparisonOperator.Value), this._ObjValue.ToString());

                    }
                    else
                    {
                        _Return = string.Format("{0} {1} {2}", this._Column.GetFullyQualifiedName(), SqlOperatorEnumConverter.getComparisonOperatorString(this._ComparisonOperator.Value), getValue(this._ObjValue));
                    }
                }
            }
            else if (this._NullValuesComparison != null)
            {
                _Return = string.Format("{0} {1}", this._Column.ToString(), getNullValuesComparisonString(this._NullValuesComparison.Value));

            }
            else if (this._Query != null)
            {
                _Return = string.Format("{0} in ({1})", this._Column.ToString(), _Query.GenerateSql());

            }
            return _Return;
        }

        #region Private Functions
        string getValue(object value)
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
        string getNullValuesComparisonString(NullValuesComparison nulComp)
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

        #endregion
    }
}
