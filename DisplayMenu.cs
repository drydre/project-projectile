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
    public class DisplayMenu
    {

        public const uint MAIN_TEXT_SIZE = 42;
        public const uint NAME_TEXT_SIZE = 30;
        public static Color DefBrdClr = new Color(112, 73, 0, 255);
        public static Color CpltBrdClr = new Color(187, 247, 143, 255);
        public static Color DefTxtClr = new Color(112, 73, 0, 255);
        public const float DEF_BRD_THICKNESS = 6f;
        public const string MIN_EARTH_PATH = "../../media/images/earth_min.png";
        public const string MIN_MARS_PATH = "../../media/images/mars_min.png";
        public const string MIN_KEPLER_PATH = "../../media/images/kepler_22b_min.png";
        public const string MIN_MOON_PATH = "../../media/images/moon_min.png";
        public const string MAINFONT_PATH = "../../fonts/BERNHC.ttf";
        public static Font mainFont;
        public static Miniature minMars;
        public static Miniature minEarth;
        public static Miniature minKepler;
        public static Miniature minMoon;


        public static void LoadFont()
        {
            mainFont = new Font(MAINFONT_PATH);
        }

        public static void LoadMiniature()
        {
            minEarth = new Miniature("Earth", MIN_EARTH_PATH, DefBrdClr, Color.Black, 150f, 150f);
            minMars = new Miniature("Mars", MIN_MARS_PATH, DefBrdClr, Color.Black, 713f, 150f);
            minMoon = new Miniature("Moon", MIN_MOON_PATH, DefBrdClr, Color.White, 150f, 435f);
            minKepler = new Miniature("Kepler 22b", MIN_KEPLER_PATH, DefBrdClr, Color.White, 713f, 435f);
        }



        public static void DrawMenu(Loop loop, Color fontColor)
        {
            Text txtTitle = new Text("Project: Projectile", mainFont, MAIN_TEXT_SIZE);
            txtTitle.Position = new Vector2f(480f, 20f);
            txtTitle.Color = fontColor;
            loop.Window.Draw(txtTitle);
            minEarth.Draw(loop);
            minMars.Draw(loop);
            minMoon.Draw(loop);
            minKepler.Draw(loop);
        }
    }

}