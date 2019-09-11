using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageSteganographyLibrary;
using TextSteganographyLibrary;
using System.IO;

namespace IConcealUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        private BinaryEmbedStruct binaryEmbedStruct;

        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            binaryEmbedStruct = new BinaryEmbedStruct();
        }

        //UI
        private void TopPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Opacity = 0.7;
            DragMove();
            Opacity = 1;
        }

        private void PasswordProtectCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ZipArchivePassword.Visibility = Visibility.Visible;
        }

        private void PasswordProtectCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ZipArchivePassword.Visibility = Visibility.Hidden;
            ZipArchivePassword.Password = "";
        }

        //Image Steganography
        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            binaryEmbedStruct.Image = FileSelectorUtility.GetImage();
            LoadImageDetails();
        }

        private void LoadImageDetails()
        {
            ImagePath.Text = binaryEmbedStruct.Image.FullName;
            ImageName.Text = binaryEmbedStruct.Image.Name;

            ImageSource imageSource = new BitmapImage(new Uri(binaryEmbedStruct.Image.FullName));
            SelectedImage.Source = imageSource;
            ImageBorder.Visibility = Visibility.Visible;
        }

        private void SelectFiles(object sender, MouseButtonEventArgs e)
        {
            if(FilesRadioButton.IsChecked == true)
            {
                binaryEmbedStruct.Files = FileSelectorUtility.GetFiles();
                LoadFilesDetails();
            }
                

            if (DirectoryRadioButton.IsChecked == true)
            {
                binaryEmbedStruct.Directory = FileSelectorUtility.GetDirectory();
                FilesPath.Text = binaryEmbedStruct.Directory.FullName;
            }

        }

        private void LoadFilesDetails()
        {
            StringBuilder sb = new StringBuilder();
            foreach(FileInfo fi in binaryEmbedStruct.Files)
            {
                sb.Append(fi.Name);
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 1, 1);

            FilesPath.Text = sb.ToString();
        }

        private void ZipFiles()
        {
            if(FilesRadioButton.IsChecked == true)
            {
                if (string.IsNullOrEmpty(ZipArchivePassword.Password))
                    binaryEmbedStruct.ZipFileLocation = new FileInfo(FileZipper.ArchiveFiles(binaryEmbedStruct.Files, null));
                else
                    binaryEmbedStruct.ZipFileLocation = new FileInfo(FileZipper.ArchiveFiles(binaryEmbedStruct.Files, ZipArchivePassword.Password));
            }
            else if(DirectoryRadioButton.IsChecked == true)
            {
                if (string.IsNullOrEmpty(ZipArchivePassword.Password))
                    binaryEmbedStruct.ZipFileLocation = new FileInfo(FileZipper.ArchiveDirectory(binaryEmbedStruct.Directory, null));
                else
                    binaryEmbedStruct.ZipFileLocation = new FileInfo(FileZipper.ArchiveDirectory(binaryEmbedStruct.Directory, ZipArchivePassword.Password));
            }
        }

        private void Encrypt(object sender, MouseButtonEventArgs e)
        {
            if (binaryEmbedStruct.Image == null || (binaryEmbedStruct.Files == null && binaryEmbedStruct.Directory == null))
                return;

            ZipFiles();

            //MessageBox.Show("Stegano Image Saved to :" +
            //    BinaryEmbed.StandardEmbed(binaryEmbedStruct.Image, binaryEmbedStruct.ZipFileLocation, false));

            string output = BinaryEmbed.StandardEmbed(binaryEmbedStruct.Image, binaryEmbedStruct.ZipFileLocation, false);
            MessageBox.Show($"Stegano Image Saved To : {output}");
            System.Diagnostics.Process.Start("explorer.exe", $"/select, \"{output}\"");
        }

        //Text Steganography
        private void EncodeTextMessage(object sender, MouseButtonEventArgs e)
        {
            OutputTextAreaTextBlock.Text = "The Following Text is Encoded";
            OutputTextArea.Text = ZeroWidthConverter.Embed(CarrierTextArea.Text, SecretTextArea.Text);
        }

        private void DecodeTextMessage(object sender, MouseButtonEventArgs e)
        {
            OutputTextAreaTextBlock.Text = "Message Extracted";
            OutputTextArea.Text = ZeroWidthExtractor.Extract(PayloadTextArea.Text);
        }


        //Utility Methods
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(TabItem tab in MainTabControl.Items)
            {
                tab.Visibility = Visibility.Collapsed;
            }
        }

        
    }
}
