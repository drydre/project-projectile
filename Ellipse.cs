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
    /** Klasa tworząca elipsę z tekstem wewnątrz i obramowaniem.*/
    public class Ellipse
    {   
        /** kształt okręgu, ktory nastepnie zostaje spłaszczony*/
        private CircleShape ellipse;
        /** tekst wewnątrz elipsy*/
        private Text text;
        /** ramię X elipsy*/
        private float radX;
        /** ramię Y elipsy*/
        private float radY;
        /** położenie x środka elipsy*/
        private float x;
        /** położenie y środka elipsy*/
        private float y;
        /** grubość obramowania elipsy*/
        private float brdThickness;
        /** kolor obramowania elipsy*/
        private Color brdColor;
        /** kolor wypełnienia elipsy*/
        private Color fillColor;
        /** kolor tekstu w elipsie*/
        private Color textColor;
        /** czcionka tekstu w elipsie*/
        private Font font;
        /** rozmiar czcionki tekstu w elipsie*/
        private uint textSize;
        /** tekst wewnątrz elipsy*/
        private string name;

        /** 
         * Konstruktor kształtu elipsy
         * @param radX ramię X elipsy
         * @param radY ramię Y elipsy
         * @param x położenie x środka elipsy
         * @param y połozenie y środka elipsy
         * @param brdColor kolor obramowania elipsy
         * @param fillColor kolor wypełnienia elipsy
         * @param font czcionka tekstu w elipsie
         * @param textSize rozmiar czcionki tekstu w elipsie
         * @param name tekst wewnątrz elipsy
         * @param textColor kolor tekstu w elipsie
         * @param brdThickness grubość obramowania elipsy
         */
        public Ellipse(float radX, float radY, float x, float y, Color brdColor, Color fillColor, Font font, uint textSize, string name, Color textColor, float brdThickness = C.DEF_BRD_THICKNESS)
        {   
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

        /** metoda wczytująca elipsę jako skalowany okrąg*/
        public void LoadEllipse()
        {   
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

        /** metoda wczytująca tekst wewnątrz elipsy*/
        public void LoadEllipseText()
        {   
            this.text = new Text(name, font, textSize);
            // FloatRect służy do wycentrowania tekstu dzięki znajomości wymiarów pola tekstowego
            FloatRect textRect = text.GetLocalBounds();
            text.Position = new Vector2f(x, y - 3f);
            text.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            text.FillColor = textColor;
        }

        /**
         * metoda modyfikująca kolor obramowania elipsy
         * @param color kolor obramowania elipsy
         */
        public void ChangeEllipseColor(Color color)
        {   
            this.ellipse.OutlineColor = color;
        }

        /**
         * metoda modyfikująca kolor wypełnienia miniatury
         * @param color kolor wypełnienia elipsy
         */
        public void ChangeFillColor(Color color)
        {   
            this.ellipse.FillColor = color;
        }

        /** metoda zwracająca kolor obramowania elipsy*/
        public Color GetOutlineColor()
        {   
            return this.ellipse.OutlineColor;
        }

        /**
         * metoda wyświetlająca elipsę wraz z wpisanym tekstem
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        public void DrawEllipse(Loop loop)
        {   
            loop.Window.Draw(this.ellipse);
            if (this.text != null)
                loop.Window.Draw(this.text);
        }
    }
}
