using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Helpers
{
    public static class DecryptionManager
    {
        public static int Progress;

        public static string Decrypt(Bitmap imageWithMessage, int rSize, int gSize, int bSize)
        {
            Bitmap pictureToDecryption = imageWithMessage;

            StringBuilder binaryStringFromImage = new StringBuilder();
            for (int x = 0; x < pictureToDecryption.Width; x++)
            {
                for (int y = 0; y < pictureToDecryption.Height; y++)
                {
                    Color deColor = pictureToDecryption.GetPixel(x, y);
                    binaryStringFromImage.Append(DecriptionPixel(deColor, rSize, gSize, bSize));
                }
                Progress++;
            }

            string message = DataManager.ConvertBinaryStringToMessage(binaryStringFromImage.ToString());

            Progress += 10;

            return message;
        }

        private static string DecriptionPixel(Color deColor, int rSize, int gSize, int bSize)
        {
            StringBuilder text = new StringBuilder();

            var Red = Convert.ToString(deColor.R, 2).PadLeft(8, '0');
            var Green = Convert.ToString(deColor.G, 2).PadLeft(8, '0');
            var Blue = Convert.ToString(deColor.B, 2).PadLeft(8, '0');

            for (int i = Red.Length - rSize; i < Red.Length; i++)
                text.Append(Red[i]);

            for (int i = Green.Length - gSize; i < Green.Length; i++)
                text.Append(Green[i]);

            for (int i = Blue.Length - bSize; i < Blue.Length; i++)
                text.Append(Blue[i]);

            return text.ToString();
        }
    }
}
