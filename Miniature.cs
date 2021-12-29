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
    {   // klasa opisująca miniaturę wyświetlaną w menu
        private Texture texture;
        private Text text;
        private Text cpltText;
        private Sprite sprite;
        private float x;
        private float y;
        private string name;
        private string path;
        private float brdThickness;
        private Color brdColor;
        private Color fontColor;
        public RectangleShape rectShp;
        public Ellipse ellipse;
        public bool active = false;
        public bool completed = false;
        

        public Miniature(string ellipsetext, float x, float y, Color brdcolor, Color fillcolor, float thickness = C.DEF_BRD_THICKNESS)
        {   // konstruktor dla przycisku reset
            ellipse = new Ellipse(C.RST_X_RAD, C.RST_Y_RAD, x, y, brdcolor, fillcolor, Projectile.mainFont, C.RST_TEXT_SIZE, ellipsetext, DisplayMenu.defTxtClr);
        }

        public Miniature(string name, string path, Color brdColor, Color fontColor, float x, float y, float thickness = C.DEF_BRD_THICKNESS)
        {   // konstruktor do miniatury poziomu
            this.name = name;
            this.path = path;
            this.x = x;
            this.y = y;
            this.brdThickness = thickness;
            this.brdColor = brdColor;
            this.fontColor = fontColor;
            this.LoadTexture();
            this.LoadBorder();
            this.LoadSprite();
            this.LoadText();
            if (IsRect())
                this.LoadCpltText();
        }

        private void LoadTexture()
        {   // wczytanie tekstury poziomu z podanej ścieżki
            this.texture = new Texture(this.path);
        }

        private void LoadBorder()
        {
            this.rectShp = new RectangleShape()
            {
                OutlineThickness = brdThickness,
                OutlineColor = brdColor,
                FillColor = Color.Transparent,
                Size = (Vector2f)texture.Size,
                Position = new Vector2f(x, y)
            };
        }

        private void LoadSprite()
        {   // wczytanie sprite'a na podstawie wcześniej wczytanej miniatury
            this.sprite = new Sprite(texture)
            {   // umieszczenie sprite'a w pozycji podanej przy konstruowaniu obiektu
                Position = new Vector2f(x, y)
            };
        }

        private void LoadText()
        {   // wczytanie tekstu opisującego miniaturę poziomu
            this.text = new Text(name, Projectile.mainFont, C.NAME_TEXT_SIZE);
            // FloatRect służy do wycentrowania tekstu dzięki znajomości wymiarów pola tekstowego
            FloatRect textRect = text.GetLocalBounds();
            text.Origin = new Vector2f(textRect.Left + textRect.Width/2, textRect.Top + textRect.Height/2);
            text.Position = new Vector2f(x+ texture.Size.X / 2, y + C.MINIATURE_TITLE_TOP_MARGIN);
            text.FillColor = fontColor;
        }

        private void LoadCpltText()
        {   // metoda wczytująca napis ukończenia poziomu
            this.cpltText = new Text("COMPLETED", Projectile.mainFont, C.CPLT_TEXT_SIZE);
            // FloatRect służy do wycentrowania tekstu dzięki znajomości wymiarów pola tekstowego
            FloatRect textRect = cpltText.GetLocalBounds();
            cpltText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            cpltText.Position = new Vector2f(x + texture.Size.X / 2, y + texture.Size.Y / 2);
            cpltText.FillColor = DisplayMenu.cpltBrdClr;
        }

        public void ChangeColor(Color color)
        {   // metoda modyfikująca kolor obramowania miniatury
            this.rectShp.OutlineColor = color;
        }

        public bool IsRect()
        {   // metoda zwracająca true jeśli obiekt jest miniaturą poziomu
            if (rectShp != null)
                return true;
            return false;
        }

        public void Draw(Loop loop)
        {   // metoda wyświetlająca elementy miniatury
            if (this.IsRect())
            {   // dla miniatury poziomu
                loop.Window.Draw(this.sprite);
                loop.Window.Draw(this.rectShp);
                if (this.completed)
                    loop.Window.Draw(this.cpltText);
            }
            else // dla przycisku reset
                ellipse.DrawEllipse(loop);
            if (this.text != null)
                loop.Window.Draw(this.text);
        }
        
}
}
