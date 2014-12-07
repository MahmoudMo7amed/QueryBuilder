using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Interfaces;

namespace QueryBuilder
{
    public interface IColumn : IField
    {
        /// <summary>
        /// Container Table ,the table where the column exist
        /// </summary>
        IColumnHolder Container { get; set; }
        string GetFullyQualifiedName();
    }
}
