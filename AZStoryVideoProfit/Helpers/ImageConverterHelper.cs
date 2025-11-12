using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class ImageConverterHelper
    {
        public static bool ConvertBase64ToPng(string base64String, string outputPath)
        {
            try
            {
              
                byte[] imageBytes = Convert.FromBase64String(base64String);

                
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {

                    using (Image image = Image.FromStream(ms))
                    {

                        image.Save(outputPath, ImageFormat.Png);
                        return true;
                    }
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"{ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return false;
            }
        }
    }
}
