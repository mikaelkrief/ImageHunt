using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FakeItEasy;

namespace TestUtilities
{
  public class WebServiceBaseTest : BaseTest
  {
    protected HttpMessageHandler _fakeHttpMessageHandler;
    protected HttpClient _httpClient;

    public WebServiceBaseTest()
    {
      _fakeHttpMessageHandler = A.Fake<HttpMessageHandler>();
      _httpClient = new HttpClient(_fakeHttpMessageHandler) { BaseAddress = new Uri("http://test.com") };
    }

    protected void FakeResponse(string resourceName)
    {
      var httpResponse = new HttpResponseMessage
      {
        Content = new StringContent(GetStringFromResource(Assembly.GetCallingAssembly(), 
          resourceName
        ))
      };
      A.CallTo(_fakeHttpMessageHandler)
        .Where(x => x.Method.Name == "SendAsync")
        .WithReturnType<Task<HttpResponseMessage>>()
        .Returns(httpResponse);
    }
  }
}