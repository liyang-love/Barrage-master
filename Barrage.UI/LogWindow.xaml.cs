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
using System.Windows.Shapes;

namespace Barrage.UI
{
    /// <summary>
    /// LogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : Window
    {
        private int Lines = 0;

        public LogWindow()
        {
            InitializeComponent();
        }

        public void AppendText(string msg)
        {
            if(Lines>500)
            {
                richTextBox.Document.Blocks.Clear();
                Lines = 0;
            }
            richTextBox.AppendText(msg+"\n");
            Lines++;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

    }
}
