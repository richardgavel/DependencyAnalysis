
namespace Analyzer.Model.Nodes
{
    public class Assembly
    {
        public Assembly()
        {
            Type = "Assembly";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
