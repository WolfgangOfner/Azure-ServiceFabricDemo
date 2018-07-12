using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using StatefulDemo.WebApp.Models;

namespace StatefulDemo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FabricClient _fabricClient;

        public HomeController(FabricClient client)
        {
            _fabricClient = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Message"] = "Your home page.";

            var model = new Dictionary<Guid, long>();
            var serviceUrl = new Uri("fabric:/LeadGenerator/Simulator");

            foreach (var partition in await _fabricClient.QueryManager.GetPartitionListAsync(serviceUrl))
            {
                var partitionKey =
                    new ServicePartitionKey(((Int64RangePartitionInformation) partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<ISimulatorService>(serviceUrl, partitionKey);
                var leads = await proxy.GetLeads();

                model.Add(partition.PartitionInformation.Id, leads);
            }

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}