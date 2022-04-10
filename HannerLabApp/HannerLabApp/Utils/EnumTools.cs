using System;
using System.Collections.Generic;
using System.Linq;
using HannerLabApp.Extensions;

namespace HannerLabApp.Utils
{
    public static class EnumTools
    {
        public static List<string> ConvertEnumToListOfStringDescriptions(Type e)
        {
            var list = new List<string>();

            try
            {
                var values = Enum.GetValues(e).Cast<Enum>();

                foreach (var v in values)
                {
                    var sz = v.GetDescription().ToString();

                    if (!string.IsNullOrEmpty(sz))
                    {
                        list.Add(sz);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Non enum provided to method ConvertEnumToListOfStringDescriptions", ex);
                return new List<string>();
            }

            return list;
        }

    }
}
