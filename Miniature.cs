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

    /** Klasa opisująca miniaturę wyświetlaną w menu.*/
    public class Miniature
    {   
        /** tekstura miniatury*/
        private Texture texture;
        /** tekst opisujący miniaturę*/
        private Text text;
        /** tekst ukończenia poziomu*/
        private Text cpltText;
        /** sprite miniatury*/
        private Sprite sprite;
        /** położenie miniatury w poziomie*/
        private float x;
        /** położenie miniatury w pionie*/
        private float y;
        /** tekst opisujacy miniaturę*/
        private string name;
        /** ścieżka dostępu do miniatury*/
        private string path;
        /** grubość obramowania*/
        private float brdThickness;
        /** kolor obramowania*/
        private Color brdColor;
        /** kolor czcionki*/
        private Color fontColor;
        /** kształt podświetlający miniaturę*/
        public RectangleShape rectShp;
        /** kształt resetu*/
        public Ellipse ellipse;
        /** flaga aktywności*/
        public bool active = false;
        /** flaga ukończenia poziomu*/
        public bool completed = false;

        /**
         * Konstruktor dla przycisku reset.
         * @param ellipsetext tekt wyświetlany na przycisku
         * @param x położenie środka przycisku w poziomie
         * @param y położenie środka przycisku w pionie
         * @param brdcolor kolor obramowania przycisku
         * @param fillcolor kolor wypełnienia przycisku
         * @param thickness grubość obramowania przycisku
         */
        public Miniature(string ellipsetext, float x, float y, Color brdcolor, Color fillcolor, float thickness = C.DEF_BRD_THICKNESS)
        {   
            ellipse = new Ellipse(C.RST_X_RAD, C.RST_Y_RAD, x, y, brdcolor, fillcolor, Projectile.mainFont, C.RST_TEXT_SIZE, ellipsetext, DisplayMenu.defTxtClr);
        }

        /**
         * Konstruktor dla miniatury poziomu.
         * @param name nazwa poziomu na miniaturze
         * @param path ścieżka dostępu do miniatury
         * @param brdColor kolor obramowania przycisku
         * @param fontColor kolor tekstu na miniaturze
         * @param x położenie miniatury w poziomie
         * @param y położenie miniatury w pionie
         * @param thickness grubość obramowania miniatury
         */
        public Miniature(string name, string path, Color brdColor, Color fontColor, float x, float y, float thickness = C.DEF_BRD_THICKNESS)
        {
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

        /** metoda wczytania tekstury poziomu z podanej ścieżki*/
        private void LoadTexture()
        {   
            this.texture = new Texture(this.path);
        }

        /** metoda wczytująca obramowanie miniatury*/
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

        /** metoda wczytująca sprite'a na podstawie wcześniej wczytanej miniatury*/
        private void LoadSprite()
        {
            this.sprite = new Sprite(texture)
            {   // umieszczenie sprite'a w pozycji podanej przy konstruowaniu obiektu
                Position = new Vector2f(x, y)
            };
        }

        /** metoda wczytujaca tekst opisujący miniaturę poziomu*/
        private void LoadText()
        {
            this.text = new Text(name, Projectile.mainFont, C.NAME_TEXT_SIZE);
            // FloatRect służy do wycentrowania tekstu dzięki znajomości wymiarów pola tekstowego
            FloatRect textRect = text.GetLocalBounds();
            text.Origin = new Vector2f(textRect.Left + textRect.Width/2, textRect.Top + textRect.Height/2);
            text.Position = new Vector2f(x+ texture.Size.X / 2, y + C.MINIATURE_TITLE_TOP_MARGIN);
            text.FillColor = fontColor;
        }

        /** metoda wczytująca napis ukończenia poziomu*/
        private void LoadCpltText()
        {   
            this.cpltText = new Text("COMPLETED", Projectile.mainFont, C.CPLT_TEXT_SIZE);
            // FloatRect służy do wycentrowania tekstu dzięki znajomości wymiarów pola tekstowego
            FloatRect textRect = cpltText.GetLocalBounds();
            cpltText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            cpltText.Position = new Vector2f(x + texture.Size.X / 2, y + texture.Size.Y / 2);
            cpltText.FillColor = DisplayMenu.cpltBrdClr;
        }

        /**
         * metoda modyfikująca kolor obramowania miniatury
         * @param color kolor obramowania miniatury
         */
        public void ChangeColor(Color color)
        {   
            this.rectShp.OutlineColor = color;
        }

        /** metoda zwracająca true jeśli obiekt jest miniaturą poziomu*/
        public bool IsRect()
        {   
            if (rectShp != null)
                return true;
            return false;
        }

        /** 
         * metoda wyświetlająca elementy miniatury
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        public void Draw(Loop loop)
        {   
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
