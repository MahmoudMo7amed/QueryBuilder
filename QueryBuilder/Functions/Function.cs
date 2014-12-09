using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QueryBuilder
{
    public class Function : IField
    {
        private string[] _strlstparameters;

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Alias { get; set; }

        private Func<string[], string> _ParametrizedfunctionSql;

        private Func<string> _NonParametrizedfunctionSql;

        public Function(Func<string[], string> functionSql, params string[] parameters)
        {
            _ParametrizedfunctionSql = functionSql;
            _strlstparameters = parameters;
        }

        public Function(string alias, Func<string[], string> functionSql, params string[] parameters)
            : this(functionSql, parameters)
        {
            this.Alias = alias;
        }

        public Function(Func<string> functionSql, string alias = null)
        {
            _NonParametrizedfunctionSql = functionSql;
            this.Alias = alias;
        }

        public override string ToString()
        {
            string _Return = string.Empty;

            if (_NonParametrizedfunctionSql != null)
            {
                _Return = _NonParametrizedfunctionSql();
            }
            else if (_ParametrizedfunctionSql != null)
            {
                _Return = _ParametrizedfunctionSql(_strlstparameters);
            }
            if (!string.IsNullOrEmpty(Alias))
            {
                _Return = string.Format(_Return + " {0}", this.Alias);
            }

            return _Return;
        }

        #region static functions

        /// <summary>
        /// SimpleFunction is functions that takes no Parameter or only one parameter which is ColumnName
        /// Ex:Count ,Sum ,Avg ,..etc
        /// </summary>
        /// <returns></returns>



        #endregion static functions
        public class AggregateFunctions
        {
            public static Function Count(string Alias = null)
            {
                return FunctionFormateReader.GetSimpleFunction("count", "*", Alias);
            }

            public static Function Count(IColumn Col, string Alias = null)
            {
                if (Col == null) throw new ArgumentException("column name should be provided for the sum function");

                return FunctionFormateReader.GetSimpleFunction("count", Col.ToString(), Alias);
            }

            public static Function Sum(IColumn Col, string Alias = null)
            {
                if (Col == null) throw new ArgumentException("column name should be provided for the sum function");

                return FunctionFormateReader.GetSimpleFunction("sum", Col.ToString(), Alias);
            }

            public static Function Avg(IColumn Col, string Alias = null)
            {
                if (Col == null) throw new ArgumentException("column name should be provided for the avg function");

                return FunctionFormateReader.GetSimpleFunction("avg", Col.ToString(), Alias);
            }

            public static Function Max(IColumn Col, string Alias = null)
            {
                if (Col == null) throw new ArgumentException("column name should be provided for the max function");

                return FunctionFormateReader.GetSimpleFunction("max", Col.ToString(), Alias);
            }

            public static Function Min(IColumn Col, string Alias = null)
            {
                if (Col == null) throw new ArgumentException("column name should be provided for the min function");

                return FunctionFormateReader.GetSimpleFunction("min", Col.ToString(), Alias);
            }

        }

        public class PagingFunctions
        {
            public static Function GetRowNumber(string columnName, string Alias, OrderByDirection direction = OrderByDirection.DESC)
            {
                if (string.IsNullOrEmpty(Alias)) throw new ArgumentException("Alias for GetRowNumber function cannot be null");
                string _strFunctionSql = string.Format("ROW_NUMBER() OVER(ORDER BY {0} " + direction.ToString() + ") ", columnName);
                Function _Return = new Function(() => { return _strFunctionSql; }, Alias);
                return _Return;
            }

        }
    }
}