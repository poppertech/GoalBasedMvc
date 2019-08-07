using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;

namespace GoalBasedMvc.Logic
{
    public interface INodeService
    {
        INode GetNodeByUrl(string url);
    }

    public class NodeService: INodeService
    {
        private readonly INodeRepository _nodeRepository;

        public NodeService(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        public INode GetNodeByUrl(string url)
        {
            return _nodeRepository.GetNodeByUrl(url);
        }
    }
}
