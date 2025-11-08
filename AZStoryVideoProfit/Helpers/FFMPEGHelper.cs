using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class FFMPEGHelper
    {

        // Hàm Log Information giả định
        private static void LogInfo(string message)
        {
            Console.WriteLine(string.Format("[INFO] {0}", message));
        }

        // Hàm Log Error giả định
        private static void LogError(string message)
        {
            Console.Error.WriteLine(string.Format("[ERROR] {0}", message));
        }

       
    }
}
