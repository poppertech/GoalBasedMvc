using GoalBasedMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace GoalBasedMvc.Controllers
{
    public class NodeController : Controller
    {
        public IActionResult Get(int networkId)
        {
            var tree = GetTree();
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            using (var json = new JsonTextWriter(streamWriter))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(json, tree);
                streamWriter.Flush();
                var bytes = stream.ToArray();
                var outputStream = new MemoryStream(bytes);
                return File(outputStream, "application/octet-stream", "nodes.json");
            }
        }

        public NodeEditViewModel GetTree()
        {
            var distribution = new DistributionViewModel
            {
                Minimum = 50,
                Worst = 85,
                Likely = 105,
                Best = 125,
                Maximum = 150,
                HeightWorst = .57,
                HeightLikely = 3.31,
                HeightBest = .8,
                Mean = 1,
                Stdev = 2,
                Skew = 3,
                Kurt = 4
            };
            var parentNode = new NodeEditViewModel
            {
                Id = 1,
                Name = "Parent",
                IsPortfolioComponent = false,
                Distributions = new[] { distribution }
            };
            var childNode = new NodeEditViewModel
            {
                Id = 2,
                Name = "Child",
                InitialInvestment = 200000,
                InitialPrice = 100,
                IsPortfolioComponent = true,
                Distributions = new[] { distribution, distribution, distribution, distribution }
            };
            parentNode.Children.Add(childNode);
            return parentNode;
        }

    }
}