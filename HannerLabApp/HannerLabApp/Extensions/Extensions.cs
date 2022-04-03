using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HannerLabApp.Extensions
{
    public static class Extensions
    {
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
