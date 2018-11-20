using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Helpers
{
    public class TestsManager
    {
        private string resultFilePath = "ResultEnryption.txt";

        public void Test()
        {
            //wczytanie plikow
            //puszczenie watkow
        }

        public void Job_Encrypt(string message, Bitmap baseImg, int rSize, int gSize, int bSize)
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
            double timeOfEcryption = ts.TotalMilliseconds;
            double percentOfMessageHidedInBaseImg;

            if (messageBinLength > spaceForMessageInBaseImg)
                percentOfMessageHidedInBaseImg = spaceForMessageInBaseImg / messageBinLength * 100;
            else
                percentOfMessageHidedInBaseImg = 100;
            
            string resultFormat = String.Format("{0};{1};{2};{3};{4}",
                message.Length,
                baseImgTotalPixCount,
                spaceForMessageInBaseImg,
                percentOfMessageHidedInBaseImg,
                timeOfEcryption
                );

            System.IO.File.WriteAllText(resultFilePath, resultFormat,Encoding.UTF32);

        }
    }
}
