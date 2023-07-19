using System.Text;
using System.Text.Json;

namespace Consul.Demo.Helpers
{
    public static class ConsulKeyValueProvider
    {
        public static async Task<T?> GetValueAsync<T>(string key)
        {
            using (var client = new ConsulClient())
            {
                var getPair = await client.KV.Get(key);

                if (getPair?.Response == null)
                {
                    return default(T);
                }

                var value = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);

                return JsonSerializer.Deserialize<T>(value);
            }
        }

        public static async Task<bool> SetValueAsync<T>(this ConsulClient client, string key, T value)
        {
            try
            {
                var pair = new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(value.ToString())
                };

                var setResult = await client.KV.Put(pair);
                return setResult.Response;
            }
            catch (Exception ex)
            {
                // Handle any exceptions here or log them as needed.
                // For simplicity, we are just rethrowing the exception.
                throw;
            }
        }
    }
}
