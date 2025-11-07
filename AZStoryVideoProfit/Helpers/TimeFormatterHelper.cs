using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class TimeFormatterHelper
    {
        /// <summary>
        /// Converts seconds (double or float) into MM:SS format.
        /// </summary>
        /// <param name="seconds">The total number of seconds (as a double).</param>
        /// <returns>A string representing the time in MM:SS format.</returns>
        public static string FormatTimestamp(double seconds)
        {            
            int totalSeconds = (int)seconds;            
            int minutes = totalSeconds / 60;            
            int remainingSeconds = totalSeconds % 60;            
            return $"{minutes:D2}:{remainingSeconds:D2}";
        }
    }
}
