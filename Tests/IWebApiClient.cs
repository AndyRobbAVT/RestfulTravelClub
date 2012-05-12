using System.Net;
using System.Net.Http;

namespace Tests
{
    public interface IWebApiClient
    {
        HttpStatusCode LastStatusCode { get; set; }
        T Get<T>(int top = 0, int skip = 0);
        T Get<T>(string id);
        HttpResponseMessage Post<T>(T data);
        string Put<T>(int id, T data);
        string Delete(int id);
    }
}