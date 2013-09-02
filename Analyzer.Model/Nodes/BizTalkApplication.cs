namespace Analyzer.Model.Nodes
{
    public class BizTalkApplication
    {
        public BizTalkApplication()
        {
            Type = "BizTalkApplication";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
