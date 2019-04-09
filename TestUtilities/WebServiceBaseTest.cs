using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FakeItEasy;

namespace TestUtilities
{
  public class WebServiceBaseTest : BaseTest
  {
    protected HttpMessageHandler FakeHttpMessageHandler;
    protected HttpClient HttpClient;

    public WebServiceBaseTest()
    {
      FakeHttpMessageHandler = A.Fake<HttpMessageHandler>();
      HttpClient = new HttpClient(FakeHttpMessageHandler) { BaseAddress = new Uri("http://test.com") };
    }

    protected void FakeResponse(string resourceName)
    {
      var httpResponse = new HttpResponseMessage
      {
        Content = new StringContent(GetStringFromResource(Assembly.GetCallingAssembly(), 
          resourceName
        ))
      };
      A.CallTo(FakeHttpMessageHandler)
        .Where(x => x.Method.Name == "SendAsync")
        .WithReturnType<Task<HttpResponseMessage>>()
        .Returns(httpResponse);
    }
  }
}