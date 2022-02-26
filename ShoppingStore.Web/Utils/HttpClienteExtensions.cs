using System.Net.Http.Headers;
using System.Text.Json;

namespace ShoppingStore.Web.Utils
{
    public static class HttpClienteExtensions
    {
        private static MediaTypeHeaderValue _contentType = new MediaTypeHeaderValue("application/json");

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static async Task<HttpResponseMessage> PostAsJson<T>(this HttpClient http, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = _contentType;

            return await http.PostAsync(url, content);
        }

        public static async Task<HttpResponseMessage> PutAsJson<T>(this HttpClient http, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = _contentType;

            return await http.PutAsync(url, content);
        }
    }
}
