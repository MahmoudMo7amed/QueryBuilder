using QueryBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.QueryBuilders
{
    public enum SqlGenerationType
    {
        SqlServer,
        MySql,
        Sybase
    }
    public class SqlFactoryGenerator
    {
        public static ISqlFactory GetSQLFactory(SqlGenerationType SqlType)
        {
            ISqlFactory _SqlFactory = null;
            switch (SqlType)
            {
                case SqlGenerationType.SqlServer:
                    _SqlFactory = new SQLServerFactory();
                    break;
                case SqlGenerationType.MySql:
                    throw new NotImplementedException();
                    break;
                case SqlGenerationType.Sybase:
                    throw new NotImplementedException();
                    break;
                default:
                    break;
            }
            return _SqlFactory;
        }
    }
}
