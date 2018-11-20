using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Helpers
{
    public static class FileManager
    {
        public static void SaveImageWithMessageToBmp(Bitmap image, string filePath = "")
        {
            if (filePath.Equals(String.Empty))
            {
                filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                           + @"\image" + DateTime.Now.Ticks + ".bmp";
            }

            image.Save(filePath, ImageFormat.Bmp);
        }

        public static void SaveMessageToTxt(string message, string filePath = "")
        {
            if (filePath.Equals(String.Empty))
            {
                filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                           + @"message" + DateTime.Now.Ticks + ".txt";
            }

            System.IO.File.WriteAllText(filePath, message);
        }

        public static void SaveResultOfTestToTxt(string resultInfo, string filePath)
        {
            System.IO.File.AppendAllText(filePath, resultInfo);
        }

        public static string ReadAllTextFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
