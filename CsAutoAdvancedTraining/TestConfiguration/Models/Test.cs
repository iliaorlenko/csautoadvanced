using System.Collections.Generic;

namespace TestConfiguration.Models
{
    public class Test
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ExpectedResult { get; set; }

        public List<TestStep> Steps { get; set; }
    }
}
