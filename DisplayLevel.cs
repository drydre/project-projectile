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
    /** Klasa statyczna przechowująca definicje kolorów używanych w grze.*/
    public static class DisplayLevel
    {   
        /** kolor obramowania planszy wyniku*/
        public static Color resultBrdClr = new Color(112, 73, 0, 127);
        /** kolor planszy wyniku*/
        public static Color resultFillClr = new Color(255, 255, 255, 127);
        /** kolor planszy teorii*/
        public static Color theoryFillClr = new Color(255, 255, 255, 127);
        /** kolor aktywnego przycisku teorii*/
        public static Color theoryFillClrActive = new Color(255, 255, 255, 180);
        /** kolor obramowania planszy teorii*/
        public static Color theoryBrdClr = new Color(255, 255, 255, 255);
        /** kolor przycisku teorii*/
        public static Color theoryBtnClr = new Color(112, 73, 0, 255);
        /** kolor czcionki przycisku teorii*/
        public static Color theoryClr = new Color(92, 8, 83, 255);
        /** kolor przycisków planszy wyniku*/
        public static Color resultBtnClr = new Color(112, 73, 0, 255);
        /** kolor aktywnego przycisku planszy wyniku*/
        public static Color resultBtnClrActive = new Color(166, 108, 0, 255);
        /** kolor napisu wyniku - sukces*/
        public static Color successClr = new Color(77, 181, 0, 255);
        /** kolor napisu wyniku - porażka*/
        public static Color failureClr = new Color(153, 5, 5, 255);
        /** kolor czcionki na poziomie Mars*/
        public static Color marsFontClr = new Color(92, 8, 83, 255);
        /** kolor czcionki na poziomie Earth*/
        public static Color earthFontClr = new Color(92, 8, 83, 255);
        /** kolor czcionki na poziomie Moon*/
        public static Color moonFontClr = new Color(196, 196, 196, 255);
        /** kolor czcionki na poziomie Kepler*/
        public static Color keplerFontClr = new Color(196, 196, 196, 255);
        /** kolor platform*/
        public static Color platformClr = new Color(61, 42, 33, 255);
        /** kolor cięciwy*/
        public static Color stringClr = new Color(186, 99, 160, 255);
        /** kolor wypełnienia pocisku*/
        public static Color projFillClr = new Color(120, 120, 120, 255);
        /** kolor obramowania pocisku*/
        public static Color projBrdClr = new Color(70, 70, 70, 255);
    }
}