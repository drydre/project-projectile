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
    {   // klasa tworząca elipsę z tekstem wewnątrz i obramowaniem
        private CircleShape ellipse;
        private Text text;
        private float radX;
        private float radY;
        private float x;
        private float y;
        private float brdThickness;
        private Color brdColor;
        private Color fillColor;
        private Color textColor;
        private Font font;
        private uint textSize;
        private string name;

        public Ellipse(float radX, float radY, float x, float y, Color brdColor, Color fillColor, Font font, uint textSize, string name, Color textColor, float brdThickness = C.DEF_BRD_THICKNESS)
        {   // konstruktor elipsy
            this.radX = radX;
            this.radY = radY;
            this.x = x;
            this.y = y;
            this.brdThickness = brdThickness;
            this.brdColor = brdColor;
            this.fillColor = fillColor;
            this.textColor = textColor;
            this.font = font;
            this.textSize = textSize;
            this.name = name;
            this.LoadEllipse();
            this.LoadEllipseText();
        }

        public void LoadEllipse()
        {   // wczytanie elipsy jako skalowanego okręgu
            this.ellipse = new CircleShape(radX)
            {
                Scale = new Vector2f(1, radY / radX),
                OutlineThickness = brdThickness,
                OutlineColor = brdColor,
                FillColor = fillColor,
                Origin = new Vector2f(radX, radX),
                Position = new Vector2f(x, y)
            };

        }
        public void LoadEllipseText()
        {   // wczytanie tekstu wewnątrz elipsy
            this.text = new Text(name, font, textSize);
            // FloatRect służy do wycentrowania tekstu dzięki znajomości wymiarów pola tekstowego
            FloatRect textRect = text.GetLocalBounds();
            text.Position = new Vector2f(x, y - 3f);
            text.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            text.FillColor = textColor;
        }

        public void ChangeEllipseColor(Color color)
        {   // metoda modyfikująca kolor obramowania elipsy
            this.ellipse.OutlineColor = color;
        }

        public void ChangeFillColor(Color color)
        {   // metoda modyfikująca kolor wypełnienia miniatury
            this.ellipse.FillColor = color;
        }

        public Color GetOutlineColor()
        {   // metoda zwracająca kolor obramowania elipsy
            return this.ellipse.OutlineColor;
        }

        public void DrawEllipse(Loop loop)
        {   // metoda wyświetlająca elipsę wraz z wpisanym tekstem
            loop.Window.Draw(this.ellipse);
            if (this.text != null)
                loop.Window.Draw(this.text);
        }
    }
}
