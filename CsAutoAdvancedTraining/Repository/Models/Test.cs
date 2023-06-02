namespace Repository.Models
{
    public class Test
    {
        public string Title { get; set; }

        public string ExpectedResult { get; set; }

        public List<TestStep> Steps { get; set; }
    }
}
