using Metin2_v2.Data;
using Metin2_v2.Helper;
using Microsoft.AspNetCore.Mvc.Routing;
using MySqlConnector;
using System.Data;
using System.Reflection;

namespace Metin2_v2.BaseDb
{
    public class DbBase
    {
        private static string _connectionString;

        static DbBase()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("Player");
        }

        #region Get
        public static List<T> Get<T>(string storedProcedure) where T : new()
        {
            List<T> items = new List<T>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T item = new T();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                PropertyInfo property = typeof(T).GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                if (property != null && !DBNull.Value.Equals(reader[i]))
                                {
                                    property.SetValue(item, reader[i]);
                                }
                            }
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }
        #endregion


        public static int ExecuteScalar(string metodName, params object[] parameters)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(SqlHelper.ExecuteScalar(dbParameters.dsnStore, metodName, parameters));
            }
            catch (Exception ex)
            {
                result = -1;
                throw ex;
            }
            return result;
        }

    }
}

        //public static V Get<T, V>(T Id, string metodName)
        //{
        //    List<V> objectlist = Activator.CreateInstance<List<V>>();
        //    SqlHelper.FillList<V>(dbParameters.dsnStore, metodName, objectlist, new object[] { Id });

        //    return objectlist.FirstOrDefault();
        //}

        //public static List<V> Get<V>(string metodName, params object[] parameters)
        //{
        //    List<V> objectlist = Activator.CreateInstance<List<V>>();
        //    SqlHelper.FillList<V>(dbParameters.dsnStore, metodName, objectlist, parameters);

        //    return objectlist;
        //}