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
    partial class FlatControlsStyleDictionary : ResourceDictionary
    {
        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = (Button)sender;
            Brush borderBrush = button.BorderBrush;
            Brush background = button.Background;

            button.Background = borderBrush;
            button.Foreground = background;
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = (Button)sender;
            Brush background = button.Background;
            Brush Foreground = button.Foreground;

            button.Background = Foreground;
            button.Foreground = background;
            button.BorderBrush = background;
            button.Opacity = 1;
        }
    }
}
