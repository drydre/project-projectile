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
        public static SoundBuffer clickBuffer = new SoundBuffer(C.CLICK_SOUND_PATH);
        public static Sound click = new Sound(clickBuffer);
        public static Color DefBrdClr = new Color(112, 73, 0, 255);
        public static Color CpltBrdClr = new Color(77, 181, 0, 255);
        public static Color DefTxtClr = new Color(112, 73, 0, 255);
        public static Color ActiveFillClr = new Color(255, 255, 255, 50);
        public static Color RstClr = new Color(123, 224, 76, 255);
        public static Color ActiveRstClr = new Color(227, 230, 57, 255);
        public static List<Miniature> miniatures = new List<Miniature>();
        public static Font mainFont;
        public static Miniature minMars;
        public static Miniature minEarth;
        public static Miniature minKepler;
        public static Miniature minMoon;
        public static Miniature rstBtn;
        public enum Area
        {
            none,
            earth,
            mars,
            moon,
            kepler,
            reset
        }

        public static void ResetProgress()
        {

        }

        public static void LoadFont()
        {
            mainFont = new Font(C.MAINFONT_PATH);
        }

        public static void LoadMiniature()
        {
            minEarth = new Miniature("Earth", C.MIN_EARTH_PATH, DefBrdClr, Color.Black, C.LEFT_MARGIN, C.TOP_MARGIN);
            minMars = new Miniature("Mars", C.MIN_MARS_PATH, DefBrdClr, Color.Black, C.LEFT_MARGIN + C.MIN_X + C.H_SPACING, C.TOP_MARGIN);
            minMoon = new Miniature("Moon", C.MIN_MOON_PATH, DefBrdClr, Color.White, C.LEFT_MARGIN, C.TOP_MARGIN + C.V_SPACING + C.MIN_Y);
            minKepler = new Miniature("Kepler 22b", C.MIN_KEPLER_PATH, DefBrdClr, Color.White, C.LEFT_MARGIN + C.MIN_X + C.H_SPACING, C.TOP_MARGIN + C.V_SPACING + C.MIN_Y);
            rstBtn = new Miniature("    RESET\nPROGRESS", C.RST_X_RAD, C.RST_Y_RAD, C.DEFAULT_WIDTH - C.RIGHT_RESET_MARGIN, C.TOP_RESET_MARGIN, DefBrdClr, RstClr);
            miniatures.Add(minEarth);
            miniatures.Add(minMars);
            miniatures.Add(minMoon);
            miniatures.Add(minKepler);
            miniatures.Add(rstBtn);
        }

        public static void CheckClick(ref int level, Loop loop)
        {
            Area area = MouseEventsMenu.CheckArea(loop);
            if (area != Area.none)
            {
                click.Play();
            }

            switch(area)
            {
                case Area.earth:
                    level = 1;
                break;
                case Area.mars:
                    level = 2;
                break;
                case Area.moon:
                    level = 3;
                    break;
                case Area.kepler:
                    level = 4;
                    break;
                case Area.reset:
                    Progress.ResetProgress ( ref miniatures, "progress");
                break;
                case Area.none:
                    break;
            }
        }

        public static void DrawMenu(Loop loop, Color fontColor)
        {
            Text txtTitle = new Text("Project: Projectile", mainFont, C.MAIN_TEXT_SIZE);
            txtTitle.Position = new Vector2f(480f, 20f);
            txtTitle.Color = fontColor;
            loop.Window.Draw(txtTitle);
            foreach(Miniature min in miniatures)
            {
                min.active = false;
                if (min.IsRect())
                {
                    if (min.completed == true && min.rectShp.OutlineColor != CpltBrdClr)
                        min.ChangeColor(CpltBrdClr);
                    if (min.completed == false && min.rectShp.OutlineColor == CpltBrdClr)
                        min.ChangeColor(DefBrdClr);
                }
            }
            switch (MouseEventsMenu.CheckArea(loop))
            {
                case Area.earth:
                    minEarth.active = true;
                    break;
                case Area.mars:
                    minMars.active = true;
                    break;
                case Area.moon:
                    minMoon.active = true;
                    break;
                case Area.kepler:
                    minKepler.active = true;
                    break;
                case Area.reset:
                    rstBtn.active = true;
                    break;
                case Area.none:
                    break;
            }
            foreach (Miniature min in miniatures)
            {
                if (min.active == true)
                {
                    if (min.IsRect())
                    {
                        min.ChangeColor(Color.White);
                        min.rectShp.FillColor = ActiveFillClr;
                    }
                    else
                    {
                        min.ChangeEllipseColor(Color.White);
                        min.ellipse.FillColor = ActiveRstClr;
                    }
                }
                else if(min.IsRect())
                {
                    if (min.active == false && min.rectShp.OutlineColor == Color.White)
                    {
                        min.ChangeColor(DefBrdClr);
                        min.rectShp.FillColor = Color.Transparent;
                    }
                }
                else if(!min.IsRect())
                {
                    if (min.active == false && min.ellipse.OutlineColor == Color.White)
                    {
                        min.ChangeEllipseColor(DefBrdClr);
                        min.ellipse.FillColor = RstClr;
                    }
                }
                min.Draw(loop);
            }
        }
    }
}