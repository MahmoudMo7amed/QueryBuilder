using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Interfaces
{
   public interface ISqlBuilder
    {

        string GenerateSQL( );
    }
}
