using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class Union
    {
        public Union(Query query, bool All = false)
        {
            QueryToUnion = query;
            UnionAll = All;

        }
        public Query QueryToUnion { get; set; }
        public bool UnionAll { get; set; }
    }
}
