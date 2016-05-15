using System;
using System.Linq;

namespace SDEMViewModels.Messages
{
    public static class MessageHelper
    {
        private static Random _Random = new Random(new Random().Next());

        private const string _CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        static MessageHelper()
        {
            // trying to be random
            int iterations = new Random().Next(1000);
            Random localRandom = new Random();
            for (int i = 0; i < iterations; i++)
            {
                localRandom.Next();
            }
            _Random = new Random(localRandom.Next());
        }

        public static string GetExtraFluff(int length = 100)
        {
            return new string(Enumerable.Repeat(_CHARS, length)
              .Select(s => s[_Random.Next(s.Length)]).ToArray());
        }
    }
}
