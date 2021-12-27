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
    {
        public bool in_game = false;
        public uint displaying = 0;
        public const int FPS = 60;
        public const float REFRESH_RATE = 1f / FPS;
        private protected Level level;

        public RenderWindow Window 
        {
            get;
            protected set;
        }

        public Frames Frames
        {
            get;
            protected set;
        }

        public Color WindowClearColor
        {
            get;
            protected set;
        }

        protected Loop (uint width, uint height, string title, Color color)
        {
            this.WindowClearColor = color;
            this.Window = new RenderWindow(new VideoMode(width, height), title, Styles.Titlebar|Styles.Close);
            this.Frames = new Frames();
            Window.Closed += WindowClosed;
            Window.MouseButtonPressed += MousePressed;
            Window.MouseButtonReleased += MouseReleased;
            Window.MouseMoved += MouseMoved;
        }

        private void LoadLevel(uint levelNumber)
        {
            Level level = new Level(levelNumber);
            this.level = level;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Window.Close();
        }

        private void MouseReleased(object sender, EventArgs e)
        {
            if (displaying == 0)
            {
                DisplayMenu.CheckClick(ref displaying, this);
                if (displaying != 0)
                    this.LoadLevel(displaying);
            }
            else
            {
                level.CheckClickUp(ref displaying, this);
            }     
        }

        private void MousePressed(object sender, EventArgs e)
        {
            if (displaying != 0)
            {
                level.CheckClickDown(this);
            }
        }

        private void MouseMoved(object sender, EventArgs e)
        {
            if (displaying != 0)
            {
                level.CheckMove(this);
            }
        }

        public void Run()
        {
            Load();
            Initialize();

            float sinceRefresh = 0f;
            float previousTime = 0f;
            float delta = 0f;
            float totalTime = 0f;
            Clock clock = new Clock();

            while(Window.IsOpen)
            {
                Window.DispatchEvents();
                totalTime = clock.ElapsedTime.AsSeconds();
                delta = totalTime - previousTime;
                previousTime = totalTime;

                sinceRefresh += delta;
                //maksymalna możliwa ilość klatek zamiast 60 fps
                //if(sinceRefresh >= REFRESH_RATE)
                //{
                    Frames.SetValue(sinceRefresh, clock.ElapsedTime.AsSeconds());
                    sinceRefresh = 0f;
                    SetValue(Frames);

                    Window.Clear(WindowClearColor);
                    Draw(Frames);
                    Window.Display();
                //}
            }
        }

        public abstract void Load();
        public abstract void Initialize();
        public abstract void SetValue(Frames frames);
        public abstract void Draw(Frames frames);
    }
}
