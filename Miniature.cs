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


    public class Miniature : DisplayMenu
    {
        Texture texture;
        Text text;
        public RectangleShape rectshp;
        public Sprite sprite;


        
        public Miniature(string name, string path, Color textcolor, float x, float y)
        {
            this.LoadTexture(texture, path);
            this.LoadBorder(rectshp, DEF_BRD_THICKNESS, DefBrdClr, x, y);
            this.LoadSprite(sprite, texture, x, y);
            this.LoadText(mainFont, x, y, NAME_TEXT_SIZE, name, DefTxtClr, texture);
        }

        public Miniature(string name, string path, Color brdcolor, Color textcolor, float x, float y, float thickness = DEF_BRD_THICKNESS)
        {
            this.LoadTexture(texture, path);
            this.LoadBorder(rectshp, thickness, brdcolor, x, y);
            this.LoadSprite(sprite, texture, x, y);
            this.LoadText(mainFont, x, y, NAME_TEXT_SIZE, name, textcolor, texture);
        }


        private void LoadTexture(Texture texture, string path)
    {
        this.texture = new Texture(path);
    }

        private void LoadBorder(RectangleShape rectshp, float thickness, Color color, float x, float y)
        {
            this.rectshp = new RectangleShape()
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

        private void LoadText(Font font, float x, float y, uint textsize, string name, Color textcolor, Texture texture)
        {
            this.text = new Text(name, mainFont, textsize);
            FloatRect textRect = text.GetLocalBounds();
            text.Origin = new Vector2f(textRect.Left + textRect.Width/2, textRect.Top + textRect.Height/2);
            text.Position = new Vector2f(x+ texture.Size.X / 2, y+25f);
            text.Color = textcolor;

        }

        public void ChangeColor(Color color)
        {
            this.rectshp.OutlineColor = color;
        }

        public void Draw(Loop loop)
        {
            loop.Window.Draw(this.sprite);
            loop.Window.Draw(this.rectshp);
            if (this.text != null)
                loop.Window.Draw(this.text);
        }

}
}
