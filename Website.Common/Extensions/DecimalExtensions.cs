using System;
using System.Globalization;

namespace Website.Common.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal ToMiles(this decimal distance, string units)
        {
            if (units.Equals("mi"))
            {
                return distance;
            }
            
            if (units.Equals("km"))
            {
                return distance * 1.61M;
            }

            return 0.0M;
        }

        public static string ToDuration(this decimal minutes)
        {
            if (minutes <= 0)
            {
                return "0";
            }

            var speedParts = Math.Round(minutes, 2).ToString(CultureInfo.InvariantCulture).Split('.');
            var duration = "";
            //reduce by 60 repeatedly
            var wholeMinutes = Convert.ToInt32(speedParts[0]);
            if (wholeMinutes >= 60)
            {
                duration = "0";
                while (wholeMinutes >= 60)
                {
                    duration = (Convert.ToInt32(duration) + 1).ToString();
                    wholeMinutes -= 60;
                }
                duration += ":";
            }
       
            return duration + wholeMinutes.ToString("D2") + ":" + Convert.ToInt32(Math.Round((speedParts.Length > 1 ? Convert.ToInt32(speedParts[1]) : 0) * 0.6, 0)).ToString("D2");
        }

        public static string ToFriendlyDistance(this decimal distance)
        {
            if (distance > 27)
            {
                return "Ultra-Marathon";
            }    
            else if (distance > 26)
            {
                return "Marathon";
            } 
            else if (distance > 13 && distance < 13.9M)
            {
                return "Half-Marathon";
            }
            else if (distance > 6 && distance < 6.5M)
            {
                return "10k";
            }
            else if (distance > 3 && distance < 3.5M)
            {
                return "5k";
            }
                   
            return distance + "mi";
        }
    }
}