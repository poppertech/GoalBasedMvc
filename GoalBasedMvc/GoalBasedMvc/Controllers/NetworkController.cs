using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GoalBasedMvc.Models;
using GoalBasedMvc.Logic;
using System.Linq;

namespace GoalBasedMvc.Controllers
{
    public class NetworkController : Controller
    {
        private readonly INetworkService _networkService;
        private readonly INodeService _nodeService;

        public NetworkController(
            INetworkService networkService,
            INodeService nodeService
            )
        {
            _networkService = networkService;
            _nodeService = nodeService;
        }

        public IActionResult Index()
        {
            var networks = _networkService.GetNetworks();
            return View(networks);
        }

        [HttpGet("{url}")]
        public IActionResult Get(string url)
        {
            var network = _networkService.GetNetworkByUrl(url);
            return View(network);
        }

        [HttpGet("{url}/nodes")]
        public IActionResult Nodes(string url)
        {
            var network = _networkService.GetNetworkByUrl(url);
            return View("Nodes", network);
        }

        [HttpGet("{networkUrl}/nodes/{nodeUrl}")]
        public IActionResult Node(string networkUrl, string nodeUrl)
        {
            var nodeOnly = _nodeService.GetNodeByUrl(nodeUrl, networkUrl);
            var network = _networkService.GetNetworkByUrl(networkUrl);
            var node = network.Nodes[nodeOnly.Id];
            node.NetworkName = network.Name;
            node.NetworkUrl = network.Url;
            return View("Node", node);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


}
}
