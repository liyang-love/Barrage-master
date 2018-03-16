using DouyuBarrage;
using DouyuBarrage.Entity;
using DouyuBarrage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DouyuConfig.room = 111716;

            AuthSocket auth = new AuthSocket();
            auth.OnReady += (sender,e)=> {

                CrawlerThread craw = new CrawlerThread(auth.DanmakuServers, auth.GID, auth.RID);
                craw.DisConnectHandler += Craw_DisConnectHandler;
                craw.ErrorHandler += Auth_ErrorHandler;
                craw.LogHandler += Auth_LogHandler;
                craw.OnDanmaku += Craw_OnDanmaku;
                craw.Start();

            };
            auth.Start();
            auth.ErrorHandler += Auth_ErrorHandler;
            auth.LogHandler += Auth_LogHandler;
            
            Console.Read();

        }

        private static void Craw_OnDanmaku(object sender, DanmakuEventArgs e)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(e.Danmaku.content);
            Console.WriteLine("-------------------------------------------------");

        }

        private static void Craw_DisConnectHandler(object sender, EventArgs e)
        {
            Console.WriteLine("断开链接");
        }

        private static void Auth_LogHandler(object sender, Events e)
        {
            Console.WriteLine("info:"+e.message);
        }

        private static void Auth_ErrorHandler(object sender, Events e)
        {
            Console.WriteLine("Error:"+e.message);
        }
    }
}
