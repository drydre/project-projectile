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
     * klasa abstrakcyjna pętli programu obsługującej pełny cykl odświeżania
     * obliczeń i wyświetlania wyników i elementów graficznych a także zdarzeń
     */
    public abstract class Loop
    {   
        /** 
         * wyświetlana zawartość ekranu
         * 0 - menu, 1 - Earth, 2 - Mars, 3 - Moon, 4 - Kepler
         */
        public uint displaying = 0;
        /** poziom opisywany metodą LoadLevel*/
        private protected Level level;

        /** pole okna rozgrywki*/
        public RenderWindow Window 
        {   
            get;
            protected set;
        }

        /** pole kontroli czasu*/
        public Frames Frames
        {   
            get;
            protected set;
        }

        /** kolor tła czystego okna RenderWindow*/
        public Color WindowClearColor
        {   
            get;
            protected set;
        }

        /** 
         * Konstruktor pętli programu
         * @param width szerokość okna
         * @param height wysokość okna
         * @param title tytuł okna
         * @param kolor tła
         */
        protected Loop (uint width, uint height, string title, Color color)
        {   
            this.WindowClearColor = color;
            this.Window = new RenderWindow(new VideoMode(width, height), title, Styles.Titlebar|Styles.Close);
            this.Frames = new Frames();
            // obsługa zdarzeń
            Window.Closed += WindowClosed;
            Window.MouseButtonPressed += MousePressed;
            Window.MouseButtonReleased += MouseReleased;
            Window.MouseMoved += MouseMoved;
        }

        /**
         * metoda tworzenia poziomu o danym numerze (1 - Earth, 2 - Mars, 3 - Moon, 4 - Kepler)
         * @param levelNumber numer poziomu
         */
        private void LoadLevel(uint levelNumber)
        {   
            Level level = new Level(levelNumber);
            this.level = level;
        }

        /** obsługa zdarzenia zamknięcia okna*/
        private void WindowClosed(object sender, EventArgs e)
        {   
            Window.Close();
        }

        /** obsługa zdarzenia puszczenia przycisku myszy*/
        private void MouseReleased(object sender, EventArgs e)
        {   
            // obsługa dla menu
            if (displaying == 0)
            {
                DisplayMenu.CheckClick(ref displaying, this);
                if (displaying != 0)
                    this.LoadLevel(displaying);
            }
            else // obsługa dla poziomu
            {
                level.CheckClickUp(ref displaying, this);
            }     
        }

        /** obsługa zdarzenia naciśnięcia przycisku myszy w grze*/
        private void MousePressed(object sender, EventArgs e)
        {   
            if (displaying != 0)
            {
                level.CheckClickDown(this);
            }
        }

        /** obsługa zdarzenia poruszenia kursorem w grze*/
        private void MouseMoved(object sender, EventArgs e)
        {   
            if (displaying != 0)
            {
                level.CheckMove(this);
            }
        }

        /** metoda realizująca cykle programu*/
        public void Run()
        {   
            // jednorazowa inicjalizacja danych
            Load();

            float sinceRefresh = 0f;
            float previousTime = 0f;
            float delta = 0f;
            float totalTime = 0f;
            Clock clock = new Clock();

            while(Window.IsOpen)
            {   // nieskończona pętla programu
                // restart zdarzeń
                Window.DispatchEvents();
                totalTime = clock.ElapsedTime.AsSeconds();
                delta = totalTime - previousTime;
                previousTime = totalTime;
                sinceRefresh += delta;
                //maksymalna możliwa ilość klatek zamiast 60 fps
                //if(sinceRefresh >= C.REFRESH_RATE)
                //{
                    Frames.SetValue(sinceRefresh, clock.ElapsedTime.AsSeconds());
                    sinceRefresh = 0f;
                    // odświeżenie danych przed kolejnym cyklem
                    SetValue(Frames);
                    // wyczyszczenie okna programu
                    Window.Clear(WindowClearColor);
                    // ponowne umieszczenie elementów w oknie po odświeżeniu danych
                    Draw(Frames);
                    // wyświetlenie elementów 
                    Window.Display();
                //}
            }
        }
        
        // metody abstrakcyjne uzupełnione w klasie dziedziczącej
        /** metoda jednorazowego wczytywania danych*/
        public abstract void Load();

        /** metoda odświeżania wartości obliczeń*/
        public abstract void SetValue(Frames frames);

        /** metoda wyświetlania danych w oknie*/
        public abstract void Draw(Frames frames);
    }
}
