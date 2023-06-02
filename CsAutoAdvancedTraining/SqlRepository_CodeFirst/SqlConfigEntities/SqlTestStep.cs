using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlRepository_CodeFirst.SqlConfigEntities
{
    public class SqlTestStep
    {
        public int Id { get; set; }

        public int StepNumber { get; set; }

        public string StepText { get; set; }

        public int TestId { get; set; }

        public SqlTest Test { get; set; }
    }
}
