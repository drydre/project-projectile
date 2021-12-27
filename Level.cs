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
    {

        private static SoundBuffer stringBuffer = new SoundBuffer(C.STRING_SOUND_PATH);
        private static Sound stringSound = new Sound(stringBuffer);
        public static SoundBuffer clickBuffer = new SoundBuffer(C.CLICK_SOUND_PATH);
        public static Sound click = new Sound(clickBuffer);
        public static SoundBuffer shotBuffer = new SoundBuffer(C.SHOT_SOUND_PATH);
        public static Sound shot = new Sound(shotBuffer);
        private Texture bgTexture;
        private RectangleShape theoryShape;
        private Sprite bgSprite;
        private Text titleText;
        private Text theoryText;
        private Texture theoryTexture;
        private Sprite theorySprite;
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
        private bool stretching = false;
        public bool displayTheory = false;
        public static Stage stage;
        private Color fontColor;
        private float[] slingshotXTab = new float[] { C.SLINGSHOT_X_EARTH, C.SLINGSHOT_X_MARS, C.SLINGSHOT_X_MOON, C.SLINGSHOT_X_KEPLER };
        private float[] targetXTab = new float[] { C.TARGET_X_EARTH, C.TARGET_X_MARS, C.TARGET_X_MOON, C.TARGET_X_KEPLER };
        private float[] gTab = new float[] {C.G_EARTH, C.G_MARS, C.G_MOON, C.G_KEPLER};
        private string[] bgPathTab = new string[] { C.EARTH_PATH, C.MARS_PATH, C.MOON_PATH, C.KEPLER_PATH };
        private float[] baseAltTab = new float[] { C.EARTH_BASE_ALT, C.MARS_BASE_ALT, C.MOON_BASE_ALT, C.KEPLER_BASE_ALT };
        private string[] nameTab = new string[] { "Earth", "Mars", "Moon", "Kepler 22b" };
        private Color[] fontColorTab = new Color[] { DisplayLevel.earthFontClr, DisplayLevel.marsFontClr, DisplayLevel.moonFontClr, DisplayLevel.keplerFontClr };
        public enum Area
        {
            none,
            projectile,
            theory,
            leftButton,
            rightButton,
        }
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
        {
            this.levelNumber = levelNumber;
            this.baseAlt = baseAltTab[levelNumber - 1];
            this.name = nameTab[levelNumber - 1];
            this.g = gTab[levelNumber - 1];
            this.bgPath = bgPathTab[levelNumber - 1];
            this.slingshotX = slingshotXTab[levelNumber - 1];
            this.targetX = targetXTab[levelNumber - 1];
            this.fontColor = fontColorTab[levelNumber - 1];
            this.LoadStage();
            this.LoadTheory();
            this.LoadBackground();
            ellipse = new Ellipse(C.THEORY_X_RAD, C.THEORY_Y_RAD, C.LEFT_THEORY_BTN_MARGIN, C.TOP_THEORY_BTN_MARGIN, DisplayLevel.theoryBrdClr, DisplayLevel.theoryFillClr, Projectile.mainFont, C.RST_TEXT_SIZE, "THEORY", DisplayMenu.DefTxtClr, C.DEF_BRD_THICKNESS);
            this.LoadTitle();
        }

        public void LoadStage()
        {
            stage = new Stage( currentStage, this.slingshotX, this.targetX, g, baseAlt, fontColor);
        }

        public float GetSlingshotX()
        {
            return slingshotX;
        }

        private void LoadBackground()
        {
            this.bgTexture = new Texture(bgPath);
            this.bgSprite = new Sprite(bgTexture)
            {
                Position = new Vector2f(0, 0)
            };
        }

        private void LoadTitle()
        {
            this.titleText = new Text(name, Projectile.mainFont, C.TITLE_TEXT_SIZE);
            FloatRect textRect = titleText.GetLocalBounds();
            titleText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            titleText.Position = new Vector2f(C.DEFAULT_WIDTH / 2, C.TITLE_TOP_MARGIN);
            titleText.Color = fontColor;
        }

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
            string theory1 = "Following simplifications were assumed:\n" +
                             "1. The energy shown during aiming corresponds to an amount of kinetic\n energy the projectile gets while leaving the slingshot.\n" +
                             "2. The projectile is dimensionless and its position corresponds to the center\n of its texture.\n" +
                             "3. An influence of air resistance shall be ignored.\n\n" +
                             "Movement of the projectile is described by the following equations:\n";
            theoryText = new Text(theory1, Projectile.theoryFont, C.THEORY_TEXT_SIZE);
            FloatRect textRect = theoryText.GetLocalBounds();
            theoryText.Position = new Vector2f(theoryShape.Position.X+C.THEORY_LEFT_MARGIN, theoryShape.Position.Y+C.THEORY_TOP_MARGIN);
            theoryText.Color = Color.Black;
            theoryTexture = new Texture(C.THEORY_PATH);
            theorySprite = new Sprite(theoryTexture)
            {
                Position = new Vector2f(theoryText.Position.X, theoryText.Position.Y + textRect.Height + C.THEORY_TOP_MARGIN)
            };
        }

        public void CheckClickUp(ref uint displaying, Loop loop)
        {
            Area area = MouseEventsGame.CheckAreaUp(this, stage, loop);
            if (area != Area.none && area != Area.projectile)
            {
                click.Play();
            }

            if (area == Area.projectile && activity == Action.aiming)
            {
                shot.Play();
            }

            switch (area)
            {
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
        {
            Area area = MouseEventsGame.CheckAreaDown(stage, loop);
            if (area == Area.projectile && activity == Action.none)
            {
                activity = Action.aiming;
            }
        }

        public void CheckMove(Loop loop)
        {
            Area area = MouseEventsGame.CheckAreaUp(this, stage, loop);
            if (area == Area.projectile && activity == Action.aiming && (float)Math.Sqrt(Math.Pow(Mouse.GetPosition(loop.Window).X-mousePosMove.X, 2) + Math.Pow(Mouse.GetPosition(loop.Window).Y-mousePosMove.Y, 2)) > 10)
            {
                this.mousePosMove = Mouse.GetPosition(loop.Window);
                stringSound.Play();
            }
        }

        public void UpdateLevel(Loop loop)
        {
            if (MouseEventsGame.CheckArea(loop) == Area.theory)
                this.ellipse.ChangeFillColor(DisplayLevel.theoryFillClrActive);
            else
                this.ellipse.ChangeFillColor(DisplayLevel.theoryFillClr);

            if (stage.updatedResult != 0 && stage.stageNumber == currentStage)
            {
                if (stage.updatedResult == 1)
                {
                    if (stage.stageNumber == 3)
                        Progress.SaveProgress((short)levelNumber);
                    currentStage = stage.stageNumber + 1;
                }
                    
                activity = Level.Action.result;
            }
            else if (!(activity == Action.result && stage.updatedResult == 0))    
                stage.UpdateStage(ref activity, loop);
                
        }

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

        public void CheckStageStatus()
        {
            if (stage.success == true)
                currentStage++;
        }
    }
}
