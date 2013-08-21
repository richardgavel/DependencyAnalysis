
namespace Analyzer.Model.Nodes
{
    public class StoredProcedure
    {
        public StoredProcedure()
        {
            Type = "StoredProcedure";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
