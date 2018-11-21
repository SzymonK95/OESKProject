using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WPFApp.Helpers
{
    public class TestsManager
    {
        private static string resultFilePath = @"E:\Projects\VS2017\OESKProject\WPFApp\Files\Output\result";
        private static int fileInCount = 3;
        public static bool areTestsActive = false;

        public static void Test(int rSize, int gSize, int bSize)
        {
            long namePart = DateTime.Now.Ticks;

            if (areParamOk(rSize, gSize, bSize))
            {
                MessageBox.Show("The tests will start in a moment", "Tests");

                try
                {
                    for (int times = 0; times < 20; times++)
                    {

                        string[] resultInfo = new string[fileInCount];

                        for (int i = 0; i < fileInCount; i++)
                        {
                            string messages = DataManager.ConvertTextToBinarySystem(
                                FileManager.ReadAllTextFromFile(
                                    @"E:\Projects\VS2017\OESKProject\WPFApp\Files\Input\Texts\TextIn" + (i + 1) +
                                    ".txt"));

                            Bitmap baseImages =
                                new Bitmap(@"E:\Projects\VS2017\OESKProject\WPFApp\Files\Input\Images\PhotoIn" +
                                           (i + 1) +
                                           ".bmp");

                            areTestsActive = true;

                            resultInfo[i] = Job_Encrypt(messages, baseImages, rSize, gSize, bSize);
                        }

                        areTestsActive = false;

                        foreach (var result in resultInfo)
                        {
                            FileManager.SaveResultOfTestToTxt(result + Environment.NewLine,
                                resultFilePath + namePart + "_" + rSize + gSize + bSize + ".txt");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("[TestsManager] " + ex.Message, "Tests Manager Error");
                }

                MessageBox.Show("Tests have been successfully completed", "Tests - Succes");
            }
            else
            {
                MessageBox.Show("Set parameters for tests");
            }
        }

        private static string Job_Encrypt(string message, Bitmap baseImg, int rSize, int gSize, int bSize)
        {
            string messageBin = DataManager.ConvertTextToBinarySystem(message);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Bitmap result = EncryptionManager.Encrypt(messageBin, baseImg, rSize, gSize, bSize);
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            long messageBinLength = messageBin.Length;
            long baseImgTotalPixCount = baseImg.Height * baseImg.Width;
            long spaceForMessageInBaseImg = baseImgTotalPixCount * (rSize + gSize + bSize);
            long timeOfEcryption = (long)ts.TotalMilliseconds;
            long howManyTimesMessageCanBeHidedInImage = (spaceForMessageInBaseImg * 100) / messageBinLength;
            
            string resultFormat =
                $"{messageBinLength};{baseImgTotalPixCount};{spaceForMessageInBaseImg};{howManyTimesMessageCanBeHidedInImage};{timeOfEcryption}";

            FileManager.SaveImageWithMessageToBmp(result, resultFilePath + DateTime.Now.Ticks + ".bmp");

            return resultFormat;
        }

        private static bool areParamOk(int rSize, int gSize, int bSize)
        {
            return !(rSize == 0 && gSize == 0 && bSize == 0);
        }
    }
}
