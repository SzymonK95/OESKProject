using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp.Helpers
{
    public static class DataManager
    {
        public const string STOP = @"#@#";

        public static string ConvertTextToBinarySystem(string text)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var c in text + STOP)
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(16, '0'));
            }

            return sb.ToString();
        }

        public static string ConvertBinaryStringToMessage(string binaryString)
        {
            StringBuilder sb = new StringBuilder();

            int size = 16;
            int itemToAdd = 0;
            for (int i = 0; i < binaryString.Length; i += size)
            {
                try
                {
                    itemToAdd = Convert.ToInt16(GetPartOfMessageByPosition(binaryString, i, size), 2);
                    sb.Append((Char) itemToAdd);
                }
                catch (Exception ex)
                {
                    sb.Append("?");
                }

            }

            string message = sb.ToString().Split(new string[]
            {
                STOP
            }, StringSplitOptions.None).First();

            return message;
        }

        public static string ConvertColorValueToString(byte colorValue)
        {
            return Convert.ToString(colorValue, 2).PadLeft(8, '0');
        }

        public static string GetPartOfMessageByPosition(string message, int messagePosition, int sizeOfPart)
        {
            try
            {
                return message.Substring(messagePosition, sizeOfPart);
            }
            catch (Exception ex)
            {
                return message.Substring(messagePosition, message.Length - messagePosition);
            }
        }
    }
}
