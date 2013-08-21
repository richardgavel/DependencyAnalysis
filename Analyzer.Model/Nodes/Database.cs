
namespace Analyzer.Model.Nodes
{
    public class Database
    {
        public Database()
        {
            Type = "Database";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
