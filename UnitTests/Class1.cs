using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class FirstTest : UnitTestBase
    {
        [Test]
        public void It_should()
        {
            context.Instruments.First().Id.ToString().ToUpper().ShouldEqual("1D37D829-AA79-4A26-911A-6AC68743F233");
        }

        [Test]
        public void It_should2()
        {
            context.Instruments.Count().ShouldEqual(2);
        }
    }

    [TestFixture]
    public class GuidHttpClientTest
    {
        [Test]
        public void Throws_exception_if_response_not_OK()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var httpClient = new HttpClient(new FakeHandler
            {
                Response = response,
                InnerHandler = new HttpClientHandler()
            });

            var client = new GuidHttpClient(httpClient);
            Assert.Throws<Exception>(() => client.Execute());
        }

        [Test]
        public void Returns_content_if_response_is_OK()
        {
            string content = Guid.NewGuid().ToString();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);

            var httpClient = new HttpClient(new FakeHandler
            {
                Response = response,
                InnerHandler = new HttpClientHandler()
            });

            var client = new GuidHttpClient(httpClient);
            string result = client.Execute();
            Assert.AreEqual(content, result);
        }
    }

    public class FakeHandler : DelegatingHandler
    {
        public HttpResponseMessage Response { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            if (Response == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            return Task.Factory.StartNew(() => Response);
        }
    }

    public class GuidHttpClient
    {
        private readonly HttpClient _client;

        public GuidHttpClient(HttpClient client)
        {
            _client = client;
        }

        public string Execute()
        {
            var request = new HttpRequestMessage { RequestUri = new Uri("http://localhost:1124/api/instruments") };
            Task<HttpResponseMessage> task = _client.SendAsync(request);
            HttpResponseMessage response = task.Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid response");
            }
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
