using System.IO;
using System.Reflection;
using Autofac;
using Newtonsoft.Json.Linq;

namespace TestUtilities
{
    public class BaseTest
    {
      protected ContainerBuilder TestContainerBuilder;
      protected IContainer Container;
      public BaseTest()
      {
        TestContainerBuilder = new ContainerBuilder();
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

    public class BaseTest<T> : BaseTest where T : class
    {
        protected T Target;
        public BaseTest() : base()
        {
            TestContainerBuilder.RegisterType<T>();
        }

        public void Build()
        {
            Container = TestContainerBuilder.Build();
            Target = Container.Resolve<T>();
        }
    }
}
