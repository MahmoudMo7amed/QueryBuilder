namespace QueryBuilder.Interfaces
{
    public interface ISqlFactory
    {
        string GenerateSQL(WhereCondition condition);

        string GenerateSQL(OrderBy ordrby);

        string GenerateSQL(Join join);

        string GenerateSQL(Having havingClause);

        string GenerateSQL(Query query);
    }
}