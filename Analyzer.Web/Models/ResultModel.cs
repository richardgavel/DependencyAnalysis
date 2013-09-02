using System.Collections.Generic;

namespace Analyzer.Web.Models
{
    public class ResultModel
    {
        public NodeModel Target { get; set; }

        public List<NodeModel> Ancestry { get; set; }

        public List<NodeModel> Contains { get; set; }

        public List<NodeModel> References { get; set; }

        public List<NodeModel> ReferencedBy { get; set; }
    }
}