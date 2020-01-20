using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using testEFCoreCacheable.Models;
using Z.EntityFramework.Plus;

namespace testEFCoreCacheable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        ///  https://github.com/zzzprojects/EntityFramework-Plus
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var users = _dbContext.User.Take(2)
                                  .FromCache(new MemoryCacheEntryOptions
                                  {
                                      AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3)
                                  })
                                  .ToList();

            Console.WriteLine($"--- {DateTime.Now} ----");
            Console.WriteLine(JsonSerializer.Serialize(users));

            var delete = _dbContext.User
                                   .Where(a => a.Id == users[0].Id)
                                   .Delete();

            Console.WriteLine($"--- delete :: {delete} ---");

            return View();
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