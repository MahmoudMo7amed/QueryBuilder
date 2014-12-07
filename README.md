QueryBuilder
============
QueryBuilder is to create dynamic sql queries at runtime 
Ex: 
 Table _Table = new Table("TableName", "Column1", "Column2");
            IColumn f = _Table["Column1"]; //to get columns
            _Table.Where("Column1", ComparisonOperator.Equal, YOUR VALUE,false)//false means if you path null value it will not be added to the query ,this saves alot of code checking for null values before sending it to the where function
                     .Where("Column2", ComparisonOperator.GreaterEqual, YOUR VALUE)
                     .Where("Column3", ComparisonOperator.LessEqual, YOUR VALUE)
                     .Where("Column4", ComparisonOperator.Equal, YOUR VALUE)
                     .Where("Column5", ComparisonOperator.Equal, YOUR VALUE);
            Table _Table2 = new Table("Table2", "Column1 'ColumnAlias'");
			string _strAlias=_Table2["Column1"].Alias;//this should return ColumnAlias
 
            Query _Query = new Query(_Table1, _Table2);//Create the query 
            _Query.Join(new Column("Column2", _Table1), Enums.JoinTypes.InnerJoin, new Column("Column1", _Table2)) // Create the join
              .Join(new Column("Id", _Table1), Enums.JoinTypes.InnerJoin, new Column("Id", _Table2)) //create another join 
			  .Count() //To Count the result 

            string _strSQL=_Query.GenerateSql();