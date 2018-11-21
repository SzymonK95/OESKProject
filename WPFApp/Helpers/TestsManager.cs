using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp.Helpers
{
    public class TestsManager
    {
        private static string resultFilePath = @"E:\Projects\VS2017\OESKProject\WPFApp\Files\Output\result";
        private static int fileInCount = 3;
        private static int timesOfTest = 20;
        public static bool areEncTestsActive = false;
        public static bool areDecTestsActive = false;

        public static void TestEnc(int rSize, int gSize, int bSize)
        {
            long namePart = DateTime.Now.Ticks;

            if (areParamOk(rSize, gSize, bSize))
            {
                MessageBox.Show("The tests will start in a moment", "Tests");

                try
                {
                    for (int times = 0; times < timesOfTest; times++)
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

                            areEncTestsActive = true;

                            resultInfo[i] = Job_Encrypt(messages, baseImages, rSize, gSize, bSize);
                        }

                        areEncTestsActive = false;

                        foreach (var result in resultInfo)
                        {
                            FileManager.SaveResultOfTestToTxt(result + Environment.NewLine,
                                resultFilePath + namePart + "_" + rSize + gSize + bSize + ".txt");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("[TestsManager:Enc] " + ex.Message, "Tests Manager Error");
                    return;
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

        public static void TestDec(int rSize, int gSize, int bSize)
        {
            long namePart = DateTime.Now.Ticks;

            if (areParamOk(rSize, gSize, bSize))
            {
                MessageBox.Show("The decryption tests will start in a moment", "Tests");

                try
                {
                    for (int times = 0; times < timesOfTest; times++)
                    {
                        string[] resultInfo = new string[fileInCount];

                        for (int i = 0; i < fileInCount; i++)
                        {
                            Bitmap baseImages =
                                new Bitmap(@"E:\Projects\VS2017\OESKProject\WPFApp\Files\Output\"+rSize+gSize+bSize+@"\PictureMess" +
                                           (i + 1) + "_ (" + (times+1)+ ").bmp");

                            areDecTestsActive = true;

                            resultInfo[i] = Job_Decrypt(baseImages, rSize, gSize, bSize);
                        }

                        areDecTestsActive = false;

                        foreach (var result in resultInfo)
                        {
                            FileManager.SaveResultOfTestToTxt(result + Environment.NewLine,
                                resultFilePath + namePart + "_" + rSize + gSize + bSize + ".txt");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("[TestsManager:Dec] " + ex.Message, "Tests Manager Error");
                    return;
                }

                MessageBox.Show("Dec tests have been successfully completed", "Tests - Succes");
            }
            else
            {
                MessageBox.Show("Set parameters for dec tests");
            }
        }

        private static string Job_Decrypt(Bitmap baseImg, int rSize, int gSize, int bSize)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string messageBin = DecryptionManager.Decrypt(baseImg, rSize, gSize, bSize);
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            string message = DataManager.ConvertBinaryStringToMessage(messageBin);
            long messageBinLength = messageBin.Length;
            long baseImgTotalPixCount = baseImg.Height * baseImg.Width;
            long spaceForMessageInBaseImg = baseImgTotalPixCount * (rSize + gSize + bSize);
            long timeOfEcryption = (long)ts.TotalMilliseconds;
            long howManyTimesMessageCanBeHidedInImage = (spaceForMessageInBaseImg * 100) / messageBinLength;

            string resultFormat =
                $"{messageBinLength};{baseImgTotalPixCount};{spaceForMessageInBaseImg};{howManyTimesMessageCanBeHidedInImage};{timeOfEcryption}";

            FileManager.SaveMessageToTxt(message, resultFilePath + DateTime.Now.Ticks + ".txt");

            return resultFormat;
        }

        private static bool areParamOk(int rSize, int gSize, int bSize)
        {
            return !(rSize == 0 && gSize == 0 && bSize == 0);
        }
    }
}
