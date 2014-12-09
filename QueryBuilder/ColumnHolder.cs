using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class ColumnHolder : IField
    {
        private string _alias;
        private Dictionary<string, IColumn> _Columns = new Dictionary<string, IColumn>();
        private List<WhereCondition> _WhereCondition = new List<WhereCondition>();
        private List<Having> _Having = new List<Having>();

        #region Properties
        public string Name { get; set; }
        public string Alias
        {
            get
            {
                return _alias;
            }
            set
            {
                if (value != null)
                {
                    _alias = value.Trim();
                }
            }
        }
        protected Dictionary<string, IColumn> ColumnsDictionary
        {
            get
            {
                return _Columns;

            }
        }
        public List<WhereCondition> Conditions
        {
            get
            {
                return _WhereCondition;
            }
        }
        public List<Having> HavingClause
        {
            get
            {
                return _Having;
            }
        }
        public List<IColumn> Columns
        {
            get
            {
                return (from b in ColumnsDictionary
                        select b.Value).ToList();
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Col"></param>
        /// <param name="ComparisonOperator"></param>
        /// <param name="value"></param>
        /// <param name="AcceptNullValue">if true it will add null values to where condition.
        /// the default is false which means that if the user pass null value it will not be added to 
        /// where condition ,this is to let the caller not to check for null values befor sending the object
        /// 
        /// </param>

        //public ColumnHolder AddSelectColumns(string ColName)
        //{

        //    Column Col = new Column(ColName, this);
        //    this._Columns.Add(Col.Name, Col);
        //    return this;
        //}
        //public ColumnHolder AddSelectColumns(string ColName, string ColAlias)
        //{
        //    Column Col = new Column(ColName, this, ColAlias);
        //    this._Columns.Add(Col.Name, Col);
        //    return this;
        //}
        //public ColumnHolder AddSelectColumns(IColumn Col)
        //{
        //    this._Columns.Add(Col.Name, Col);
        //    return this;
        //}

        //public ColumnHolder addWhereCondition(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        //{
        //    //ArgumentException _ArgExcp = CheckForColumnExistance(ColName);
        //    //if (_ArgExcp != null) throw _ArgExcp;

        //    addWhereCondition(getColumn(ColName), ComparisonOperator, value, AcceptNullValue);

        //    return this;
        //}
        //public ColumnHolder addWhereCondition(IColumn Col, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        //{

        //    if (value == null && AcceptNullValue)
        //    {
        //        _WhereCondition.Add(new WhereCondition(Col, ComparisonOperator, value));
        //    }
        //    else if (value != null)
        //    {
        //        _WhereCondition.Add(new WhereCondition(Col, ComparisonOperator, value));
        //    }

        //    return this;
        //}
        //public ColumnHolder addWhereCondition(string ColName, NullValuesComparison NullComparison)
        //{
        //    //ArgumentException _ArgExcp = CheckForColumnExistance(ColName);
        //    //if (_ArgExcp != null) throw _ArgExcp;
        //    //_WhereCondition.Add(new WhereCondition(_Columns[ColName], NullComparison));
        //    addWhereCondition(getColumn(ColName), NullComparison);
        //    return this;
        //}
        //public ColumnHolder addWhereCondition(IColumn Col, NullValuesComparison NullComparison)
        //{
        //    _WhereCondition.Add(new WhereCondition(Col, NullComparison));
        //   // addWhereCondition(Col.Name, NullComparison);
        //    return this;
        //}
        //public ColumnHolder addWhereCondition(string ColName, Query InnerQuery)
        //{
        //    _WhereCondition.Add(new WhereCondition(getColumn( ColName), InnerQuery));
        //    return this;
        //}
        //public ColumnHolder addWhereCondition(IColumn Col, Query InnerQuery)
        //{
        //    _WhereCondition.Add(new WhereCondition(Col, InnerQuery));
        //    return this;
        //}

    }
}
