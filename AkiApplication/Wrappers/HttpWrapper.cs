using System.Text;

namespace AkiApplication.Data.Models
{
    public class HttpWrapper
    {
        IHttpClientFactory ClientFactory;
        public HttpWrapper(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        public async Task<string> GetAsync(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(string uri, MultipartFormDataContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = ClientFactory.CreateClient();
            request.Content = content;
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<byte[]> GetByteArrayAsync(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task PostAsync(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var client = ClientFactory.CreateClient();
            {
                await client.SendAsync(request);
            }
        }

        public async Task PostAsync(string uri, MultipartFormDataContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var client = ClientFactory.CreateClient();
            {
                request.Content = content;
                await client.SendAsync(request);
            }
        }
    }
}
