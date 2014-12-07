using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public interface IField
    {
        string Name { get; set; }
        string Alias { get; set; }
    }
}
