using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using TMWebAPI;

namespace Tests
{
    public class InMemoryTests
    {
        private HttpServer Server;
        private string UrlBase = "https://localhost:44366/";

        [OneTimeSetUp]
        public void Setup()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(name: "Default", routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Server = new HttpServer(config);
        }

        [Test]
        public void GetOrderStatus()
        {
            var client = new HttpClient(Server);
            var request = CreateRequest("api/values/Get", "application/json", HttpMethod.Get);

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(response.Content);
            }
        }
        //[Test]
        //public void GetOrderStatus()
        //{
        //    var client = new HttpClient(Server);
        //    var request = CreateRequest("api/Orders/Get?companyCode=001&orderNumber=1234", "application/json", HttpMethod.Get);

        //    using (HttpResponseMessage response = client.SendAsync(request).Result)
        //    {
        //        Assert.IsNotNull(response);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //        Assert.NotNull(response.Content);
        //    }
        //}
        private HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(UrlBase + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }

        private HttpRequestMessage createRequest<T>(string url, string mthv, HttpMethod method, T content, MediaTypeFormatter formatter) where T : class
        {
            HttpRequestMessage request = CreateRequest(url, mthv, method);
            request.Content = new ObjectContent<T>(content, formatter);

            return request;
        }

        public void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
        }
    }
}