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


    public class Miniature
    {
        Texture texture;
        Text text;
        Text cpltText;
        public RectangleShape rectShp;
        public Ellipse ellipse;
        public Sprite sprite;
        public bool active = false;
        public bool completed = false;
        public float x;
        public float y;

        public Miniature(string ellipsetext, float x, float y, Color brdcolor, Color fillcolor, float thickness = C.DEF_BRD_THICKNESS)
        {
           
            ellipse = new Ellipse(C.RST_X_RAD, C.RST_Y_RAD, x, y, brdcolor, fillcolor, Projectile.mainFont, C.RST_TEXT_SIZE, ellipsetext, DisplayMenu.DefTxtClr, thickness = C.DEF_BRD_THICKNESS);
        }

        public Miniature(string name, string path, Color brdcolor, Color textcolor, float x, float y, float thickness = C.DEF_BRD_THICKNESS)
        {
            this.x = x;
            this.y = y;
            this.LoadTexture(texture, path);
            this.LoadBorder(rectShp, thickness, brdcolor, x, y);
            this.LoadSprite(sprite, texture, x, y);
            this.LoadText(C.NAME_TEXT_SIZE, name, textcolor, texture);
            if (IsRect())
                this.LoadCpltText();
        }

        private void LoadTexture(Texture texture, string path)
    {
        this.texture = new Texture(path);
    }

        private void LoadBorder(RectangleShape rectShp, float thickness, Color color, float x, float y)
        {
            this.rectShp = new RectangleShape()
            {
                OutlineThickness = thickness,
                OutlineColor = color,
                FillColor = Color.Transparent,
                Size = (Vector2f)texture.Size,
                Position = new Vector2f(x, y)
            };
        }

        private void LoadSprite(Sprite sprite, Texture texture, float x, float y)
        {
            this.sprite = new Sprite(texture)
            {
                Position = new Vector2f(x, y)
            };
        }

        private void LoadText(uint textsize, string name, Color textcolor, Texture texture)
        {
            this.text = new Text(name, Projectile.mainFont, textsize);
            FloatRect textRect = text.GetLocalBounds();
            text.Origin = new Vector2f(textRect.Left + textRect.Width/2, textRect.Top + textRect.Height/2);
            text.Position = new Vector2f(x+ texture.Size.X / 2, y+25f);
            text.Color = textcolor;
        }

        private void LoadCpltText()
        {
            this.cpltText = new Text("COMPLETED", Projectile.mainFont, C.CPLT_TEXT_SIZE);
            FloatRect textRect = cpltText.GetLocalBounds();
            cpltText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            cpltText.Position = new Vector2f(x + texture.Size.X / 2, y + texture.Size.Y / 2);
            cpltText.Color = DisplayMenu.CpltBrdClr;
        }

        public void ChangeColor(Color color)
        {
            this.rectShp.OutlineColor = color;
        }

        public bool IsRect()
        {
            if (rectShp != null)
                return true;
            return false;
        }

        public void Draw(Loop loop)
        {

            if (this.IsRect())
            {
                loop.Window.Draw(this.sprite);
                loop.Window.Draw(this.rectShp);
                if (this.completed)
                    loop.Window.Draw(this.cpltText);
            }
            else
                ellipse.DrawEllipse(loop);
            if (this.text != null)
                loop.Window.Draw(this.text);
        }
        
}
}
