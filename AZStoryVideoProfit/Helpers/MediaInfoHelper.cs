using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class MediaInfoHelper
    {
        public static int GetDuration(string filePath)
        {
            var tfile = TagLib.File.Create(filePath);
            string title = tfile.Tag.Title;
            TimeSpan duration = tfile.Properties.Duration;

            int retDuraion = (int)duration.TotalSeconds;
            if (retDuraion == 0)
            {
                duration = TimeSpan.Parse(tfile.Tag.Length);
                retDuraion = (int)duration.TotalSeconds;
            }

            return retDuraion;

        }
    }
}
