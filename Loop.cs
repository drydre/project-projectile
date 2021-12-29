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
    public abstract class Loop
    {   // klasa abstrakcyjna pętli programu obsługującej pełny cykl odświeżania
        // obliczeń i wyświetlania wyników i elementów graficznych a także zdarzeń
        public uint displaying = 0;
        private protected Level level;

        public RenderWindow Window 
        {   // pole okna rozgrywki
            get;
            protected set;
        }

        public Frames Frames
        {   // pole kontroli czasu
            get;
            protected set;
        }

        public Color WindowClearColor
        {   // kolor tła czystego okna RenderWindow
            get;
            protected set;
        }

        protected Loop (uint width, uint height, string title, Color color)
        {   // konstruktor pętli programu
            this.WindowClearColor = color;
            this.Window = new RenderWindow(new VideoMode(width, height), title, Styles.Titlebar|Styles.Close);
            this.Frames = new Frames();
            // obsługa zdarzeń
            Window.Closed += WindowClosed;
            Window.MouseButtonPressed += MousePressed;
            Window.MouseButtonReleased += MouseReleased;
            Window.MouseMoved += MouseMoved;
        }

        private void LoadLevel(uint levelNumber)
        {   // metoda tworzenia poziomu o danym numerze (1 - Earth, 2 - Mars, 3 - Moon, 4 - Kepler)
            Level level = new Level(levelNumber);
            this.level = level;
        }

        private void WindowClosed(object sender, EventArgs e)
        {   // obsługa zdarzenia zamknięcia okna
            Window.Close();
        }

        private void MouseReleased(object sender, EventArgs e)
        {   // obsługa zdarzenia puszczenia przycisku myszy
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

        private void MousePressed(object sender, EventArgs e)
        {   // obsługa zdarzenia naciśnięcia przycisku myszy w grze
            if (displaying != 0)
            {
                level.CheckClickDown(this);
            }
        }

        private void MouseMoved(object sender, EventArgs e)
        {   // obsługa zdarzenia poruszenia kursorem w grze
            if (displaying != 0)
            {
                level.CheckMove(this);
            }
        }

        public void Run()
        {   // metoda realizująca cykle programu
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
        public abstract void Load();
        public abstract void SetValue(Frames frames);
        public abstract void Draw(Frames frames);
    }
}
