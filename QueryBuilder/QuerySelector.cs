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
    public class QuerySelector : Query, IColumnHolder<QuerySelector>
    {


        private string _alias;
        private Dictionary<string, IColumn> ColumnsDictionary = new Dictionary<string, IColumn>();
        private List<WhereCondition> _WhereCondition = new List<WhereCondition>();
        private Query _NestedQuery = null;
        private List<Function> _lstNormalSelectFFunctions = new List<Function>();

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
        #endregion
        internal List<WhereCondition> Conditions
        {
            get
            {
                return _WhereCondition;
            }
        }
        internal List<IColumn> Columns
        {
            get
            {
                return (from b in ColumnsDictionary
                        select b.Value).ToList();
            }
        }
        internal Query NestedQuery
        {
            get { return _NestedQuery; }
        }
        internal List<Function> NormalSelectFFunctions
        {
            get { return _lstNormalSelectFFunctions; }
        }

        public QuerySelector(Query q)
        {
            this._NestedQuery = q;
            if (q is QuerySelector)
            {
                var qselector = q as QuerySelector;
                this.Name = this.Alias = AliasUtil.GetNextAlias(qselector.Alias);
            }
            else
            {
                this.Name = this.Alias = AliasUtil.GetAlias();
            }
        }
        public QuerySelector Select()
        {
            SelectAll = true;
            return this;
        }

        public QuerySelector Select(string ColName)
        {
            SelectAll = false;
            Column Col = new Column(ColName, this);
            ColumnsDictionary.Add(Col.Name, Col);
            return this;
        }

        public QuerySelector Select(string ColName, string ColAlias)
        {
            SelectAll = false;
            Column Col = new Column(ColName, this, ColAlias);
            ColumnsDictionary.Add(Col.Name, Col);
            return this;
        }

        public QuerySelector Select(IColumn Col)
        {
            SelectAll = false;
            ColumnsDictionary.Add(Col.Name, Col);
            return this;
        }

        public QuerySelector SelectFunction(Func<string[], string> functionSql, params string[] parameters)
        {
            _lstNormalSelectFFunctions.Add(new Function(functionSql, parameters));
            return this;
        }

        public QuerySelector SelectFunction(string alias, Func<string[], string> functionSql, params string[] parameters)
        {
            _lstNormalSelectFFunctions.Add(new Function(alias, functionSql, parameters));
            return this;
        }

        public QuerySelector SelectFunction(Func<string> functionSql, string alias = null)
        {
            _lstNormalSelectFFunctions.Add(new Function(functionSql, alias));
            return this;
        }

        public QuerySelector SelectFunction(Function dbFunction)
        {
            _lstNormalSelectFFunctions.Add(dbFunction);
            return this;
        }

        public QuerySelector Where(string ColName, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        {
            CheckForColumnExistance(ColName);

            if (value == null && AcceptNullValue)
            {
                Conditions.Add(new WhereCondition(ColumnsDictionary[ColName], ComparisonOperator, value));
            }
            else if (value != null)
            {
                Conditions.Add(new WhereCondition(ColumnsDictionary[ColName], ComparisonOperator, value));
            }
            return this;
        }

        public QuerySelector Where(IColumn Col, ComparisonOperator ComparisonOperator, object value, bool AcceptNullValue = false)
        {
            Where(Col.Name, ComparisonOperator, value, AcceptNullValue);
            return this;
        }

        public QuerySelector Where(string ColName, NullValuesComparison NullComparison)
        {
            CheckForColumnExistance(ColName);

            Conditions.Add(new WhereCondition(ColumnsDictionary[ColName], NullComparison));

            return this;
        }

        public QuerySelector Where(IColumn Col, NullValuesComparison NullComparison)
        {
            Where(Col.Name, NullComparison);
            return this;
        }

        public QuerySelector Where(IColumn Col, Query InnerQuery)
        {
            Where(Col.Name, InnerQuery);
            return this;
        }

        public QuerySelector Where(string ColName, Query InnerQuery)
        {
            CheckForColumnExistance(ColName);
            Conditions.Add(new WhereCondition(ColumnsDictionary[ColName], InnerQuery));
            return this;
        }

        #region Private Methods

        private void CheckForColumnExistance(string ColName)
        {

            IColumn _Column;
            bool ColumnExist = ColumnsDictionary.TryGetValue(ColName, out _Column);
            throw new ArgumentException("Column " + ColName + " does not exist on " + this.Name + " table");
        }

        #endregion

    }
}