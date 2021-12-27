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
    public class Ellipse
    {
        private CircleShape ellipse;
        private Text text;

        public Ellipse(float rad_x, float rad_y, float x, float y, Color brdcolor, Color fillcolor, Font font, uint textsize, string name, Color textcolor, float thickness = C.DEF_BRD_THICKNESS)
        {
            this.LoadEllipse(rad_x, rad_y, x, y, brdcolor, fillcolor, thickness);
            this.LoadEllipseText(font, rad_x, x, y, textsize, name, textcolor);
        }

        public void LoadEllipse(float rad_x, float rad_y, float x, float y, Color brdcolor, Color fillcolor, float thickness = C.DEF_BRD_THICKNESS)
        {
            this.ellipse = new CircleShape(rad_x);
            this.ellipse.Scale = new Vector2f(1, rad_y / rad_x);
            this.ellipse.OutlineThickness = thickness;
            this.ellipse.OutlineColor = brdcolor;
            this.ellipse.FillColor = fillcolor;
            this.ellipse.Origin = new Vector2f(rad_x, rad_x);
            this.ellipse.Position = new Vector2f(x, y);
        }
        public void LoadEllipseText(Font font, float rad_x, float x, float y, uint textsize, string name, Color textcolor)
        {
            this.text = new Text(name, Projectile.mainFont, textsize);
            FloatRect textRect = text.GetLocalBounds();
            text.Position = new Vector2f(x, y - 3f);
            text.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            text.Color = textcolor;
        }

        public void ChangeEllipseColor(Color color)
        {
            this.ellipse.OutlineColor = color;
        }

        public void ChangeFillColor(Color color)
        {
            this.ellipse.FillColor = color;
        }

        public Color GetOutlineColor()
        {
            return this.ellipse.OutlineColor;
        }

        public void DrawEllipse(Loop loop)
        {
            loop.Window.Draw(this.ellipse);
            if (this.text != null)
                loop.Window.Draw(this.text);
        }
    }
}
