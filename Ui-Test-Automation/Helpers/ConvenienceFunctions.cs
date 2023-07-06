using System;
using System.Linq;

namespace UiTest.Helpers
{
    public static class ConvenienceFunctions
    {
        private static Random random = new Random();
        public static string RandomStringGenerator(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
