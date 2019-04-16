using System;
using System.Linq;

namespace ImageHunt.Helpers
{
    public static class EntityHelper
    {
        private static Random _random = new Random();

        public static string CreateCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code;
            code = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            return code;
        }
    }
}