
namespace Analyzer.Model.Nodes
{
    public class ReportServer
    {
        public ReportServer()
        {
            Type = "ReportServer";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
