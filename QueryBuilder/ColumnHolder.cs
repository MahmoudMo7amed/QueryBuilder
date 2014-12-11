using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder
{
    public class ColumnHolder : IColumnHolder<ColumnHolder>, IField
    {
        private string _alias;
        private Dictionary<string, IColumn> _Columns = new Dictionary<string, IColumn>();
        private List<WhereCondition> _WhereCondition = new List<WhereCondition>();
     

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
        //private List<Having> _Having = new List<Having>();
        //public List<Having> HavingClause
        //{
        //    get
        //    {
        //        return _Having;
        //    }
        //}

        public List<IColumn> Columns
        {
            get
            {
                return (from b in ColumnsDictionary
                        select b.Value).ToList();
            }
        }

        #endregion Properties

        internal bool SelectAll = true;

        public ColumnHolder Select()
        {
            SelectAll = true;
            return this;
        }

        public ColumnHolder Select(string ColName, string ColAlias = null)
        {
            SelectAll = false;
            Column Col = new Column(ColName, this, ColAlias);
            ColumnsDictionary.Add(Col.Name, Col);
            return this;
        }

        protected List<Function> _lstNormalSelectFFunctions = new List<Function>();

        public ColumnHolder SelectFunction(Func<string[], string> functionSql, params string[] parameters)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(new Function(functionSql, parameters));
            return this;
        }

        public ColumnHolder SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(new Function(alias, functionSql, parameters));
            return this;
        }

        public ColumnHolder SelectFunction(Func<string> functionSql, string alias = null)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(new Function(functionSql, alias));
            return this;
        }

        public ColumnHolder SelectFunction(Function dbFunction)
        {
            SelectAll = false;
            _lstNormalSelectFFunctions.Add(dbFunction);
            return this;
        }

        public ColumnHolder Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        {
            IColumn Col = getColumnOrCreateIfNotExist(ColName);
            if (value == null && AcceptNullValue)
            {
                Conditions.Add(new WhereCondition(Col, ComparisonOperator, value));
            }
            else if (value != null)
            {
                Conditions.Add(new WhereCondition(Col, ComparisonOperator, value));
            }
            return this;
        }

        public ColumnHolder Where(string ColName, NullValuesComparison NullComparison)
        {
            IColumn Col = getColumnOrCreateIfNotExist(ColName);
            Conditions.Add(new WhereCondition(Col, NullComparison));
            return this;
        }

        public ColumnHolder Where(string ColName, Query InnerQuery)
        {
            Conditions.Add(new WhereCondition(getColumnOrCreateIfNotExist(ColName), InnerQuery));
            return this;
        }

     

        protected IColumn getColumnOrCreateIfNotExist(string ColName)
        {
            IColumn _Column;
            bool ColumnExist = ColumnsDictionary.TryGetValue(ColName, out _Column);
            if (!ColumnExist)
            {
                _Column = new Column(ColName, this);
            }
            return _Column;
        }


    }
}