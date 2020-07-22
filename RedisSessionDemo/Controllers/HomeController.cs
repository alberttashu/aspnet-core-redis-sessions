using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RedisSessionDemo.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RedisSessionDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;

        public HomeController(
            ILogger<HomeController> logger,
            IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var dateString = await _cache.GetStringAsync("time", cancellationToken);
            if (string.IsNullOrEmpty(dateString))
            {
                dateString = DateTime.Now.ToLongTimeString();
                _logger.LogInformation("Writing date time to cache");
                await _cache.SetStringAsync("time", dateString, new DistributedCacheEntryOptions() 
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                }, cancellationToken);
            }

            return View((object)dateString);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
