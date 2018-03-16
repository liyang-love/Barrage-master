using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Dialog.xaml 的交互逻辑
    /// </summary>
    public partial class Dialog : Window
    {
        private CustomMessageBoxButton Buttons;
        public CustomMessageBoxResult Result { get; private set; }
        public Dialog(CustomMessageBoxButton Buttons)
        {
            InitializeComponent();
            this.DataContext = this;

            Result = CustomMessageBoxResult.None;
            BitmapImage imgSource = new BitmapImage(new Uri(@"logo.ico", UriKind.Relative));
            image_logo.Source = imgSource;
            this.Buttons = Buttons;
            switch (Buttons)
            {
                case CustomMessageBoxButton.OK:
                    btn_ok.Content = "确 定";
                    btn_cancel.Visibility = Visibility.Collapsed;
                    btn_yes.Visibility = Visibility.Collapsed;
                    break;
                case CustomMessageBoxButton.OKCancel:
                    btn_ok.Content = "确 定";
                    btn_cancel.Content = "取 消";
                    btn_yes.Visibility = Visibility.Collapsed;
                    break;
                case CustomMessageBoxButton.YesNo:
                    btn_cancel.Content = "否";
                    btn_ok.Visibility = Visibility.Collapsed;
                    break;
                case CustomMessageBoxButton.YesNoCancel:
                    break;
                default:
                    break;
            }

        }

        public new string Title
        {
            set { lbl_title.Content = value; }
        }
        public string Message
        {
            set { lbl_msg.Content = value; }
        }

        public Visibility CancelVisibility
        {
            set { btn_cancel.Visibility = value; }
        }

        public static CustomMessageBoxResult Show(string title,string msg, CustomMessageBoxButton Buttons, CustomMessageBoxIcon BoxIcon)
        {
            Dialog dialog = new Dialog(Buttons);
            dialog.Title = title;
            dialog.Message = msg;
            dialog.ShowDialog();
            return dialog.Result;
        }




        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.OK;
            this.Close();
        }

        private void btn_yes_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.Yes;
            this.Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.Cancel;
            this.Close();
        }
    }

    /// <summary>
    /// 显示按钮类型
    /// </summary>
    public enum CustomMessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNo = 2,
        YesNoCancel = 3
    }
    /// <summary>
    /// 消息框的返回值
    /// </summary>
    public enum CustomMessageBoxResult
    {
        //用户直接关闭了消息窗口
        None = 0,
        //用户点击确定按钮
        OK = 1,
        //用户点击取消按钮
        Cancel = 2,
        //用户点击是按钮
        Yes = 3,
        //用户点击否按钮
        No = 4
    }
    /// <summary>
    /// 图标类型
    /// </summary>
    public enum CustomMessageBoxIcon
    {
        None = 0,
        Error = 1,
        Question = 2,
        Warning = 3
    }
}
