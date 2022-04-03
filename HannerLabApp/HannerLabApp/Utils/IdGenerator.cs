using System;
using System.Linq;

namespace HannerLabApp.Utils
{
    /// <summary>
    /// Generates random strings for userspecified Ids
    /// </summary>
    public static class IdGenerator
    {
        private const int Length = 6;
        private static readonly string EncodeChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly Random Random = new Random();
        
        /// <summary>
        /// Generates a random identifier string. Used for generating UserSpecifiedIds. Has absolutely no guarantee on security or uniqueness. 
        /// </summary>
        /// <returns>A random 6 digit string</returns>
        public static string GetNewRandomId()
        {
            return new string(Enumerable.Repeat(EncodeChars, Length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
