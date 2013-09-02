namespace Analyzer.Model.Nodes
{
    public class BizTalkServer
    {
        public BizTalkServer()
        {
            Type = "BizTalkServer";
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
