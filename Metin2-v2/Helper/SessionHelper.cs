using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Metin2_v2.Helper
{
    public class SessionHelper
    {
        private static IHttpContextAccessor httpContextAccessor;
        private static ISession _session => httpContextAccessor.HttpContext.Session;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public static void Clear()
        {
            _session.Clear();
        }

        public static HttpRequest GetCurrentHttpRequest()
        {
            return httpContextAccessor.HttpContext.Request;
        }
        public static ConnectionInfo GetCurrentHttpRequestConnection()
        {
            return httpContextAccessor.HttpContext?.Connection;
        }

        public static void Set<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                });
            var bytes = Encoding.UTF8.GetBytes(json);
            _session.Set(key, bytes);
        }

        public static bool Get<T>(string key, out T value)
        {
            byte[] bytes;
            if (!_session.TryGetValue(key, out bytes))
            {
                value = default(T);
                return false;
            }
            try
            {
                var json = Encoding.UTF8.GetString(bytes);
                value = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }
    }
}
