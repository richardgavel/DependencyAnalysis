
namespace Analyzer.Model.Nodes
{
    public class Job
    {
        public Job()
        {
            Type = "Column";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
