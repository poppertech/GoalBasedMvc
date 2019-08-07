using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;

namespace GoalBasedMvc.Logic
{
    public interface INodeService
    {
        INode GetNodeByUrl(string nodeUrl, string networkUrl);
    }

    public class NodeService: INodeService
    {
        private readonly INodeRepository _nodeRepository;

        public NodeService(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        public INode GetNodeByUrl(string nodeUrl, string networkUrl)
        {
            return _nodeRepository.GetNodeByUrl(nodeUrl, networkUrl);
        }
    }
}
