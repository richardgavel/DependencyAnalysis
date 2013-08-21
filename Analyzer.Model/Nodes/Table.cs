
namespace Analyzer.Model.Nodes
{
    public class Table
    {
        public Table()
        {
            Type = "Table";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
