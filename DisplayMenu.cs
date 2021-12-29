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
    /** Klasa statyczna zawierająca metody obsługujące funkcje tworzenia i wyświetlania menu.*/
    public class DisplayMenu
    {   
        /** bufor dźwięku kliknięcia*/
        private static SoundBuffer clickBuffer = new SoundBuffer(C.CLICK_SOUND_PATH);
        /** dźwięk klikniecia*/
        private static Sound click = new Sound(clickBuffer);
        /** domyślny kolor obramowania*/
        public static Color defBrdClr = new Color(112, 73, 0, 255);
        /** kolor obramowania po ukończeniu*/
        public static Color cpltBrdClr = new Color(77, 181, 0, 255);
        /** domyślny kolor tekstu*/
        public static Color defTxtClr = new Color(112, 73, 0, 255);
        /** rozjaśnienie miniatury po uaktywnieniu*/
        public static Color activeFillClr = new Color(255, 255, 255, 50);
        /** kolor przycisku resetu*/
        public static Color rstClr = new Color(123, 224, 76, 255);
        /** kolor aktywnego przycisku resetu*/
        public static Color activeRstClr = new Color(227, 230, 57, 255);
        /** tekst tytułowy*/
        private static Text titleText;
        /** lista miniatur*/
        public static List<Miniature> miniatures = new List<Miniature>();
        /** miniatura Marsa*/
        private static Miniature minMars;
        /** miniatura Ziemi*/
        private static Miniature minEarth;
        /** miniatura Keplera*/
        private static Miniature minKepler;
        /** miniatura Księżyca*/
        private static Miniature minMoon;
        /** miniatura przycisku resetu*/
        private static Miniature rstBtn;

        /** typ wyliczeniowy opisujący obszary w menu*/
        public enum Area
        {   
            none,
            earth,
            mars,
            moon,
            kepler,
            reset
        }

        /** metoda wczytująca wszystkie elementy menu wraz z dodaniem ich do listy*/
        private static void LoadMiniature()
        {   
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

        /** metoda wczytująca elementy menu wraz z tytułem gry*/
        public static void LoadMenu()
        {   
            titleText = new Text("Project: Projectile", Projectile.mainFont, C.MAIN_TEXT_SIZE)
            {
                Position = new Vector2f(480f, 20f),
                FillColor = Projectile.MainFontColor
            };
            LoadMiniature();
        }

        /**
          * metoda sprawdzająca w jakim obszarze znajdował się kursor w trakcie kliknięcia,
          * ustawiająca odpowiedni poziom w zależności od obszaru
          * @param displaying opisuje wyświetlaną aktualnie zawartość ekranu
          * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
          */
        public static void CheckClick(ref uint displaying, Loop loop)
        {   
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

        /**
         * metoda sprawdzajaca w jakim obszarze znajduje się kursor i aktywująca podświetlenie
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        private static void CheckArea(Loop loop)
        {   
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

        /**
         * metoda ustawiająca na ekranie miniatury w odpowiednich wariantach kolorów
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
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