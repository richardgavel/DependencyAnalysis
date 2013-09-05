
namespace Analyzer.Model.Nodes
{
    public class UserDefinedFunction
    {
        public UserDefinedFunction()
        {
            Type = "UserDefinedFunction";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
