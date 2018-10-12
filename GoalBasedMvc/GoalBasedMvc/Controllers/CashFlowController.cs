using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using GoalBasedMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoalBasedMvc.Controllers
{
    public class CashFlowController : Controller
    {
        public IActionResult Get(int networkId)
        {
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            using (var csv = new CsvWriter(streamWriter))
            {
                var cashFlows = GetCashFlows();
                csv.WriteRecords(cashFlows);
                streamWriter.Flush();
                var bytes = stream.ToArray();
                var outputStream = new MemoryStream(bytes);
                return File(outputStream, "application/octet-stream", "cashflows.csv");
            }
        }

        private IList<CashFlowViewModel> GetCashFlows()
        {
            return new CashFlowViewModel[] {
                new CashFlowViewModel { Cost = 60000 },
                new CashFlowViewModel { Cost = 61000 },
                new CashFlowViewModel { Cost = 62000 },
                new CashFlowViewModel { Cost = 63000 },
                new CashFlowViewModel { Cost = 64000 },
                new CashFlowViewModel { Cost = 65000 },
            };
        }
    }
}