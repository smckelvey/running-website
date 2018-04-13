using System;
using System.Linq;

namespace Website.Common.Extensions
{
    public static class StringExtensions
    {
        public static int ToMinutes(this string timestamp)
        {
            var timeSegments = timestamp.Trim().Split(Convert.ToChar(":"));

            foreach (string s in timeSegments)
            {
                if (!Int32.TryParse(s, out int _))
                {
                    return 0;
                }
            }

            if (timeSegments.Length == 3)
            {
                return (Convert.ToInt32(timeSegments[0]) * 60) + Convert.ToInt32(timeSegments[1]) + (Convert.ToInt32(timeSegments[2]) / 60);
            }

            if (timeSegments.Length == 2)
            {
                return Convert.ToInt32(timeSegments[0]) + (Convert.ToInt32(timeSegments[1]) / 60);
            }

            return 0;
        }
    }
}