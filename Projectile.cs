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
        public const string DEFAULT_TITLE = "Project: Projectile";

        public Projectile() : base(DEFAULT_WIDTH, DEFAULT_HEIGHT, DEFAULT_TITLE, Color.Black) { }

        public override void Load() 
        {
            DisplayInfo.LoadFont();
        }
        public override void Initialize() { }
        public override void SetValue(Frames frames) { }
        public override void Draw(Frames frames) 
        {
            DisplayInfo.DrawInfo(this, Color.Red);
        }
    }
}
