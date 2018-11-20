using System;
using System.Drawing;

namespace WPFApp.Helpers
{
    public static class EncryptionManager
    {
        public static int Progress;

        public static Bitmap Encrypt(string message, Bitmap baseImage, int rSize, int gSize, int bSize)
        {
            Bitmap picture = baseImage;
            int messagePosition = 0;
            Progress = 0;

            for (int x = 0; x < picture.Width; x++)
            {
                for (int y = 0; y < picture.Height; y++)
                {
                    if (messagePosition < message.Length)
                    {
                        string partOfmessage = DataManager.GetPartOfMessageByPosition(
                            message, 
                            messagePosition,
                            rSize+gSize+bSize
                            );

                        picture.SetPixel(
                            x,
                            y,
                            NewColorUsingMessage
                            (
                                picture.GetPixel(x, y),
                                partOfmessage,
                                rSize,
                                gSize,
                                bSize
                            )
                        );

                        messagePosition += partOfmessage.Length;
                    }
                    else
                    {
                        break;
                    }
                }

                Progress++;
            }

            Progress += 10;

            return picture;
        }

        private static Color NewColorUsingMessage(Color pixel, string message, int rSize, int gSize, int bSize)
        {
            int iiMessage = 0;

            string convertRed = DataManager.ConvertColorValueToString(pixel.R);
            string convertGreen = DataManager.ConvertColorValueToString(pixel.G);
            string convertBlue = DataManager.ConvertColorValueToString(pixel.B);

            char[] baseCRed = new char[convertRed.Length];
            char[] baseCGreen = new char[convertGreen.Length];
            char[] baseCBlue = new char[convertBlue.Length];

            for (int i = 0; i < baseCRed.Length; i++)
            {
                baseCRed[i] = convertRed[i];
                baseCGreen[i] = convertGreen[i];
                baseCBlue[i] = convertBlue[i];
            }

            for (int j = baseCRed.Length - rSize; j < baseCRed.Length; j++)
            {
                if (iiMessage < message.Length)
                {
                    baseCRed[j] = message[iiMessage];
                    iiMessage++;
                }
            }

            for (int j = baseCGreen.Length - gSize; j < baseCGreen.Length; j++)
            {
                if (iiMessage < message.Length)
                {
                    baseCGreen[j] = message[iiMessage];
                    iiMessage++;
                }
            }

            for (int j = baseCBlue.Length - bSize; j < baseCBlue.Length; j++)
            {
                if (iiMessage < message.Length)
                {
                    baseCBlue[j] = message[iiMessage];
                    iiMessage++;
                }
            }

            int newRed = Convert.ToInt32(new string(baseCRed), 2);
            int newGreen = Convert.ToInt32(new string(baseCGreen), 2);
            int newBlue = Convert.ToInt32(new string(baseCBlue), 2);

            return Color.FromArgb(pixel.A, newRed, newGreen, newBlue);
        }
    }
}
