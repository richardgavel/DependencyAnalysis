
namespace Analyzer.Model.Nodes
{
    public class Field
    {
        public Field()
        {
            Type = "Field";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
