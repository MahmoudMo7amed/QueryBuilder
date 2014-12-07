using QueryBuilder.Interfaces;
using QueryBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class Column : IColumn
    {
        private string ColName;
        private IColumnHolder columnHolder;


        public Column(string name, IColumnHolder table, string alias = null)
        {
            this.Container = table;

            string[] ar = AliasUtil.getNameAndAlias(name);
            if (ar != null)
            {
                this.Name = ar[0];
                this.Alias = ar[1];
            }
            else
            {
                this.Name = name;
                if (alias != null)
                {
                    this.Alias = alias.Trim();
                }
            }
        }

        public Column(string name, string TableName)
            : this(name, new Table(TableName))
        {

        }

        public IColumnHolder Container { get; set; }

        public string Name { get; set; }
        public string Alias { get; set; }


        /// <summary>
        /// //to be used in select statment
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string _Return = string.Empty;
            if (string.IsNullOrEmpty(Alias))
            {
                _Return = ColumnFullyQualifiedName();
            }
            else
            {
                _Return = string.Format("{0} {1}", ColumnFullyQualifiedName(), this.Alias);
            }
            return _Return;

        }
        public string GetFullyQualifiedName()
        {
            return ColumnFullyQualifiedName();
        }

        #region Private functions
        string ColumnFullyQualifiedName()
        {
            string _Return;
            if (string.IsNullOrEmpty(Container.Alias))
            {
                if (string.IsNullOrEmpty(Container.Name))
                {
                    _Return = this.Name;
                }
                else
                {
                    _Return = string.Format("{0}.{1}", Container.Name, this.Name);
                }

            }
            else
            {
                _Return = string.Format("{0}.{1}", Container.Alias, this.Name);
            }
            return _Return;
        }

        void ParseName(string name)
        {

        }
        #endregion




    }
}
