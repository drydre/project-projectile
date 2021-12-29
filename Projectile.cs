﻿using System;
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
    /** Klasa dziedzicząca po Loop.*/
    public class Projectile : Loop
    {   
        public static Font mainFont;
        public static Font theoryFont;
        public static Color backgroundColor = new Color(187, 247, 143, 255);
        public static Color MainFontColor = new Color(112, 73, 0, 255);

        /** W konstruktorze ustalono rozdzielczość okna, tytuł oraz kolor po wyczyszczeniu.*/
        public Projectile() : base(C.DEFAULT_WIDTH, C.DEFAULT_HEIGHT, C.DEFAULT_TITLE, backgroundColor) { }

        /** metoda wczytywania czcionek używanych w programie*/
        public static void LoadFont()
        {   
            mainFont = new Font(C.MAINFONT_PATH);
            theoryFont = new Font(C.THEORY_FONT_PATH);
        }

        /** metoda jednorazowego wczytywania danych po uruchomieniu*/
        public override void Load() 
        {   
            LoadFont();
            DisplayInfo.LoadFont();
            DisplayMenu.LoadMenu();
            Progress.LoadProgress();
        }

        /** metoda odświeżania wartości obliczeń*/
        public override void SetValue(Frames frames) 
        {
            if (displaying != 0)
                level.UpdateLevel(this);
        }

        /** metoda ustawiania elementów w oknie po odświeżeniu danych*/
        public override void Draw(Frames frames) 
        {   
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