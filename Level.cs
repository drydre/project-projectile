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
    class Level
    {   // klasa odpowiadająca za wygląd i charakterystykę danego poziomu, wewnątrz niej
        // tworzony jest osobny obiekt Stage, który pozwala wyróżnić cechy poszczególnych etapów
        // wszystkie dźwięki w grze są polami przypisanymi do obiektu level
        private static SoundBuffer stringBuffer = new SoundBuffer(C.STRING_SOUND_PATH);
        private static Sound stringSound = new Sound(stringBuffer);
        public static SoundBuffer clickBuffer = new SoundBuffer(C.CLICK_SOUND_PATH);
        public static Sound click = new Sound(clickBuffer);
        public static SoundBuffer shotBuffer = new SoundBuffer(C.SHOT_SOUND_PATH);
        public static Sound shot = new Sound(shotBuffer);
        public static SoundBuffer hitBuffer = new SoundBuffer(C.HIT_SOUND_PATH);
        public static Sound hitSound = new Sound(hitBuffer);
        public static SoundBuffer missBuffer = new SoundBuffer(C.MISS_SOUND_PATH);
        public static Sound missSound = new Sound(missBuffer);
        private Texture bgTexture;
        private Texture theoryTexture;
        private RectangleShape theoryShape;
        private Sprite bgSprite;
        private Sprite theorySprite;
        private Text titleText;
        private Text theoryText;   
        private string bgPath;
        private string name;
        private uint currentStage = 1;
        private uint levelNumber;
        public Ellipse ellipse;
        private Vector2i mousePosMove = new Vector2i(0,0);
        private float g;
        private float slingshotX;
        private float targetX;
        private float baseAlt;
        public bool displayTheory = false;
        public static Stage stage;
        private Color fontColor;
        private Color shotFontColor;
        // ponieważ każdy poziom ma inną charakterystykę, do pól przypisywane są stałe z tablic
        // na podstawie numeru poziomu
        private float[] slingshotXTab = new float[] { C.SLINGSHOT_X_EARTH, C.SLINGSHOT_X_MARS, C.SLINGSHOT_X_MOON, C.SLINGSHOT_X_KEPLER };
        private float[] targetXTab = new float[] { C.TARGET_X_EARTH, C.TARGET_X_MARS, C.TARGET_X_MOON, C.TARGET_X_KEPLER };
        private float[] gTab = new float[] {C.G_EARTH, C.G_MARS, C.G_MOON, C.G_KEPLER};
        private string[] bgPathTab = new string[] { C.EARTH_PATH, C.MARS_PATH, C.MOON_PATH, C.KEPLER_PATH };
        private float[] baseAltTab = new float[] { C.EARTH_BASE_ALT, C.MARS_BASE_ALT, C.MOON_BASE_ALT, C.KEPLER_BASE_ALT };
        private string[] nameTab = new string[] { "Earth", "Mars", "Moon", "Kepler 22b" };
        private Color[] fontColorTab = new Color[] { DisplayLevel.earthFontClr, DisplayLevel.marsFontClr, DisplayLevel.moonFontClr, DisplayLevel.keplerFontClr };
        private Color[] shotFontColorTab = new Color[] { DisplayLevel.earthFontClr, DisplayLevel.marsFontClr, DisplayLevel.earthFontClr, DisplayLevel.earthFontClr };
        // typ wyliczeniowy opisujący obszary w grze
        public enum Area
        {
            none,
            projectile,
            theory,
            leftButton,
            rightButton,
        }
        // typ wyliczeniowy opisujący zdarzenia w rozgrywce
        public enum Action
        {
            none,
            aiming,
            acc,
            flight,
            theory,
            result
        }
        public Action activity = Action.none;

        public Level(uint levelNumber)
        {   // konstruktor poziomu zawiera dostosowanie wysokości dźwięku
            // do warunków panujących na danym poziomie
            this.levelNumber = levelNumber;
            this.baseAlt = baseAltTab[levelNumber - 1];
            this.name = nameTab[levelNumber - 1];
            this.g = gTab[levelNumber - 1];
            this.bgPath = bgPathTab[levelNumber - 1];
            this.slingshotX = slingshotXTab[levelNumber - 1];
            this.targetX = targetXTab[levelNumber - 1];
            this.fontColor = fontColorTab[levelNumber - 1];
            this.shotFontColor = shotFontColorTab[levelNumber - 1];
            stringSound.Pitch = (float)Math.Sqrt(this.g / C.G_EARTH);
            stringSound.Volume = 100*(float)Math.Sqrt(this.g / C.G_KEPLER);
            shot.Pitch = (float)Math.Sqrt(this.g / C.G_KEPLER);
            hitSound.Pitch = (float)Math.Sqrt(this.g / C.G_EARTH);
            missSound.Pitch = (float)Math.Sqrt(this.g / C.G_EARTH);
            this.LoadStage();
            this.LoadTheory();
            this.LoadBackground();
            this.LoadEllipse();
            this.LoadTitle();
        }

        private void LoadStage()
        {   // metoda wczytująca etap
            stage = new Stage( currentStage, this.slingshotX, this.targetX, g, baseAlt, fontColor, shotFontColor);
        }

        private void LoadEllipse()
        {   // metoda wczytująca przycisk wyświetlania teorii
            ellipse = new Ellipse(C.THEORY_X_RAD, C.THEORY_Y_RAD, C.LEFT_THEORY_BTN_MARGIN, C.TOP_THEORY_BTN_MARGIN, DisplayLevel.theoryBrdClr, DisplayLevel.theoryFillClr, Projectile.mainFont, C.RST_TEXT_SIZE, "THEORY", DisplayLevel.theoryClr, C.DEF_BRD_THICKNESS);
        }

        private void LoadBackground()
        {   // metoda wczytująca tło charakterystyczne dla danego poziomu
            this.bgTexture = new Texture(bgPath);
            this.bgSprite = new Sprite(bgTexture)
            {
                Position = new Vector2f(0, 0)
            };
        }

        private void LoadTitle()
        {   // metoda wczytująca tytuł poziomu
            this.titleText = new Text(name, Projectile.mainFont, C.TITLE_TEXT_SIZE);
            FloatRect textRect = titleText.GetLocalBounds();
            titleText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            titleText.Position = new Vector2f(C.DEFAULT_WIDTH / 2, C.TITLE_TOP_MARGIN);
            titleText.FillColor = fontColor;
        }

        private void LoadTheory()
        {   // metoda wczytująca zawartość planszy teorii
            this.theoryShape = new RectangleShape()
            {
                OutlineThickness = C.THEORY_BRD_THICKNESS,
                OutlineColor = DisplayLevel.theoryBrdClr,
                FillColor = DisplayLevel.theoryFillClr,
                Size = new Vector2f(C.THEORY_WIDTH, C.THEORY_HEIGHT),
                Position = new Vector2f((C.DEFAULT_WIDTH - C.THEORY_WIDTH) / 2, (C.DEFAULT_HEIGHT - C.THEORY_HEIGHT) / 2)
            };
            string theory1 = "The following assumptions can be made:\n" +
                             "1. The energy shown during aiming corresponds to the kinetic energy\n the projectile gains when leaving the slingshot.\n" +
                             "2. The projectile is dimensionless and its position corresponds to the center\n of its texture.\n" +
                             "3. The effect of air resistance is negligible.\n\n" +
                             "The movement of the projectile is described by the following equations:\n";
            theoryText = new Text(theory1, Projectile.theoryFont, C.THEORY_TEXT_SIZE);
            FloatRect textRect = theoryText.GetLocalBounds();
            theoryText.Position = new Vector2f(theoryShape.Position.X+C.THEORY_LEFT_MARGIN, theoryShape.Position.Y+C.THEORY_TOP_MARGIN);
            theoryText.FillColor = Color.Black;
            // równania zapisane w LATEX są elementem graficznym
            theoryTexture = new Texture(C.THEORY_PATH);
            theorySprite = new Sprite(theoryTexture)
            {
                Position = new Vector2f(theoryText.Position.X, theoryText.Position.Y + textRect.Height + C.THEORY_TOP_MARGIN)
            };
        }

        public void CheckClickUp(ref uint displaying, Loop loop)
        {   // metoda uruchamiana przy zdarzeniu zwolnienia przycisku myszy
            Area area = MouseEventsGame.CheckAreaUp(this, stage, loop);
            if (area != Area.none && area != Area.projectile)
            {
                click.Play();
            }

            if (area == Area.projectile && activity == Action.aiming)
            {   // głośność strzału jest zależna od naciągnięcia cięciwy
                // oraz od planety, na której rozgrywany jest poziom
                // (z uwagi na różnice sił naciągu pomiędzy poziomami)
                shot.Volume = 100*(float)Math.Sqrt(g/C.G_KEPLER) * stage.energy / stage.maxEnergy;
                shot.Play();
            }

            switch (area)
            {   // w zależności od obszaru, w którym nastąpiło zwolnienie
                // przycisku myszy, podejmowane są różne czynności
                case Area.projectile:
                    if (activity == Action.aiming)
                        activity = Action.acc;
                    break;
                case Area.theory:
                    if (activity == Action.theory || activity == Action.none)
                    {
                        this.displayTheory = !this.displayTheory;
                        if (displayTheory)
                            activity = Action.theory;
                        else
                            activity = Action.none;
                    }
                    break;
                case Area.leftButton:
                    if (activity == Action.result)
                    {
                        displaying = 0;
                        activity = Action.none;
                    }
                    break;
                case Area.rightButton:
                    if (activity == Action.result)
                    {
                        activity = Action.none;
                        this.LoadStage();
                    }
                    break;
                case Area.none:
                    break;
                default:
                    break;
            }
        }

        public void CheckClickDown(Loop loop)
        {   // metoda uruchamiana przy zdarzeniu naciśnięcia przycisku myszy
            // używana tylko w kontekście naciągania cięciwy procy
            Area area = MouseEventsGame.CheckAreaDown(stage, loop);
            if (area == Area.projectile && activity == Action.none)
            {
                activity = Action.aiming;
            }
        }

        public void CheckMove(Loop loop)
        {   // metoda uruchamiana przy zdarzeniu poruszenia myszą
            // używana tylko w kontekście naciągania cięciwy procy
            Area area = MouseEventsGame.CheckAreaUp(this, stage, loop);
            if (area == Area.projectile && activity == Action.aiming && (float)Math.Sqrt(Math.Pow(Mouse.GetPosition(loop.Window).X-mousePosMove.X, 2) + Math.Pow(Mouse.GetPosition(loop.Window).Y-mousePosMove.Y, 2)) > 14)
            {   // po rozciągnięciu cięciwy o wartość w warunku odtwarzane jest trzeszczenie gumy
                if (stringSound.Status != SoundStatus.Playing)
                    stringSound.Play();
                this.mousePosMove = Mouse.GetPosition(loop.Window);
                
            }
        }

        public void UpdateLevel(Loop loop)
        {   // odświeżanie danych kolejnego cyklu przez wykonanie obliczeń
            // charakterystycznych do procesu, który jest aktualnie wykonywany
            // w trakcie rozgrywki
            if (MouseEventsGame.CheckArea(loop) == Area.theory)
                this.ellipse.ChangeFillColor(DisplayLevel.theoryFillClrActive);
            else
                this.ellipse.ChangeFillColor(DisplayLevel.theoryFillClr);
            // warunek spełniony, gdy istnieje rezultat strzału
            if (stage.updatedResult != 0 && stage.stageNumber == currentStage)
            {
                if (stage.updatedResult == 1)
                {   // zwiększanie numeru etapu w razie sukcesu
                    // jeśli ostatni poziom, to zapisz postęp w pliku
                    if (stage.stageNumber == 3)
                        Progress.SaveProgress((short)levelNumber);
                    currentStage = stage.stageNumber + 1;
                }
                activity = Level.Action.result;
            }
            // jeśli brak jeszcze wyniku, to odśwież obliczenia charakterystyczne dla etapu
            else if (!(activity == Action.result && stage.updatedResult == 0))    
                stage.UpdateStage(ref activity, loop);
                
        }

        public void Draw(Loop loop)
        {   // umieść w oknie elementy poziomu
            loop.Window.Draw(this.bgSprite);
            loop.Window.Draw(this.titleText);
            ellipse.DrawEllipse(loop);
            if(stage != null)
                stage.Draw(activity, loop);
            if (displayTheory)
            {
                loop.Window.Draw(this.theoryShape);
                loop.Window.Draw(this.theoryText);
                loop.Window.Draw(this.theorySprite);
            }
        }
    }
}
