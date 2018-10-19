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

        private IList<double> GetCashFlows()
        {
            return new double[] {
                60000, 61000, 62000, 63000, 64000, 65000
            };
        }
    }
}