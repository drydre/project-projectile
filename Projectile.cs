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
    {
        public const uint DEFAULT_WIDTH = 1280;
        public const uint DEFAULT_HEIGHT = 720;
        public const string DEFAULT_TITLE  = "Project: Projectile";
        public static Color Bgcolor = new Color(187, 247, 143, 255);
        public static Color MainFontColor = new Color(112, 73, 0, 255);
        public Projectile() : base(DEFAULT_WIDTH, DEFAULT_HEIGHT, DEFAULT_TITLE, Bgcolor) { }

        public override void Load() 
        {
            DisplayInfo.LoadFont();
            DisplayMenu.LoadFont();
            DisplayMenu.LoadMiniature();
        }
        public override void Initialize() { }
        public override void SetValue(Frames frames) { }
        public override void Draw(Frames frames) 
        {
            DisplayInfo.DrawInfo(this, Color.Red);
            DisplayMenu.DrawMenu(this, MainFontColor);
        }
    }
}
