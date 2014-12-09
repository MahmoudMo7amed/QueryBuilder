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
    public class Table : ColumnHolder, ITable
    {
        #region properties
        public IColumn IdColumn { get; private set; }
        public IColumn this[string ColName]
        {
            get
            {
                return ColumnsDictionary[ColName];
            }
            set
            {
                //string _f = ColName;
                if (value != null)
                {
                    ColumnsDictionary.Add(ColName, value);
                }

            }
        }
        #endregion

        #region Constructors
        public Table(string name)
        {
            string[] ar = AliasUtil.getNameAndAlias(name);
            if (ar != null)
            {
                this.Name = ar[0];
                this.Alias = ar[1];
            }
            else
            {
                this.Name = name;
            }
        }
        public Table(string name, params IColumn[] Columns)
            : this(name)
        {
            foreach (var item in Columns)
            {
                ColumnsDictionary.Add(item.Name, item);
            }
        }
        public Table(string name, params string[] Columns)
            : this(name)
        {
            foreach (var columnName in Columns)
            {
                Column Col = new Column(columnName, this);
                ColumnsDictionary.Add(Col.Name, Col);

            }
        }
        #endregion

        #region public methods
        public ITable SetIdColumn(string IdColName)
        {
            this.IdColumn = new Column(IdColName, this);
            return this;
        }

        internal bool SelectAll = false;
        public ITable Select()
        {
            SelectAll = true;
            return this;
        }
        public ITable Select(string ColName, string ColAlias = null)
        {
            SelectAll = false;
            Column Col = new Column(ColName, this, ColAlias);
            ColumnsDictionary.Add(Col.Name, Col);
            return this;
        }
        //public ITable Select(string ColName, string ColAlias)
        //{
        //    SelectAll = false;
        //    Column Col = new Column(ColName, this, ColAlias);
        //    ColumnsDictionary.Add(Col.Name, Col);
        //    return this;
        //}
        //public ITable Select(IColumn Col)
        //{
        //    SelectAll = false;
        //    ColumnsDictionary.Add(Col.Name, Col);
        //    return this;
        //}

        public ITable Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        {
            // Where(getColumnOrCreateIfNotExist(ColName), ComparisonOperator, value, AcceptNullValue);


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
        //public ITable Where(IColumn Col, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        //{

        //    if (value == null && AcceptNullValue)
        //    {
        //        Conditions.Add(new WhereCondition(Col, ComparisonOperator, value));
        //    }
        //    else if (value != null)
        //    {
        //        Conditions.Add(new WhereCondition(Col, ComparisonOperator, value));
        //    }

        //    return this;
        //}
        public ITable Where(string ColName, NullValuesComparison NullComparison)
        {
            // Where(getColumnOrCreateIfNotExist(ColName), NullComparison);
            IColumn Col = getColumnOrCreateIfNotExist(ColName);
            Conditions.Add(new WhereCondition(Col, NullComparison));
            return this;
        }
        //public ITable Where(IColumn Col, NullValuesComparison NullComparison)
        //{
        //    Conditions.Add(new WhereCondition(Col, NullComparison));
        //    // addWhereCondition(Col.Name, NullComparison);
        //    return this;
        //}
        public ITable Where(string ColName, Query InnerQuery)
        {
            Conditions.Add(new WhereCondition(getColumnOrCreateIfNotExist(ColName), InnerQuery));
            return this;
        }
        //public ITable Where(IColumn Col, Query InnerQuery)
        //{
        //    Conditions.Add(new WhereCondition(Col, InnerQuery));
        //    return this;
        //}



        public override string ToString()
        {
            if (string.IsNullOrEmpty(Alias))
            {
                return this.Name;
            }
            else
            {
                return this.Name + " " + this.Alias;
            }
        }

        #endregion

        IColumn getColumnOrCreateIfNotExist(string ColName)
        {
            IColumn _Column;
            bool ColumnExist = ColumnsDictionary.TryGetValue(ColName, out _Column);
            if (!ColumnExist)
            {
                _Column = new Column(ColName, this);
            }
            return _Column;
        }

        private List<Function> _lstNormalSelectFFunctions = new List<Function>();

        public ITable SelectFunction(Func<string[], string> functionSql, params string[] parameters)
        {
            _lstNormalSelectFFunctions.Add(new Function(functionSql, parameters));
            return this;
        }

        public ITable SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters)
        {
            _lstNormalSelectFFunctions.Add(new Function(alias, functionSql, parameters));
            return this;
        }

        public ITable SelectFunction(Func<string> functionSql, string alias = null)
        {
            _lstNormalSelectFFunctions.Add(new Function(functionSql, alias));
            return this;
        }

        public ITable SelectFunction(Function dbFunction)
        {
            _lstNormalSelectFFunctions.Add(dbFunction);
            return this;
        }
    }
}
