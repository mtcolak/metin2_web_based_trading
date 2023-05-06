using Metin2_v2.BaseDb;
using Metin2_v2.Data;
using Metin2_v2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Metin2_v2.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlayerController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Player");
            this._httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");

            if (userIdClaim == null)
            {
                return View("Error");
            }

            int userId = int.Parse(userIdClaim.Value);
            List<PlayerModel> characters = GetAccountInformationById(userId);

            long totalSum = 0;
            foreach (PlayerModel character in characters)
            {
                totalSum += character.offline_shop_cheque;
            }

            ViewBag.TotalSum = totalSum;

            return View(characters);
        }


        [Authorize]
        public IActionResult ListAccount()
        {
            var playerModels = DbBase.Get<PlayerModel>("spr_ListAllCharacters");

            return View(playerModels);
        }

        public IActionResult ListItems()
        {
            List<ItemModel> items = GetItemsFromDatabase();

            return View(items);
        }


        [HttpPost]
        public IActionResult BuyItemOnShop(int id)
        {
            string resultMessage = "";
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);

                    using var connection = new MySqlConnection(_connectionString);
                    connection.Open();

                    using var command = new MySqlCommand("spr_buyItemFromShop", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@input_item_id", id);
                    command.Parameters.AddWithValue("@input_account_id", userId);
                    int rowsAffected = Convert.ToInt32(command.ExecuteScalar());

                    if (rowsAffected > 0)
                    {
                        resultMessage = $"Update successfully. {rowsAffected} rows added.";
                    }
                    else
                    {
                        resultMessage = "Update failed.";
                    }
                }
                else { resultMessage = "There is no user found ."; }
            }
            catch (Exception ex)
            {
                resultMessage = $"Error during update : {ex.Message}";
            }

            // Eğer güncelleme başarılıysa, başarı mesajını ViewData ile gönderin
            ViewData["ResultMessage"] = resultMessage;
            return RedirectToAction("Index", "Home");
        }


        public List<T> ListItemsQuery<T>(string storedProcedure) where T : new()
        {
            List<T> items = new List<T>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(storedProcedure, connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                T item = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.WriteLine($"Column name: {reader.GetName(i)}, Value: {reader[i]}");
                    PropertyInfo? property = typeof(T).GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && !DBNull.Value.Equals(reader[i]))
                    {
                        property.SetValue(item, reader[i]);
                    }
                }

                items.Add(item);
            }

            return items;
        }

        public List<T> GetItems<T>(string storedProcedure, Dictionary<string, Action<T, object>> propertySetters) where T : new()
        {
            List<T> items = new List<T>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(storedProcedure, connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                T item = new T();
                foreach (var propertySetter in propertySetters)
                {
                    if (!DBNull.Value.Equals(reader[propertySetter.Key]))
                    {
                        propertySetter.Value(item, reader[propertySetter.Key]);
                    }
                }
                items.Add(item);
            }

            return items;
        }

        public List<ItemModel> GetItemsFromDatabase()
        {
            var propertySetters = new Dictionary<string, Action<ItemModel, object>>
    {

        { "id", (item, value) => item.id = Convert.ToInt32(value) },
        { "owner_id", (item, value) => item.owner_id = Convert.ToInt32(value) },
        { "name", (item, value) => item.name = Convert.ToString(value) },
        { "vnum", (item, value) => item.vnum = Convert.ToUInt16(value) },
        //{ "locale_name", (item, value) => item.locale_name = value != null ? Encoding.GetEncoding("Windows-1254").GetString(value as byte[]) : null },
                { "locale_name", (item, value) => {
            if (value != null)
            {
                byte[] localeNameBytes = value as byte[];
                Console.WriteLine("Raw bytes: " + BitConverter.ToString(localeNameBytes));
                item.locale_name = Encoding.GetEncoding("Windows-1254").GetString(localeNameBytes);
            }
            else
            {
                item.locale_name = null;
            }
        }},
        { "limitvalue0", (item, value) => item.limitvalue0 = Convert.ToInt32(value) },
        { "price_cheque", (item, value) => item.price_cheque = Convert.ToInt32(value) },
    };

            return GetItems<ItemModel>("spr_ListItemsOnShop", propertySetters);
        }

        public List<PlayerModel> GetAccountInformationById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand("spr_ListAllCharsInAccount", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@i_account_id", id);

            using var reader = command.ExecuteReader();

            List<PlayerModel> accounts = new List<PlayerModel>();

            while (reader.Read())
            {
                PlayerModel account = new PlayerModel
                {
                    id = reader.GetInt32("id"),
                    name = reader.GetString("name"),
                    job = reader.GetInt32("job"),
                    level = reader.GetInt32("level"),
                    playtime = reader.GetInt32("playtime"),
                    alignment = reader.GetInt32("alignment"),
                    offline_shop_cheque = reader.GetInt32("offline_shop_cheque"),
                };
                accounts.Add(account);
            }

            return accounts;
        }

    }
}
