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
    /**
     * Klasa stworzona jedynie na potrzeby diagnostyki,
     * w obecnej formie pozwala wyświetlać ilość klatek
     * na sekundę, czas liczony od uruchomienia programu
     * oraz dane autora.
     */
    public static class DisplayInfo
    {   
        /** czcionka informacji*/
        public static Font infoFont;
        
        /** metoda wczytująca czcionke*/
        public static void LoadFont()
        {
            infoFont = new Font(C.INFO_FONT_PATH);
        }

        /** metoda generująca dane do wyświetlenia
          * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
          * @param fontColor kolor czcionki danych
          */
        public static void DrawInfo(Loop loop, Color fontColor)
        {
            if (infoFont == null)
                return;
            string totalTimeString = loop.Frames.TotalTime.ToString("0.000");
            string deltaString = loop.Frames.Delta.ToString("0.000");
            float fps = 1f / loop.Frames.Delta;
            string fpsString = fps.ToString("0.000");
            /*Text txtTime = new Text(totalTimeString, infoFont, 16)
            {
                Position = new Vector2f(15f, 5f),
                FillColor = fontColor
            };*/

            /*Text txtFPS = new Text(fpsString, infoFont, 16)
            {
                Position = new Vector2f(15f, 25f),
                FillColor = fontColor
            };*/

            Text txtAuthor = new Text(C.AUTHOR_INFO, infoFont, 16);
            FloatRect txtRect = txtAuthor.GetLocalBounds();
            txtAuthor.Position = new Vector2f(C.DEFAULT_WIDTH - txtRect.Width - 5f, C.DEFAULT_HEIGHT - txtRect.Height - 8f);
            txtAuthor.FillColor = fontColor;
            //loop.Window.Draw(txtTime);
            //loop.Window.Draw(txtFPS);
            loop.Window.Draw(txtAuthor);
        }
    }


}
