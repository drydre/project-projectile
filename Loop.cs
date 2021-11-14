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
        public const int FPS = 60;
        public const float REFRESH_RATE = 1f / FPS;
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
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Window.Close();
        }

        public void Run()
        {
            Load();
            Initialize();

            float untilRefresh = 0f;
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

                untilRefresh += delta;

                if(untilRefresh >= REFRESH_RATE)
                {
                    Frames.SetValue(untilRefresh, clock.ElapsedTime.AsSeconds());
                    untilRefresh = 0f;
                    SetValue(Frames);

                    Window.Clear(WindowClearColor);
                    Draw(Frames);
                    Window.Display();
                }
            }
        }

        public abstract void Load();
        public abstract void Initialize();
        public abstract void SetValue(Frames frames);
        public abstract void Draw(Frames frames);
    }
}
