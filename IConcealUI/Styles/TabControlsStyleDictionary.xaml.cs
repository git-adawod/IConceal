using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IConcealUI.Styles
{
    partial class TabControlsStyleDictionary : ResourceDictionary
    {
        private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock tblock = sender as TextBlock;
            tblock.Background = new SolidColorBrush(Color.FromArgb(5, 211, 211, 211));
        }

        private void TextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock tblock = sender as TextBlock;
            tblock.Background = new SolidColorBrush(Color.FromArgb(255, 29, 29, 29));
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock t = sender as TextBlock;

            foreach(TabItem tab in MainWindow.mainWindow.MainTabControl.Items)
            {
                if(tab.Header.ToString() == t.Text)
                {
                    MainWindow.mainWindow.MainTabControl.SelectedItem = tab;
                    MainWindow.mainWindow.Title = $"IConceal ({t.Text} Steganography)";
                }
            } 
        }
    }
}
