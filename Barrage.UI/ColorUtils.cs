using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Barrage.UI
{
    public static class ColorUtils
    {

        public static Color[] Colors=new Color[15];

        static ColorUtils()
        {
            Colors[0] = Color.FromRgb(7,249,84);
            Colors[1] = Color.FromRgb(0,0,0);
            Colors[2] = Color.FromRgb(245, 17, 17);
            Colors[3] = Color.FromRgb(58, 17, 245);
            Colors[4] = Color.FromRgb(240, 251, 4);
            Colors[5] = Color.FromRgb(224, 14, 100);
            Colors[6] = Color.FromRgb(255, 255, 255);
            Colors[7] =(Color) ColorConverter.ConvertFromString("#EE2C2C");
            Colors[8] = (Color)ColorConverter.ConvertFromString("#B22222");
            Colors[9] = (Color)ColorConverter.ConvertFromString("#9400D3");
            Colors[10] = (Color)ColorConverter.ConvertFromString("#363636");
            Colors[11] = (Color)ColorConverter.ConvertFromString("#404040");
            Colors[12] = (Color)ColorConverter.ConvertFromString("#383838");
            Colors[13] = (Color)ColorConverter.ConvertFromString("#1A1A1A");
            Colors[14] = (Color)ColorConverter.ConvertFromString("#1E90FF");
        }

        public static Color GetRandomColor( Random random)
        {
            return Colors[random.Next(0,Colors.Length)];
        }

    }
}
