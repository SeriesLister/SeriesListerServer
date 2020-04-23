using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Helpers
{
    public class Utils
    {

        public static string GetSafeBase64ImageString(string base64)
        {
            string[] split = base64.Split("base64,");
            return split.Length == 1 ? split[0] : split[1];
        }

    }
}
