using System.IO;
using System.Reflection;
using Autofac;
using Newtonsoft.Json.Linq;

namespace TestUtilities
{
    public class BaseTest
    {
      protected ContainerBuilder _testContainerBuilder;
      protected IContainer _container;
      public BaseTest()
      {
        _testContainerBuilder = new ContainerBuilder();
      }
    protected byte[] GetImageFromResource(Assembly assembly, string resourceName)
        {
          using (var stream = assembly.GetManifestResourceStream(resourceName))
          {
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, (int) stream.Length);
            return array;
          }
        }

      protected string GetStringFromResource(Assembly assembly, string resourceName)
      {
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
          using (var reader = new StreamReader(stream))
          {
            return reader.ReadToEnd();
          }
        }

      }
      protected JObject GetJObjectFromResource(Assembly assembly, string resourceName)
      {
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
          using (var reader = new StreamReader(stream))
          {
            return JObject.Parse(reader.ReadToEnd());
          }
        }

      }
  }
}
