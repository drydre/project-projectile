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
    public class Projectile : Loop
    {   // klasa dziedzicząca po Loop
        public static Font mainFont;
        public static Font theoryFont;
        public static Color backgroundColor = new Color(187, 247, 143, 255);
        public static Color MainFontColor = new Color(112, 73, 0, 255);

        // w konstruktorze ustalono rozdzielczość okna, tytuł oraz kolor po wyczyszczeniu
        public Projectile() : base(C.DEFAULT_WIDTH, C.DEFAULT_HEIGHT, C.DEFAULT_TITLE, backgroundColor) { }
        public static void LoadFont()
        {   // wczytanie czcionek używanych w programie
            mainFont = new Font(C.MAINFONT_PATH);
            theoryFont = new Font(C.THEORY_FONT_PATH);
        }

        public override void Load() 
        {   // dane wczytywane jednorazowo po uruchomieniu
            LoadFont();
            DisplayInfo.LoadFont();
            DisplayMenu.LoadMenu();
            Progress.LoadProgress();
        }

        public override void SetValue(Frames frames) 
        {   // odświeżenie obliczeń w grze
            if (displaying != 0)
                level.UpdateLevel(this);
        }
        public override void Draw(Frames frames) 
        {   // ustawienie elementów w oknie po odświeżeniu danych
            if (displaying == 0)
            {   // dla menu
                DisplayMenu.DrawMenu(this);
            }     
            else // dla poziomu
                level.Draw(this);
            DisplayInfo.DrawInfo(this, Color.Red);
        }
    }
}