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
    {   // klasa statyczna zawierająca metody obsługujące funkcje tworzenia i wyświetlania menu
        private static SoundBuffer clickBuffer = new SoundBuffer(C.CLICK_SOUND_PATH);
        private static Sound click = new Sound(clickBuffer);
        public static Color defBrdClr = new Color(112, 73, 0, 255);
        public static Color cpltBrdClr = new Color(77, 181, 0, 255);
        public static Color defTxtClr = new Color(112, 73, 0, 255);
        public static Color activeFillClr = new Color(255, 255, 255, 50);
        public static Color rstClr = new Color(123, 224, 76, 255);
        public static Color activeRstClr = new Color(227, 230, 57, 255);
        private static Text titleText;
        public static List<Miniature> miniatures = new List<Miniature>();
        private static Miniature minMars;
        private static Miniature minEarth;
        private static Miniature minKepler;
        private static Miniature minMoon;
        private static Miniature rstBtn;

        public enum Area
        {   // typ wyliczeniowy opisujący obszary w menu
            none,
            earth,
            mars,
            moon,
            kepler,
            reset
        }

        private static void LoadMiniature()
        {   // wczytanie wszystkich elementów menu wraz z dodaniem ich do listy
            minEarth = new Miniature("Earth", C.MIN_EARTH_PATH, defBrdClr, Color.Black, C.LEFT_MARGIN, C.TOP_MARGIN);
            minMars = new Miniature("Mars", C.MIN_MARS_PATH, defBrdClr, Color.Black, C.LEFT_MARGIN + C.MIN_X + C.H_SPACING, C.TOP_MARGIN);
            minMoon = new Miniature("Moon", C.MIN_MOON_PATH, defBrdClr, Color.White, C.LEFT_MARGIN, C.TOP_MARGIN + C.V_SPACING + C.MIN_Y);
            minKepler = new Miniature("Kepler 22b", C.MIN_KEPLER_PATH, defBrdClr, Color.White, C.LEFT_MARGIN + C.MIN_X + C.H_SPACING, C.TOP_MARGIN + C.V_SPACING + C.MIN_Y);
            rstBtn = new Miniature("    RESET\nPROGRESS", C.DEFAULT_WIDTH - C.RIGHT_RESET_MARGIN, C.TOP_RESET_MARGIN, defBrdClr, rstClr);
            miniatures.Add(minEarth);
            miniatures.Add(minMars);
            miniatures.Add(minMoon);
            miniatures.Add(minKepler);
            miniatures.Add(rstBtn);
        }

        public static void LoadMenu()
        {   // wczytanie elementów menu wraz z tytułem gry
            titleText = new Text("Project: Projectile", Projectile.mainFont, C.MAIN_TEXT_SIZE)
            {
                Position = new Vector2f(480f, 20f),
                FillColor = Projectile.MainFontColor
            };
            LoadMiniature();
        }

        public static void CheckClick(ref uint displaying, Loop loop)
        {   // sprawdzenie w jakim obszarze znajdował się kursor w trakcie kliknięcia
            // ustawienie odpowiedniego poziomu w zależności od obszaru
            Area area = MouseEventsMenu.CheckArea(loop);
            if (area != Area.none)
            {
                click.Play();
            }

            switch(area)
            {
                case Area.earth:
                    displaying = 1;
                    break;
                case Area.mars:
                    displaying = 2;
                break;
                case Area.moon:
                    displaying = 3;
                    break;
                case Area.kepler:
                    displaying = 4;
                    break;
                case Area.reset: // wyczyszczenie pliku postępu
                    Progress.ResetProgress ( ref miniatures);
                break;
                case Area.none:
                    break;
            }
        }

        private static void CheckArea(Loop loop)
        {   // sprawdzenie w jakim obszarze znajduje się kursor i aktywowanie podświetlenia
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
        }

        public static void DrawMenu(Loop loop)
        {   // wyświelanie tytułu gry
            loop.Window.Draw(titleText);
            foreach(Miniature min in miniatures)
            {   // zmiana koloru ramki w zależności od ukończenia poziomu
                min.active = false;
                if (min.IsRect())
                {
                    if (min.completed == true && min.rectShp.OutlineColor != cpltBrdClr)
                        min.ChangeColor(cpltBrdClr);
                    if (min.completed == false && min.rectShp.OutlineColor == cpltBrdClr)
                        min.ChangeColor(defBrdClr);
                }
            }
            CheckArea(loop);
            foreach (Miniature min in miniatures)
            {   // zmiana koloru w zależności od obecności kursora w obszarze
                if (min.active == true)
                {
                    if (min.IsRect())
                    {
                        min.ChangeColor(Color.White);
                        min.rectShp.FillColor = activeFillClr;
                    }
                    else
                    {
                        min.ellipse.ChangeEllipseColor(Color.White);
                        min.ellipse.ChangeFillColor(activeRstClr);
                    }
                }
                else if(min.IsRect())
                {   // powrót do standardowych kolorów z podświetlenia dla miniatur poziomów
                    if (min.active == false && (min.rectShp.OutlineColor == Color.White || min.rectShp.OutlineColor == cpltBrdClr))
                    {
                        if (min.completed == false)
                            min.ChangeColor(defBrdClr);
                        else
                            min.ChangeColor(cpltBrdClr);
                        min.rectShp.FillColor = Color.Transparent;
                    }
                }
                else if(!min.IsRect())
                {   // powrót do standardowych kolorów z podświetlenia dla przycisku resetu postępu
                    if (min.active == false && min.ellipse.GetOutlineColor() == Color.White)
                    {
                        min.ellipse.ChangeEllipseColor(defBrdClr);
                        min.ellipse.ChangeFillColor(rstClr);
                    }
                }
                // wyświetlenie każdej miniatury z listy
                min.Draw(loop);
            }
        }
    }
}