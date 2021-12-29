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
     * Klasa odpowiadająca za wygląd i charakterystykę danego poziomu, wewnątrz niej
     * tworzony jest osobny obiekt Stage, który pozwala wyróżnić cechy poszczególnych etapów.
     */
    class Level
    {   
        // wszystkie dźwięki w grze są polami przypisanymi do obiektu level
        /** bufor dźwieku naciągania*/
        private static SoundBuffer stringBuffer = new SoundBuffer(C.STRING_SOUND_PATH);
        /** dźwięk naciągania*/
        private static Sound stringSound = new Sound(stringBuffer);
        /** bufor dźwięku kliknięcia*/
        public static SoundBuffer clickBuffer = new SoundBuffer(C.CLICK_SOUND_PATH);
        /** dźwięk kliknięcia*/
        public static Sound click = new Sound(clickBuffer);
        /** bufor dźwięku wystrzału*/
        public static SoundBuffer shotBuffer = new SoundBuffer(C.SHOT_SOUND_PATH);
        /** dźwięk wystrzału*/
        public static Sound shot = new Sound(shotBuffer);
        /** bufor dźwięku trafienia*/
        public static SoundBuffer hitBuffer = new SoundBuffer(C.HIT_SOUND_PATH);
        /** dźwięk trafienia*/
        public static Sound hitSound = new Sound(hitBuffer);
        /** bufor dźwięku uderzenia w ziemię*/
        public static SoundBuffer missBuffer = new SoundBuffer(C.MISS_SOUND_PATH);
        /** dźwięk uderzenia w ziemię*/
        public static Sound missSound = new Sound(missBuffer);
        /** tekstura tła*/
        private Texture bgTexture;
        /** tekstura teorii*/
        private Texture theoryTexture;
        /** kształt planszy teorii*/
        private RectangleShape theoryShape;
        /** sprite tła*/
        private Sprite bgSprite;
        /** sprite teorii*/
        private Sprite theorySprite;
        /** tekst nazwy poziomu*/
        private Text titleText;
        /** tekst teorii*/
        private Text theoryText;
        /** ścieżka dostępu do tła*/
        private string bgPath;
        /** nazwa poziomu*/
        private string name;
        /** aktualny etap*/
        private uint currentStage = 1;
        /** numer odpowiadający poziomowi*/
        private uint levelNumber;
        /** kształt przycisku teorii*/
        public Ellipse ellipse;
        /** polozenie kursora przy naciąganiu*/
        private Vector2i mousePosMove = new Vector2i(0,0);
        /** przyspieszenie grawitacyjne*/
        private float g;
        /** położenie środka procy w poziomie*/
        private float slingshotX;
        /** polożenie lewej strony celu w poziomie*/
        private float targetX;
        /** punkt zerowy powierzchni*/
        private float baseAlt;
        /** czy teoria jest wyświetlana*/
        public bool displayTheory = false;
        /** etap*/
        public static Stage stage;
        /** kolor czcionki głównej*/
        private Color fontColor;
        /** kolor czcionki dla informacji o strzale*/
        private Color shotFontColor;
        // ponieważ każdy poziom ma inną charakterystykę, do pól przypisywane są stałe z tablic
        // na podstawie numeru poziomu
        /** tablica położeń procy w poziomie*/
        private float[] slingshotXTab = new float[] { C.SLINGSHOT_X_EARTH, C.SLINGSHOT_X_MARS, C.SLINGSHOT_X_MOON, C.SLINGSHOT_X_KEPLER };
        /** tablica położeń celu w poziomie*/
        private float[] targetXTab = new float[] { C.TARGET_X_EARTH, C.TARGET_X_MARS, C.TARGET_X_MOON, C.TARGET_X_KEPLER };
        /** tablica przyspieszeń grawitacyjnych*/
        private float[] gTab = new float[] {C.G_EARTH, C.G_MARS, C.G_MOON, C.G_KEPLER};
        /** tablica ścieżek dostępu do tła*/
        private string[] bgPathTab = new string[] { C.EARTH_PATH, C.MARS_PATH, C.MOON_PATH, C.KEPLER_PATH };
        /** tablica punktów zerowych powierzchni*/
        private float[] baseAltTab = new float[] { C.EARTH_BASE_ALT, C.MARS_BASE_ALT, C.MOON_BASE_ALT, C.KEPLER_BASE_ALT };
        /** tablica nazw poziomów*/
        private string[] nameTab = new string[] { "Earth", "Mars", "Moon", "Kepler 22b" };
        /** tablica kolorów głównych czcionki*/
        private Color[] fontColorTab = new Color[] { DisplayLevel.earthFontClr, DisplayLevel.marsFontClr, DisplayLevel.moonFontClr, DisplayLevel.keplerFontClr };
        /** tablica kolorow czcionki dla informacji o strzale*/
        private Color[] shotFontColorTab = new Color[] { DisplayLevel.earthFontClr, DisplayLevel.marsFontClr, DisplayLevel.earthFontClr, DisplayLevel.earthFontClr };
        /** typ wyliczeniowy opisujący obszary w grze*/
        public enum Area
        {
            none,
            projectile,
            theory,
            leftButton,
            rightButton,
        }
        /** typ wyliczeniowy opisujący zdarzenia w rozgrywce*/
        public enum Action
        {
            none,
            aiming,
            acc,
            flight,
            theory,
            result
        }
        /** aktualne zdarzenie w grze*/
        public Action activity = Action.none;

        /** 
         * Konstruktor poziomu zawiera dostosowanie wysokości dźwięku
         * do warunków panujących na danym poziomie.
         * @param levelNumber numer odpowiadający poziomowi
         */
        public Level(uint levelNumber)
        {   
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

        /** metoda wczytująca etap*/
        private void LoadStage()
        {  
            stage = new Stage( currentStage, this.slingshotX, this.targetX, g, baseAlt, fontColor, shotFontColor);
        }

        /** metoda wczytująca przycisk wyświetlania teorii*/
        private void LoadEllipse()
        {   
            ellipse = new Ellipse(C.THEORY_X_RAD, C.THEORY_Y_RAD, C.LEFT_THEORY_BTN_MARGIN, C.TOP_THEORY_BTN_MARGIN, DisplayLevel.theoryBrdClr, DisplayLevel.theoryFillClr, Projectile.mainFont, C.RST_TEXT_SIZE, "THEORY", DisplayLevel.theoryClr, C.DEF_BRD_THICKNESS);
        }

        /** metoda wczytująca tło charakterystyczne dla danego poziomu*/
        private void LoadBackground()
        {   
            this.bgTexture = new Texture(bgPath);
            this.bgSprite = new Sprite(bgTexture)
            {
                Position = new Vector2f(0, 0)
            };
        }

        /** metoda wczytująca tytuł poziomu*/
        private void LoadTitle()
        {   
            this.titleText = new Text(name, Projectile.mainFont, C.TITLE_TEXT_SIZE);
            FloatRect textRect = titleText.GetLocalBounds();
            titleText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            titleText.Position = new Vector2f(C.DEFAULT_WIDTH / 2, C.TITLE_TOP_MARGIN);
            titleText.FillColor = fontColor;
        }

        /** metoda wczytująca zawartość planszy teorii*/
        private void LoadTheory()
        {   
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

        /**
         * metoda uruchamiana przy zdarzeniu zwolnienia przycisku myszy
         * @param displaying wyświetlana zawartość ekranu
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        public void CheckClickUp(ref uint displaying, Loop loop)
        {   
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

        /**
         * metoda uruchamiana przy zdarzeniu naciśnięcia przycisku myszy,
         * używana tylko w kontekście naciągania cięciwy procy
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        public void CheckClickDown(Loop loop)
        {   
            Area area = MouseEventsGame.CheckAreaDown(stage, loop);
            if (area == Area.projectile && activity == Action.none)
            {
                activity = Action.aiming;
            }
        }

        /**
         * metoda uruchamiana przy zdarzeniu poruszenia myszą
         * używana tylko w kontekście naciągania cięciwy procy
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        public void CheckMove(Loop loop)
        {   
            Area area = MouseEventsGame.CheckAreaUp(this, stage, loop);
            if (area == Area.projectile && activity == Action.aiming && (float)Math.Sqrt(Math.Pow(Mouse.GetPosition(loop.Window).X-mousePosMove.X, 2) + Math.Pow(Mouse.GetPosition(loop.Window).Y-mousePosMove.Y, 2)) > 14)
            {   // po rozciągnięciu cięciwy o wartość w warunku odtwarzane jest trzeszczenie gumy
                if (stringSound.Status != SoundStatus.Playing)
                    stringSound.Play();
                this.mousePosMove = Mouse.GetPosition(loop.Window);
                
            }
        }

        /**
         * metoda odświeżająca dane kolejnego cyklu przez wykonanie obliczeń
         * charakterystycznych do procesu, który jest aktualnie wykonywany
         * w trakcie rozgrywki
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        public void UpdateLevel(Loop loop)
        {  
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

        /**
         * metoda umieszczająca w oknie elementy poziomu 
         * @param loop przekazanie pętli programu pozwala odwoływać sie do RenderWindow
         */
        public void Draw(Loop loop)
        {   
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
