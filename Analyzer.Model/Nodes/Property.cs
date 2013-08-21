
namespace Analyzer.Model.Nodes
{
    public class Property
    {
        public Property()
        {
            Type = "Property";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
