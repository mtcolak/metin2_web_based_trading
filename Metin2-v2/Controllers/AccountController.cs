using Metin2_v2.Controllers;
using Metin2_v2.Data;
using Metin2_v2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Metin2_v2.Helper;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;

namespace Metin2_v2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _connectionStringPlayer;

        public AccountController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Account");
            _connectionStringPlayer = _configuration.GetConnectionString("Player");
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["HideNavbar"] = true;
            return View();
        }

        public string MySqlPasswordHash(string password)
        {
            using var sha1 = new SHA1Managed();

            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] doubleHash = sha1.ComputeHash(hash);
            
            StringBuilder result = new StringBuilder(41);
            result.Append('*');
            foreach (byte b in doubleHash)
            {
                result.AppendFormat("{0:X2}", b);
            }

            return result.ToString();
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = MySqlPasswordHash(model.Password);

                var account = IsAccountTrue(model.Username, hashedPassword);
                
                //List<AccountId> items = GetItemsFromDatabase();
                //AccountId account = GetItemFromDatabase();

                if (account != null && account.id > 0)
                {
                    //List<PlayerModel> characters = GetAccountInformationById(account.id);

                    var claims = new List<Claim>
            {
                new Claim("id", account.id.ToString()) 
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),

                        IsPersistent = false,

                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or password is wrong.");
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public AccountId IsAccountTrue(string username, string password)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand("spr_GetAccountInformation", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using var reader = command.ExecuteReader();

            AccountId res = new AccountId();

            if (reader.Read())
            {
                res.id = reader.GetInt32("id");
            }

            return res;
        }

        public async void SignIn(HttpContext httpContext, ActiveAccount user, bool isPersistent = false)
        {
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }


        #region LogOff

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            //var authCookie = Request.Cookies["iyinsan_RememberMe"];

            //if (authCookie != null)
            //{
            //    authCookie.Expires = DateTime.Now.AddDays(-1d);
            //    Response.Cookies.Add(authCookie);
            //}

            SessionHelper.Clear();
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        #endregion

    }
}
