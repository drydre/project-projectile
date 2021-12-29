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
    public static class DisplayLevel
    {   // klasa statyczna przechowująca definicje kolorów używanych w grze
        public static Color resultBrdClr = new Color(112, 73, 0, 127);
        public static Color resultFillClr = new Color(255, 255, 255, 127);
        public static Color theoryFillClr = new Color(255, 255, 255, 127);
        public static Color theoryFillClrActive = new Color(255, 255, 255, 180);
        public static Color theoryBrdClr = new Color(255, 255, 255, 255);
        public static Color theoryBtnClr = new Color(112, 73, 0, 255);
        public static Color theoryClr = new Color(92, 8, 83, 255);
        public static Color resultBtnClr = new Color(112, 73, 0, 255);
        public static Color resultBtnClrActive = new Color(166, 108, 0, 255);
        public static Color successClr = new Color(77, 181, 0, 255);
        public static Color failureClr = new Color(153, 5, 5, 255);
        public static Color marsFontClr = new Color(92, 8, 83, 255);
        public static Color earthFontClr = new Color(92, 8, 83, 255);
        public static Color moonFontClr = new Color(196, 196, 196, 255);
        public static Color keplerFontClr = new Color(196, 196, 196, 255);
        public static Color platformClr = new Color(61, 42, 33, 255);
        public static Color stringClr = new Color(186, 99, 160, 255);
        public static Color projFillClr = new Color(120, 120, 120, 255);
        public static Color projBrdClr = new Color(70, 70, 70, 255);
    }
}