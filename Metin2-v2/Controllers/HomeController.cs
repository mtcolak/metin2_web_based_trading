using Metin2_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySqlConnector;
using Metin2_v2.Data;
using Microsoft.EntityFrameworkCore;
using Metin2_v2.BaseDb;
using Microsoft.AspNetCore.Authorization;

namespace Metin2_v2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Account");
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            var playerModels = DbBase.Get<PlayerModel>("spr_ListAllCharacters");

            return View(playerModels);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}