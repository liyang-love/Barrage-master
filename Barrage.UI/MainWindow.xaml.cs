using DouyuBarrage;
using DouyuBarrage.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Barrage.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// UI线程的同步上下文
        /// </summary>
        private SynchronizationContext m_SyncContext = null;
        private NotifyIcon notifyIcon;
        private Random random;
        private CrawlerThread craw;

        private SettingWindow settingWindow;
        private SharedPreference shared;
        private LogWindow LogWindow;

        public MainWindow()
        {
            InitializeComponent();

            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "已停止... ...";
            this.notifyIcon.ShowBalloonTip(2000);
            this.notifyIcon.Text = "已停止... ...";

            this.notifyIcon.Icon = UI.Properties.Resources.logo;
            this.notifyIcon.Visible = true;
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(Close);

            //设置菜单项
            System.Windows.Forms.MenuItem setting = new System.Windows.Forms.MenuItem("设置");
            setting.Click += (obj, e) =>
            {

                if (settingWindow == null)
                    settingWindow = new SettingWindow(this);
                settingWindow.Show();
            };

            //System.Windows.Forms.MenuItem log = new System.Windows.Forms.MenuItem("日志");
            //log.Click += (obj, e) =>
            //{
            //    if (LogWindow == null)
            //        LogWindow = new LogWindow();
            //    LogWindow.Show();
            //};



            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { setting, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (settingWindow == null)
                        settingWindow = new SettingWindow(this);
                    settingWindow.Show();
                }
            });

            //获取UI线程同步上下文
            m_SyncContext = SynchronizationContext.Current;
            this.Loaded += MainWindow_Loaded;
            this.Deactivated += MainWindow_Deactivated;
            this.StateChanged += MainWindow_StateChanged;
            random = new Random();
            shared = new SharedPreference();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int room = shared.GetIntValue("room");
            string topForm = shared.GetStringValue("topForm");
            this.Topmost = (string.IsNullOrEmpty(topForm)) ? true : Convert.ToBoolean(topForm);
            if (room != 0)
            {
                Start(room);
            }
        }


        public void Start(int room)
        {
            DouyuConfig.room = room;
            if (craw != null)
            {
                craw.DisConnect();
                craw.OnDanmaku -= BarrageMnaager_OnReceiveMessage;
                craw.DisConnectHandler -= Craw_DisConnectHandler;
                craw.ErrorHandler -= Auth_ErrorHandler;
                craw.LogHandler -= Auth_LogHandler;
                craw = null;
            }
            AuthSocket auth = new AuthSocket();
            auth.OnReady += (obj, a) =>
            {
                craw = new CrawlerThread(auth.DanmakuServers, auth.GID, auth.RID);
                craw.OnDanmaku += BarrageMnaager_OnReceiveMessage;
                craw.DisConnectHandler += Craw_DisConnectHandler;
                craw.ErrorHandler += Auth_ErrorHandler;
                craw.LogHandler += Auth_LogHandler;
                craw.Start();
            };
            auth.ErrorHandler += Auth_ErrorHandler;
            auth.LogHandler += Auth_LogHandler;
            auth.Start();
            UpdateState("运行中... ...");
        }

        private void UpdateState(string state)
        {
            notifyIcon.Text = state;
            notifyIcon.BalloonTipText = state;
        }

        #region 回掉
        private void Craw_DisConnectHandler(object sender, EventArgs e)
        {
            m_SyncContext.Post((o) =>
            {
                UpdateState("已停止... ...");
            }, null);
        }

        private void Auth_LogHandler(object sender, Events e)
        {
            m_SyncContext.Post(UpdateLog, e.message);
        }

        private void UpdateLog(object state)
        {
            if (LogWindow != null)
                LogWindow.AppendText(state.ToString());
        }

        private void Auth_ErrorHandler(object sender, Events e)
        {
            m_SyncContext.Post(ShowError, e.message);
        }

        private void ShowError(object state)
        {
            UpdateState("已停止... ...");
            Dialog.Show("提示", state.ToString(), CustomMessageBoxButton.OK, CustomMessageBoxIcon.Error);
            if (craw != null)
                craw.DisConnect();
        }

        private void BarrageMnaager_OnReceiveMessage(object sender, DanmakuEventArgs e)
        {
            m_SyncContext.Post(AddLabel, e.Danmaku);
        }
        #endregion


        private void AddLabel(object obj)
        {
            Danmaku danmaku = obj as Danmaku;
            string message = danmaku.snick + ":" + danmaku.content;
            Barrage(message);
        }


        #region 添加弹幕
        /// <summary>
        /// 在Window界面上显示弹幕信息,速度和位置随机产生
        /// </summary>
        /// <param name="contentlist"></param>
        public void Barrage(string item)
        {
            //获取位置随机数
            double randomtop = random.NextDouble();
            double inittop = (canvas.ActualHeight - 100) * randomtop;
            //获取速度随机数
            //double randomspeed = random.NextDouble();
            //double initspeed = 50 * randomspeed;
            double initspeed = 20;
            //实例化TextBlock和设置基本属性,并添加到Canvas中
            TextBlock textblock = new TextBlock();
            textblock.Text = item;
            textblock.FontSize = 20;
            textblock.Foreground = new SolidColorBrush(ColorUtils.GetRandomColor(random));
            Canvas.SetTop(textblock, inittop);
            canvas.Children.Add(textblock);
            //实例化动画
            DoubleAnimation animation = new DoubleAnimation();
            Timeline.SetDesiredFrameRate(animation, 60);  //如果有性能问题,这里可以设置帧数
            animation.From = canvas.ActualWidth;
            animation.To = 0;
            animation.Duration = TimeSpan.FromSeconds(initspeed);
            animation.AutoReverse = false;
            animation.RepeatBehavior = RepeatBehavior.Forever;
            animation.Completed += (object sender, EventArgs e) =>
            {
                canvas.Children.Remove(textblock);
            };
            //启动动画
            textblock.BeginAnimation(Canvas.LeftProperty, animation);
        }
        #endregion


        #region 窗体事件
        /// <summary>
        /// 确保永远最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// 确保永远置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.notifyIcon.Visible = false;
            if (craw != null)
                craw.DisConnect();
        }

        private void Close(object sender, EventArgs e)
        {
            base.Close();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion



    }
}
