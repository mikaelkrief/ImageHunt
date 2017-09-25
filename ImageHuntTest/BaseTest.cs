using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ImageHuntTest
{
    public class BaseTest
    {
        protected byte[] GetImageFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, (int)stream.Length);
            return array;
        }

    }
}
