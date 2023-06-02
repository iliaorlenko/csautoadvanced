namespace SqlRepository_CodeFirst.SqlConfigEntities
{
    public class SqlTest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ExpectedResult { get; set; }

        public List<SqlTestStep> Steps { get; set; }

        public int UserId { get; set; }

        public SqlUser User { get; set; }  
    }
}
