using QueryBuilder.Enums;
using QueryBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    //public class ColumnHolder : IField
    //{
    //    private string _alias;
    //    private Dictionary<string, IColumn> _Columns = new Dictionary<string, IColumn>();
    //    private List<WhereCondition> _WhereCondition = new List<WhereCondition>();

    //    #region Properties
    //    public string Name { get; set; }
    //    public string Alias
    //    {
    //        get
    //        {
    //            return _alias;
    //        }
    //        set
    //        {
    //            if (value != null)
    //            {
    //                _alias = value.Trim();
    //            }
    //        }
    //    }
    //    protected Dictionary<string, IColumn> ColumnsDictionary
    //    {
    //        get
    //        {
    //            return _Columns;

    //        }
    //    }
    //    public List<WhereCondition> Conditions
    //    {
    //        get
    //        {
    //            return _WhereCondition;
    //        }
    //    }
    //    public List<IColumn> Columns
    //    {
    //        get
    //        {
    //            return (from b in ColumnsDictionary
    //                    select b.Value).ToList();
    //        }
    //    }

    //    #endregion
  
 
         

    //}
}
