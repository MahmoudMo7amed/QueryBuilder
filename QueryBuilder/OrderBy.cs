using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class OrderBy
    {
        IColumn OrderByCol;
        OrderByDirection Dir;
        public IColumn OrderByColumn
        {
            get
            {

                return OrderByCol;
            }
        }

        public OrderByDirection Direction
        {
            get
            {

                return Dir;
            }
        }
        public OrderBy(IColumn orderByCol, OrderByDirection dir = OrderByDirection.ASC)
        {
            this.OrderByCol = orderByCol;
            this.Dir = dir;
        }
        //public override string ToString()
        //{
        //    return OrderByCol.ToString() + " " + Dir.ToString();
        //}
    }
}
