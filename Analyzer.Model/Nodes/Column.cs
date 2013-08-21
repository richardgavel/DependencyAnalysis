
namespace Analyzer.Model.Nodes
{
    public class Column
    {
        public Column()
        {
            Type = "Column";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
