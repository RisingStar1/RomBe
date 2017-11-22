using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public static class RedisCacherHelper
    {
        private static ConnectionMultiplexer connectionMultiplexer;
        private static IDatabase database;

        public static void Init()
        {
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                    {
                        { "rombe.redis.cache.windows.net", 6380 }
                    },
                KeepAlive = 180,
                Password = "cwCLPU2YAPimyG6dC54am7iZKE/QRrcjR/92d8wLwLk=",
                AbortOnConnectFail = false,
                Ssl = true,
                SyncTimeout = 1000 * 20
            };

            connectionMultiplexer = ConnectionMultiplexer.Connect(config);
            database = connectionMultiplexer.GetDatabase();
        }

        private static bool StoreData(string key, string value)
        {
            return database.StringSet(key, value);
        }

        private static string GetData(string key)
        {
            return database.StringGet(key);
        }

        private static void DeleteData(string key)
        {
            database.KeyDelete(key);
        }

        public static bool Remove(string key)
        {
            return database.KeyDelete(key);
        }

        public static bool Exists(string key)
        {
            try
            {
                return database.KeyExists(key);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool Add<T>(string key, T value, DateTimeOffset expiresAt) where T : class
        {
            var serializedObject = JsonConvert.SerializeObject(value);
            var expiration = expiresAt.Subtract(DateTimeOffset.Now);
            try
            {
                return database.StringSet(key, serializedObject, expiration);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static T Get<T>(string key) where T : class
        {
            if (Exists(key))
            {
                var serializedObject = database.StringGet(key);

                return JsonConvert.DeserializeObject<T>(serializedObject);
            }
            return null;
        }
    }
}
