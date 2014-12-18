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
            if (Columns.Length > 0)
            {
                SelectAll = false;
            }
            foreach (var item in Columns)
            {

                ColumnsDictionary.Add(item.Name, item);
            }
        }
        public Table(string name, params string[] Columns)
            : this(name)
        {
            if (Columns.Length > 0)
            {
                SelectAll = false;
            }
            foreach (var columnName in Columns)
            {
                Column Col = new Column(columnName, this);
                ColumnsDictionary.Add(Col.Name, Col);

            }
        }
        #endregion

        #region public methods


        public new ITable Select()
        {
            base.Select();
            return this;
        }
        public new ITable Select(string ColName, string ColAlias = null)
        {
            base.Select(ColName, ColAlias);
            return this;
        }
        public new ITable SelectFunction(Func<string[], string> functionSql, params string[] parameters)
        {
            base.SelectFunction(functionSql, parameters);
            return this;
        }

        public ITable SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters)
        {
            base.SelectFunction(alias, functionSql, parameters);
            return this;
        }

        public new ITable SelectFunction(Func<string> functionSql, string alias = null)
        {
            base.SelectFunction(functionSql, alias);
            return this;
        }

        public new ITable SelectFunction(Function dbFunction)
        {
            base.SelectFunction(dbFunction);
            return this;
        }


        public new ITable Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        {

            base.Where(ColName, ComparisonOperator, value, AcceptNullValue);
            return this;
        }

        public new ITable Where(string ColName, NullValuesComparison NullComparison)
        {
            base.Where(ColName, NullComparison);
            return this;
        }

        public new ITable Where(string ColName, Query InnerQuery)
        {
            base.Where(ColName, InnerQuery);
            return this;
        }




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
    }
}
