using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tests
{
    public class WebApiClient : IWebApiClient
    {
        protected readonly string _endpoint;
        private HttpServer _inMemoryServer;
        public HttpStatusCode LastStatusCode { get; set; }

        public WebApiClient(string endpoint)
        {
            _endpoint = endpoint;
        }

        public WebApiClient AgainstInMemoryServer(HttpServer server)
        {
            _inMemoryServer = server;
            return this;
        }

        public T Get<T>(int top = 0, int skip = 0)
        {
            using (var httpClient = NewHttpClient())
            {
                var endpoint = _endpoint + "?";
                var parameters = new List<string>();

                if (top > 0)
                    parameters.Add(string.Concat("$top=", top));

                if (skip > 0)
                    parameters.Add(string.Concat("$skip=", skip));

                endpoint += string.Join("&", parameters);

                var response = httpClient.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode();
                LastStatusCode = response.StatusCode;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public T Get<T>(string id)
        {
            using (var httpClient = NewHttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.GetAsync(_endpoint + id).Result;

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public HttpResponseMessage Post<T>(T data)
        {
            using (var httpClient = NewHttpClient())
            {
                var requestMessage = GetHttpRequestMessage<T>(data);

                return httpClient.PostAsync(_endpoint, requestMessage.Content).Result;
            }
        }

        public string Put<T>(int id, T data)
        {
            using (var httpClient = NewHttpClient())
            {
                var requestMessage = GetHttpRequestMessage<T>(data);

                var result = httpClient.PutAsync(_endpoint + id, requestMessage.Content).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public string Delete(int id)
        {
            using (var httpClient = NewHttpClient())
            {
                var result = httpClient.DeleteAsync(_endpoint + id).Result;

                return result.Content.ToString();
            }
        }

        protected HttpRequestMessage GetHttpRequestMessage<T>(T data)
        {
            var mediaType = new MediaTypeHeaderValue("application/json");
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());
            var jsonFormatter = new JsonNetFormatter(jsonSerializerSettings);

            var requestMessage = new HttpRequestMessage<T>(data, mediaType, new MediaTypeFormatter[] { jsonFormatter });

            return requestMessage;
        }

        protected HttpClient NewHttpClient()
        {
            if (_inMemoryServer != null)
                return new HttpClient(_inMemoryServer);
            return new HttpClient();
        }
    }
}