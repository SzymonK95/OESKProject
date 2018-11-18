using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using MvvmDialogs;
using WPFApp.Helpers;
using Image = System.Drawing.Image;
using System.Windows;
using MvvmDialogs.FrameworkDialogs.MessageBox;

namespace WPFApp.ViewModel
{
    public class ParametersViewModel : ViewModelBase
    {
        #region Parameters

        private int decryptionProgressBarValue;
        public int DecryptionProgressBarValue
        {
            get => decryptionProgressBarValue;
            set
            {
                decryptionProgressBarValue = value;
                NotifyPropertyChanged(nameof(DecryptionProgressBarValue));
            }
        }

        private int encryptionProgressBarValue;
        public int EncryptionProgressBarValue
        {
            get => encryptionProgressBarValue;
            set
            {
                encryptionProgressBarValue = value;
                NotifyPropertyChanged(nameof(EncryptionProgressBarValue));
            }
        }

        private int decryptionProgressBarMaxValue;
        public int DecryptionProgressBarMaxValue
        {
            get => decryptionProgressBarMaxValue;
            set
            {
                decryptionProgressBarMaxValue = value;
                NotifyPropertyChanged(nameof(DecryptionProgressBarMaxValue));
            }
        }

        private int encryptionProgressBarMaxValue;
        public int EncryptionProgressBarMaxValue
        {
            get => encryptionProgressBarMaxValue;
            set
            {
                encryptionProgressBarMaxValue = value;
                NotifyPropertyChanged(nameof(EncryptionProgressBarMaxValue));
            }
        }

        private bool encryptButtonIsEnabled;
        public bool EncryptButtonIsEnabled
        {
            get => encryptButtonIsEnabled;
            set
            {
                encryptButtonIsEnabled = value;
                NotifyPropertyChanged(nameof(EncryptButtonIsEnabled));
            }
        }

        private bool decryptButtonIsEnabled;
        public bool DecryptButtonIsEnabled
        {
            get => decryptButtonIsEnabled;
            set
            {
                decryptButtonIsEnabled = value;
                NotifyPropertyChanged(nameof(DecryptButtonIsEnabled));
            }
        }

        private string basicImgPath;
        public string BasicImgPath
        {
            get
            {
                if (basicImgPath == null) return null;
                return basicImgPath.Length < 30 ? basicImgPath : basicImgPath.Split('\\').Last();
            }
            set
            {
                basicImgPath = value;
                NotifyPropertyChanged(nameof(BasicImgPath));
            }
        }

        private string messagePath;
        public string MessagePath
        {
            get
            {
                if (messagePath == null) return null;
                return messagePath.Length < 30 ? messagePath : messagePath.Split('\\').Last();
            }
            set
            {
                messagePath = value;
                NotifyPropertyChanged(nameof(MessagePath));
            }
        }

        private Bitmap imageWithMessage;
        public Bitmap ImageWithMessage
        {
            get => imageWithMessage;
            set
            {
                imageWithMessage = value;
                NotifyPropertyChanged(nameof(ImageWithMessage));
            }
        }

        public string messageFromImage;
        public string MessageFromImage
        {
            get => messageFromImage;
            set
            {
                messageFromImage = value;
                NotifyPropertyChanged(nameof(MessageFromImage));
            }
        }

        private int sliderRedValue;
        public int SliderRedValue
        {
            get => sliderRedValue;
            set
            {
                if (value != sliderRedValue)
                {
                    sliderRedValue = value;
                    NotifyPropertyChanged(nameof(SliderRedValue));
                }
            }
        }

        private int sliderGreenValue;
        public int SliderGreenValue
        {
            get => sliderGreenValue;
            set
            {
                if (value != sliderGreenValue)
                {
                    sliderGreenValue = value;
                    NotifyPropertyChanged(nameof(SliderGreenValue));
                }
            }
        }

        private int sliderBlueValue;
        public int SliderBlueValue
        {
            get => sliderBlueValue;
            set
            {
                if (value != sliderBlueValue)
                {
                    sliderBlueValue = value;
                    NotifyPropertyChanged(nameof(SliderBlueValue));
                }
            }
        }

        private bool _isSelectedCheckBoxMessageType;
        public bool IsSelectedCheckBoxMessageType
        {
            get => _isSelectedCheckBoxMessageType;
            set
            {
                _isSelectedCheckBoxMessageType = value;
                NotifyPropertyChanged(nameof(IsSelectedCheckBoxMessageType));
            }
        }

        public string Title => "Parameters";

        public string EncryptButton_ButtonContent => "Encrypt";

        public string DecryptButton_ButtonContent => "Decrypt";

        public string BasicImgLoadButton_ButtonContent => "Load basic img";

        public string MessageLoadButton_ButtonContent => "Load message";

        public string TestingButton_ButtonContent => "Start tests";

        #endregion

        #region Constructors
        public ParametersViewModel()
        {
            Log.Info("[ParametersViewModel] Constructor");
            SetEncryptionProgressBarValues();
            SetDecryptionProgressBarValues();
            DecryptButtonIsEnabled = true;
            EncryptButtonIsEnabled = true;
        }
        #endregion

        #region Methods

        public void EncryptButton_ClickMethod()
        {
            if (EncryptParametersOK())
            {
                int rSize = SliderRedValue;
                int gSize = SliderGreenValue;
                int bSize = SliderBlueValue;
                Bitmap baseImage = new Bitmap(basicImgPath);
                String message = File.ReadAllText(messagePath);

                SetEncryptionProgressBarValues(maxValue: baseImage.Width + 10);

                if (IsSelectedCheckBoxMessageType != true)
                {
                    message = DataManager.ConvertTextToBinarySystem(message);
                }

                EncryptButtonIsEnabled = false;

                new Thread(() => 
                    ImageWithMessage = EncryptionManager.Encrypt(message, baseImage, rSize, gSize, bSize)).Start();

                new Thread(Job_ProgressBarValueUpdateAndSaveImage).Start();
            }
            else
            {
                MessageBox.Show("Problem with input data");
            }
        }

        public void DecryptButton_ClickMethod()
        {
            if (DecryptParametersOK())
            {
                Bitmap baseImage = new Bitmap(basicImgPath);
                int rSize = SliderRedValue;
                int gSize = SliderGreenValue;
                int bSize = SliderBlueValue;

                SetDecryptionProgressBarValues(maxValue: baseImage.Width + 10);

                DecryptButtonIsEnabled = false;

                new Thread(() => MessageFromImage = DecryptionManager.Decrypt(baseImage, rSize, gSize, bSize)).Start();

                new Thread(Job_ProgressBarValueUpdateAndSaveTxt).Start();
            }
            else
            {
                MessageBox.Show("Problem with input data");
            }
        }

        public void BasicImgLoadButton_ClickMethod()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                BasicImgPath = openFileDialog.FileName;
            }
        }

        public void MessageLoadButton_ClickMethod()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Txt files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                MessagePath = openFileDialog.FileName;
            }
        }

        public void TestingButton_ClickMethod()
        {
            MessageBox.Show("Test starts");
        }

        #region Private

        private void SetEncryptionProgressBarValues(int value = 0, int maxValue = Int32.MaxValue)
        {

            EncryptionProgressBarMaxValue = maxValue;
            EncryptionProgressBarValue = value;
        }

        private void SetDecryptionProgressBarValues(int value = 0, int maxValue = Int32.MaxValue)
        {

            DecryptionProgressBarMaxValue = maxValue;
            DecryptionProgressBarValue = value;
        }

        private void Job_ProgressBarValueUpdateAndSaveImage()
        {
            while (EncryptionProgressBarValue < EncryptionProgressBarMaxValue)
            {
                Thread.Sleep(100);
                EncryptionProgressBarValue = EncryptionManager.Progress;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap Image|*.bmp";
            saveFileDialog.Title = "Save the bitmap with Your message";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                SaveImageWithMessageToBmp(ImageWithMessage, saveFileDialog.FileName);
            }
            else
            {
                SaveImageWithMessageToBmp(ImageWithMessage);
            }

            SetEncryptionProgressBarValues();
            EncryptButtonIsEnabled = true;
        }

        private void Job_ProgressBarValueUpdateAndSaveTxt()
        {
            while (DecryptionProgressBarValue < DecryptionProgressBarMaxValue)
            {
                Thread.Sleep(100);
                DecryptionProgressBarValue = DecryptionManager.Progress;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Txt File|*.txt";
            saveFileDialog.Title = "Save the message to Txt File";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                SaveMessageToTxt(MessageFromImage, saveFileDialog.FileName);
            }
            else
            {
                SaveMessageToTxt(MessageFromImage);
            }

            SetDecryptionProgressBarValues();
            DecryptButtonIsEnabled = true;
        }

        private bool EncryptParametersOK()
        {
            bool isOK = false;

            isOK = File.Exists(BasicImgPath);
            isOK = File.Exists(MessagePath);
            isOK = !(SliderRedValue == 0 && SliderGreenValue == 0 && SliderBlueValue == 0);

            return isOK;
        }

        private bool DecryptParametersOK()
        {
            bool isOK = false;

            isOK = File.Exists(BasicImgPath);
            isOK = !(SliderRedValue == 0 && SliderGreenValue == 0 && SliderBlueValue == 0);

            return isOK;
        }

        private void SaveImageWithMessageToBmp(Bitmap image, string filePath = "")
        {
            if (filePath.Equals(String.Empty))
            {
                filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) 
                    + @"\image" + DateTime.Now.Ticks + ".bmp";
            }

            image.Save(filePath, ImageFormat.Bmp);
        }

        private void SaveMessageToTxt(string message, string filePath = "")
        {
            if (filePath.Equals(String.Empty))
            {
                filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    + @"message" + DateTime.Now.Ticks + ".txt";
            }

            System.IO.File.WriteAllText(filePath, message);
        }
        #endregion

        #endregion

        #region Commands

        public ICommand EncryptButton_ClickCommand => new RelayCommand(EncryptButton_ClickMethod, AlwaysTrue);
        public ICommand DecryptButton_ClickCommand => new RelayCommand(DecryptButton_ClickMethod, AlwaysTrue);
        public ICommand BasicImgLoadButton_ClickCommand => new RelayCommand(BasicImgLoadButton_ClickMethod, AlwaysTrue);
        public ICommand MessageLoadButton_ClickCommand => new RelayCommand(MessageLoadButton_ClickMethod, AlwaysTrue);
        public ICommand TestingButton_ClickCommand => new RelayCommand(TestingButton_ClickMethod, AlwaysTrue);

        #endregion

        #region Events

        #endregion
    }
}
