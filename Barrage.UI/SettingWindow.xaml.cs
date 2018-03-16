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
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private MainWindow mainWindow;
        private SharedPreference shared;

        public SettingWindow()
        {
            InitializeComponent();
            shared = new SharedPreference();
        }

        public SettingWindow(MainWindow mainWindow) : this()
        {
            this.mainWindow = mainWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int room = shared.GetIntValue("room");
            string topForm = shared.GetStringValue("topForm");
            this.cb_topForm.IsChecked = (string.IsNullOrEmpty(topForm)) ? true : Convert.ToBoolean(topForm);
            txt_room.Text = room + "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int room = Convert.ToInt32(txt_room.Text);
            bool topForm = (bool)cb_topForm.IsChecked;
            shared.Add("topForm", topForm + "");
            mainWindow.Topmost = topForm;
            if (room != shared.GetIntValue("room"))
            {
                shared.Add("room", room);
                mainWindow.Start(room);
            }
            this.Hide();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }


    }
}
