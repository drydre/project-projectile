using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Projectile
{
    public static class DisplayInfo
    {
        public static Font infoFont;

        public static void LoadFont()
        {
            infoFont = new Font(C.FONT_PATH);
        }

        

        public static void DrawInfo(Loop loop, Color fontColor)
        {
            if (infoFont == null)
              return;
            string totalTimeString = loop.Frames.TotalTime.ToString("0.000");
            string deltaString = loop.Frames.Delta.ToString("0.000");
            float fps = 1f/loop.Frames.Delta;
            string fpsString = fps.ToString("0.000");
            Text txtTime = new Text(totalTimeString, infoFont, 16);
            txtTime.Position = new Vector2f(15f, 5f);
            txtTime.Color = fontColor;

            Text txtFPS = new Text(fpsString, infoFont, 16);
            txtFPS.Position = new Vector2f(15f, 25f);
            txtFPS.Color = fontColor;
            loop.Window.Draw(txtTime);
            loop.Window.Draw(txtFPS);
        }
    }


}
