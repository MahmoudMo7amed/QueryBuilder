using QueryBuilder.Utils;

namespace QueryBuilder
{
    public class Column : IColumn
    {
        private string ColName;
        private ColumnHolder columnHolder;

        public Column(string name, ColumnHolder table, string alias = null)
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

        public ColumnHolder Container { get; set; }

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
                _Return = GetFullyQualifiedName();
            }
            else
            {
                _Return = string.Format("{0} {1}", GetFullyQualifiedName(), this.Alias);
            }
            return _Return;
        }

        public string GetFullyQualifiedName()
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
    }
}