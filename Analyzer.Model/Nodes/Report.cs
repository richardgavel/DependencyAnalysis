
namespace Analyzer.Model.Nodes
{
    public class Report
    {
        public Report()
        {
            Type = "Report";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
