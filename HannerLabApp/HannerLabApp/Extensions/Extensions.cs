using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Extensions
{
    public static class Extensions
    {
        public static async Task<string> ToBase64StringAsync(this FileResult fr)
        {
            using (var stream = await fr.OpenReadAsync())
            {
                return stream.ConvertToBase64String();
            }
        }

        public static async Task<byte[]> ToBytesAsync(this FileResult fr)
        {
            await Task.Delay(1);

            using (var stream = await fr.OpenReadAsync())
            {
                return stream.ConvertToBytes();
            }
        }

        public static string ConvertToBase64String(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static byte[] ConvertToBytes(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return bytes;
        }

        /// <summary>
        /// File.Delete() Wrapper around threading, not real asynchronous file i/o.
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static Task DeleteAsync(this FileInfo fi)
        {
            return Task.Factory.StartNew(() => fi.Delete());
        }

        public static string GetDescription(this Enum e)
        {
            var attribute =
                e.GetType()
                        .GetTypeInfo()
                        .GetMember(e.ToString())
                        .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                    as DescriptionAttribute;

            return attribute?.Description ?? e.ToString();
        }

        /// <summary>
        /// Creates a list of strings for the descriptions of an enum.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static IList<string> GetDescriptionList(this Enum e)
        {
            var values = Enum.GetValues(e.GetType()).Cast<Enum>();

            return values.Select(v => v.GetDescription().ToString()).ToList();
        }
    }
}
