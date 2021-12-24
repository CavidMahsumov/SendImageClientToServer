using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient.Extentations
{
    public class ImageHelper
    {
        public static void CreateIfMissing(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string GetImagePath(byte[] buffer, int counter)
        {
            Image x = (Bitmap)((new ImageConverter()).ConvertFrom(buffer));
            //ImageConverter ic = new ImageConverter();
            //Image img = (Image)ic.ConvertFrom(buffer);
            Bitmap bitmap1 = new Bitmap(x);
            bitmap1.Save($@"C:\Users\Mahs_kz07\source\repos\SendImageClientToServer\SendImageClientToServer\bin\Debug\image{counter}.jpg");
            var imagepath = $@"C:\Users\Mahs_kz07\source\repos\SendImageClientToServer\SendImageClientToServer\bin\Debug\image{counter}.jpg";
            return imagepath;
        }
        public static byte[] GetBytesOfImage(string path)
        {
            byte[] b = File.ReadAllBytes(path);
            return b;
        }
    }
}
