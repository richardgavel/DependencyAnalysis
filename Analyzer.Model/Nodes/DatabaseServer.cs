
namespace Analyzer.Model.Nodes
{
    public class DatabaseServer
    {
        public DatabaseServer()
        {
            Type = "DatabaseServer";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
