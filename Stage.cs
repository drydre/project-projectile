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
    class Stage
    {
        private Clock clock;
        private CircleShape projectileShape;
        private Texture slingTexture;
        private Sprite slingSprite;
        private Texture targetTexture;
        private Sprite targetSprite;
        private RectangleShape slingshotPlatformShape;
        private RectangleShape targetPlatformShape;
        private RectangleShape stringLeftShape;
        private RectangleShape stringRightShape;
        private RectangleShape resultShape;
        private RectangleShape resultRightBtnShape;
        private RectangleShape resultLeftBtnShape;
        private Text resultText;
        private Text resultRightText;
        private Text resultLeftText;
        private Text constText;
        private Text stageText;
        private Text shotText;
        private Text flightText;
        private Color textcolor;
        public uint stageNumber;
        private uint slingshotPlatformHeight;
        private uint targetPlatformHeight;
        public int updatedResult = 0;
        private float t_prev = -1;
        private float slingshotX;
        private float slingshotY;
        private float targetX;
        private float targetY;
        private float baseAlt;
        private float stringLimit;
        private float length;
        private float stretch;
        private float str_prev;
        private float angle;
        private float stringK;
        private float mass;
        private float g;
        private float energy;
        private float maxEnergy;
        private string mainResultString;
        private string leftResultString;
        private string rightResultString;
        private Vector2i startPos;
        private bool clockRun = false;
        public bool success = false;
        public Physics physics = new Physics();
        private Random random = new Random();

        public Stage(uint stageNumber, float slingshotX, float targetX, float g, float baseAlt, Color StageFontColor)
        {
            this.textcolor = StageFontColor;
            this.stageNumber = stageNumber;
            this.g = g;
            this.baseAlt = baseAlt;
            this.LoadSlingshot();
            this.LoadTarget();
            this.CreatePlatforms();
            this.slingshotX = slingshotX;
            this.slingshotY = C.DEFAULT_HEIGHT - baseAlt - slingTexture.Size.Y - slingshotPlatformHeight;
            this.targetX = targetX;
            this.targetY = C.DEFAULT_HEIGHT - baseAlt - targetTexture.Size.Y - targetPlatformHeight;
            this.LoadSlingshot();
            this.LoadTarget();
            this.LoadPlatforms();
            this.LoadStageNumber();
            this.LoadProjectile();
            this.LoadString();
            this.stringLimit = (slingshotX>baseAlt+slingTexture.Size.Y) ? slingTexture.Size.Y+baseAlt-5 : slingshotX-5;
            this.GenerateConsts();
            this.maxEnergy = physics.GetEnergy(stringK, stringLimit);
            this.LoadConst();
            this.LoadResult(false);
        }


        private void LoadSlingshot()
        {
            this.slingTexture = new Texture(C.SLINGSHOT_PATH);
            this.slingSprite = new Sprite(slingTexture)
            {
                Origin = new Vector2f(slingTexture.Size.X / 2, 0),
                Position = new Vector2f(slingshotX, slingshotY)
            };
        }

        private void LoadTarget()
        {
            this.targetTexture = new Texture(C.TARGET_PATH);
            this.targetSprite = new Sprite(targetTexture)
            {
                Position = new Vector2f(targetX, targetY)
            };
        }

        private void LoadConst()
        {
            string constTextString = $"X distance:\t{(targetX - slingshotX) / C.DISTANCE_SCALE} m\n" +        
                               $"Slingshot\'s altitude:\t{(C.DEFAULT_HEIGHT-slingshotY-baseAlt-slingTexture.Size.Y) / C.DISTANCE_SCALE} m\n" +
                               $"Target\'s altitude:\t{(C.DEFAULT_HEIGHT - targetY - baseAlt - targetTexture.Size.Y) / C.DISTANCE_SCALE} m\n" +
                               $"Slingshot\'s height:\t{slingTexture.Size.Y / C.DISTANCE_SCALE} m\n" +
                               $"Target\'s height:\t{targetTexture.Size.Y / C.DISTANCE_SCALE} m\n" +
                               $"Target\'s width:\t{targetTexture.Size.X / C.DISTANCE_SCALE} m\n" +
                               $"Projectile's mass:\t{mass} kg\n" +
                               $"Maximal energy:\t{maxEnergy} J\n" +
                               $"g:\t{g} m/s\u00B2";
            this.constText = new Text(constTextString, Projectile.mainFont, C.CONST_TEXT_SIZE);
            constText.Position = new Vector2f(C.CONST_TEXT_LEFT_MARGIN, C.CONST_TEXT_TOP_MARGIN);
            constText.Color = textcolor;
        }

        private void UpdateFlightData()
        {
            Vector2f coords = physics.UpdateCoords();
            float velocity = physics.UpdateVelocity();
            string flightTextString = $"X position:\t{(coords.X-slingshotX)/ C.DISTANCE_SCALE} m\n" +
                                    $"Y position:\t{(C.DEFAULT_HEIGHT-coords.Y-baseAlt) / C.DISTANCE_SCALE} m\n" +
                                    $"Velocity:\t{velocity / C.DISTANCE_SCALE} m/s";
            this.flightText = new Text(flightTextString, Projectile.mainFont, C.FLIGHT_TEXT_SIZE);
            flightText.Position = new Vector2f(C.FLIGHT_TEXT_LEFT_MARGIN, C.FLIGHT_TEXT_TOP_MARGIN);
            flightText.Color = textcolor;
        }

        private void UpdateShotData()
        {
            string shotTextString = $"Energy:\t{energy} J\n" +
                                    $"Angle:\t{-angle*180 / (float)Math.PI}\u00B0\n";
            this.shotText = new Text(shotTextString, Projectile.mainFont, C.SHOT_TEXT_SIZE);
            shotText.Position = new Vector2f(C.SHOT_TEXT_LEFT_MARGIN, C.SHOT_TEXT_TOP_MARGIN);
            shotText.Color = textcolor;
        }

        private void LoadStageNumber()
        {
            this.stageText = new Text($"Stage {stageNumber}/3", Projectile.mainFont, C.STAGE_NUM_TEXT_SIZE);
            FloatRect textRect = stageText.GetLocalBounds();
            stageText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            stageText.Position = new Vector2f(C.DEFAULT_WIDTH / 2, C.STAGE_TOP_MARGIN);
            stageText.Color = textcolor;
        }

        private void LoadProjectile()
        {
            this.projectileShape = new CircleShape(C.PROJECTILE_RAD)
            {
                OutlineThickness = C.PROJ_BRD_THICKNESS,
                OutlineColor = DisplayLevel.ProjBrdClr,
                FillColor = DisplayLevel.ProjFillClr,
                Origin = new Vector2f(C.PROJECTILE_RAD, C.PROJECTILE_RAD),
                Position = new Vector2f(slingshotX, slingshotY)
            };
        }

        private void LoadPlatforms()
        {
            Vector2f slingshotPlatformSize = new Vector2f(C.SLINGSHOT_PLATFORM_WIDTH, this.slingshotPlatformHeight);
            this.slingshotPlatformShape = new RectangleShape(slingshotPlatformSize)
            {
                Origin = new Vector2f(C.SLINGSHOT_PLATFORM_WIDTH / 2, 0),
                Position = new Vector2f(slingshotX, slingshotY+slingTexture.Size.Y),
                FillColor = DisplayLevel.PlatformClr
            };
            Vector2f targetPlatformSize = new Vector2f(C.TARGET_PLATFORM_WIDTH, this.targetPlatformHeight);
            this.targetPlatformShape = new RectangleShape(targetPlatformSize)
            {
                Origin = new Vector2f(C.TARGET_PLATFORM_WIDTH / 2, 0),
                Position = new Vector2f(targetX+targetTexture.Size.X/2, targetY+targetTexture.Size.Y),
                FillColor = DisplayLevel.PlatformClr
            };
        }

        private void LoadString()
        {
            Vector2f stringSize = new Vector2f(slingTexture.Size.X / 2 - C.STRING_DISTANCE, C.STRING_THICKNESS);
            this.stringLeftShape = new RectangleShape(stringSize)
            {
                Origin = new Vector2f(0, C.STRING_THICKNESS/2),
                Position = new Vector2f(slingSprite.Position.X-slingTexture.Size.X/2+C.STRING_DISTANCE, slingSprite.Position.Y),
                FillColor = DisplayLevel.StringClr
            };
            this.stringRightShape = new RectangleShape(stringSize)
            {
                Origin = new Vector2f(0, C.STRING_THICKNESS / 2),
                Rotation = 180,
                Position = new Vector2f(slingSprite.Position.X + slingTexture.Size.X / 2 - C.STRING_DISTANCE, slingSprite.Position.Y),
                FillColor = DisplayLevel.StringClr
            };
        }

        private void CreatePlatforms()
        {
            switch (stageNumber)
            {
                case 1:
                    this.slingshotPlatformHeight = 0;
                    this.targetPlatformHeight = 0;
                    break;
                case 2:
                    this.targetPlatformHeight = (uint)(random.Next((int)((C.DEFAULT_HEIGHT-baseAlt-100 - targetTexture.Size.Y)/C.DISTANCE_SCALE*2))*C.DISTANCE_SCALE/2);
                    this.slingshotPlatformHeight = 0;
                    break;
                case 3:
                    this.slingshotPlatformHeight = (uint)(random.Next((int)((C.DEFAULT_HEIGHT - baseAlt - 200 - slingTexture.Size.Y) / C.DISTANCE_SCALE * 2)) * C.DISTANCE_SCALE / 2);
                    this.targetPlatformHeight = (uint)(random.Next((int)((C.DEFAULT_HEIGHT - baseAlt - 100 - targetTexture.Size.Y) / C.DISTANCE_SCALE * 2)) * C.DISTANCE_SCALE / 2);
                    break;
                default:
                    break;
            } 
        }

        private void GenerateConsts()
        {
            int kFactor = 0;
            this.mass = (random.Next(9)) / 2f + 1;
            do
            {
                this.stringK = (random.Next(550000) + 1) / 1000f;
            } while (!CheckK());
        }

        private bool CheckK()
        {
            float diffAngle = (float)Math.Atan2((int)(targetPlatformHeight - slingshotPlatformHeight - slingTexture.Size.Y + targetTexture.Size.Y), (int)(targetX - slingshotX));
            float checkLowAngle = diffAngle + C.ANGLE_CUTOFF * (float)Math.PI / 180;
            float checkHighAngle = diffAngle + C.ANGLE_SAFE_K * (float)Math.PI / 180; 
            float t = (targetX - slingshotX) / (float)(Math.Cos(checkLowAngle) * Math.Sqrt(stringK * Math.Pow(stringLimit, 2) / mass));
            float y_low = (float)(Math.Sqrt(stringK * Math.Pow(stringLimit, 2) / mass) * Math.Sin(checkLowAngle) * t - C.DISTANCE_SCALE * g * Math.Pow(t, 2) / 2);
            float y_safe = (float)(Math.Sqrt(stringK * Math.Pow(stringLimit, 2) / mass) * Math.Sin(checkHighAngle) * t - C.DISTANCE_SCALE * g * Math.Pow(t, 2) / 2);
            if (y_low >= targetPlatformHeight || y_safe < targetPlatformHeight + targetTexture.Size.Y)
            {
                return false;
            }
            else
                return true;
        }

        public void UpdateString(Loop loop)
        {   // w klasie obliczane są długości i kąty nachylenia prostokątów tworzących cięciwę
            // tak, aby podążały za ruchami myszki
            Vector2i mousePos = Mouse.GetPosition(loop.Window);
            float length = (float)Math.Sqrt(Math.Pow(slingshotX - mousePos.X, 2) + Math.Pow(slingshotY - mousePos.Y, 2));
            float alpha = (float)Math.Asin((slingshotY - mousePos.Y) / length);
            // ograniczenie przed strzelaniem w przeciwnym kierunku (kąt maksymalny +/-90 st.)
            if (slingshotX - mousePos.X < 0)
            {
                alpha = ( slingshotY - mousePos.Y <= 0) ? (float)-Math.PI / 2 : (float)Math.PI / 2;
                mousePos.X = (int)slingshotX;
            }
            // ograniczenie rozciągania cięciwy
            if (length>stringLimit)
            {
                length = stringLimit;
                mousePos.X = (int)(slingshotX - Math.Cos(alpha) * length);
                mousePos.Y = (int)(slingshotY - Math.Sin(alpha) * length);
            }
            // zmienne przypisywane nadmiarowo służą do wyznaczenia ruchu cięciwy w trakcie powrotu
            this.length = length;
            this.str_prev = length;
            this.stretch = length;
            this.angle = alpha;
            this.startPos = mousePos;
            // wyznaczenie energii dla obecnego naciągnięcia
            this.energy = physics.GetEnergy(stringK, stretch);
            //obliczanie skalowania i nachylenia dla poszczególnych fragmentów cięciwy
            float Y = slingshotY - mousePos.Y;
            float leftX = slingshotX - slingTexture.Size.X / 2 + C.STRING_DISTANCE - mousePos.X;
            float leftLength = (float)Math.Sqrt(Math.Pow(leftX, 2) + Math.Pow(Y, 2));
            float rightX = slingshotX + slingTexture.Size.X / 2 - C.STRING_DISTANCE - mousePos.X;
            float rightLength = (float)Math.Sqrt(Math.Pow(rightX, 2) + Math.Pow(Y, 2));
            float leftAlpha = (Y < 0) ? -(float)(Math.Acos(leftX / leftLength) + Math.PI) : -(float)(Math.PI - Math.Acos(leftX / leftLength));
            float rightAlpha = -(float)(Math.PI - Math.Asin(Y / rightLength));
            float leftScale = leftLength / stringLeftShape.Size.X;
            float rightScale = rightLength / stringRightShape.Size.X;
            // przypisanie nowych skali i pozycji pocisku
            stringLeftShape.Scale = new Vector2f(leftScale, 1);
            stringLeftShape.Rotation = leftAlpha * 180 / (float)Math.PI;
            stringRightShape.Scale = new Vector2f(rightScale, 1);
            stringRightShape.Rotation = rightAlpha * 180 / (float)Math.PI;
            projectileShape.Position = (Vector2f)mousePos;
        }

        public void ReturnString(float dt)
        {
            stretch = ((mass * str_prev - dt * (float)Math.Sqrt(stringK * (stringK * (float)Math.Pow(length, 2) * (float)Math.Pow(dt, 2) - mass * (float)Math.Pow(str_prev, 2) + mass * (float)Math.Pow(length, 2)))) / (stringK * (float)Math.Pow(dt, 2) + mass));
            if (stretch<0)
                stretch = 0;
            this.str_prev = stretch;
            Vector2f projectilePos = new Vector2f(slingshotX - (float)Math.Cos(angle) * stretch, slingshotY - (float)Math.Sin(angle) * stretch);
            if (projectilePos.X > slingshotX)
                projectilePos = new Vector2f(slingshotX, slingshotY);
            float Y = slingshotY - projectilePos.Y;
            float leftX = slingshotX - slingTexture.Size.X / 2 + C.STRING_DISTANCE - projectilePos.X;
            float leftLength = (float)Math.Sqrt(Math.Pow(leftX, 2) + Math.Pow(Y, 2));
            float rightX = slingshotX + slingTexture.Size.X / 2 - C.STRING_DISTANCE - projectilePos.X;
            float rightLength = (float)Math.Sqrt(Math.Pow(rightX, 2) + Math.Pow(Y, 2));
            float leftAlpha = (Y < 0) ? -(float)(Math.Acos(leftX / leftLength) + Math.PI) : -(float)(Math.PI - Math.Acos(leftX / leftLength));
            float rightAlpha = -(float)(Math.PI - Math.Asin(Y / rightLength));
            float leftScale = leftLength / stringLeftShape.Size.X;
            float rightScale = rightLength / stringRightShape.Size.X;
            stringLeftShape.Scale = new Vector2f(leftScale, 1);
            stringLeftShape.Rotation = leftAlpha * 180 / (float)Math.PI;
            stringRightShape.Scale = new Vector2f(rightScale, 1);
            stringRightShape.Rotation = rightAlpha * 180 / (float)Math.PI;
            projectileShape.Position = projectilePos;
        }

        public int CheckHit()
        {
            Vector2f projectilePosition = projectileShape.Position;
            Vector2f targetSize = (Vector2f)targetTexture.Size;
            if (projectilePosition.X >= targetSprite.Position.X && projectilePosition.X <= targetSprite.Position.X + targetSize.X &&
                 projectilePosition.Y >= targetSprite.Position.Y && projectilePosition.Y <= targetSprite.Position.Y + targetSize.Y)
                return 1;
            else if (projectilePosition.X > targetSprite.Position.X + targetSize.X || projectilePosition.Y > C.DEFAULT_HEIGHT-baseAlt)
                return -1;
            else
                return 0;
        }

        public void LoadResult(bool result)
        {
            Vector2f resultSize = new Vector2f(C.RESULT_WIDTH, C.RESULT_HEIGHT);
            Vector2f resultBtnSize = new Vector2f(C.RESULT_BTN_WIDTH, C.RESULT_BTN_HEIGHT);
            this.resultShape = new RectangleShape(resultSize)
            {
                OutlineThickness = C.RESULT_BRD_THICKNESS,
                OutlineColor = DisplayLevel.resultBrdClr,
                FillColor = DisplayLevel.resultFillClr,
                Position = new Vector2f((C.DEFAULT_WIDTH - C.RESULT_WIDTH) / 2, (C.DEFAULT_HEIGHT - C.RESULT_HEIGHT) / 2)
            };
            SetResultStrings(result);
            this.resultText = new Text(mainResultString, Projectile.mainFont, C.RESULT_TEXT_SIZE);
            FloatRect textRect = resultText.GetLocalBounds();
            resultText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            resultText.Position = new Vector2f(C.DEFAULT_WIDTH / 2, resultShape.Position.Y+C.RESULT_TEXT_TOP_MARGIN+textRect.Height/2);
            if (result)
                resultText.Color = Color.Green;
            else
                resultText.Color = Color.Red;
            
            this.resultLeftText = new Text(leftResultString, Projectile.mainFont, C.RESULT_BTN_TEXT_SIZE);
            FloatRect resultLeftTextRect = resultLeftText.GetLocalBounds();
            if (stageNumber == 3 && result)
                resultLeftText.Position = new Vector2f(resultShape.Position.X + resultShape.Size.X/2, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y/2 - C.RESULT_BTN_BOTTOM_MARGIN);
            else    
                resultLeftText.Position = new Vector2f(resultShape.Position.X + resultBtnSize.X / 2 + C.RESULT_BTN_WIDTH_MARGIN, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y/2 - C.RESULT_BTN_BOTTOM_MARGIN);
            resultLeftText.Origin = new Vector2f(resultLeftTextRect.Left + resultLeftTextRect.Width / 2, resultLeftTextRect.Top + resultLeftTextRect.Height / 2);
            this.resultLeftBtnShape = new RectangleShape(resultBtnSize)
            {
                OutlineThickness = C.RESULT_BRD_THICKNESS,
                OutlineColor = DisplayLevel.resultBrdClr,
                FillColor = DisplayLevel.resultBtnClr
            };
                if (stageNumber == 3 && result)
                resultLeftBtnShape.Position = new Vector2f(resultShape.Position.X + resultShape.Size.X / 2 - resultLeftBtnShape.Size.X / 2, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y - C.RESULT_BTN_BOTTOM_MARGIN);
            else
                resultLeftBtnShape.Position = new Vector2f(resultShape.Position.X + C.RESULT_BTN_WIDTH_MARGIN, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y - C.RESULT_BTN_BOTTOM_MARGIN);
            
            if (!(stageNumber == 3 && result))
            {
                this.resultRightText = new Text(rightResultString, Projectile.mainFont, C.RESULT_BTN_TEXT_SIZE);
                FloatRect resultRightTextRect = resultRightText.GetLocalBounds();
                resultRightText.Position = new Vector2f(resultShape.Position.X + resultShape.Size.X - resultBtnSize.X/2 - C.RESULT_BTN_WIDTH_MARGIN, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y/2 - C.RESULT_BTN_BOTTOM_MARGIN);
                resultRightText.Origin = new Vector2f(resultRightTextRect.Left + resultRightTextRect.Width / 2, resultRightTextRect.Top + resultRightTextRect.Height / 2);
                this.resultRightBtnShape = new RectangleShape(resultBtnSize)
                {
                    OutlineThickness = C.RESULT_BRD_THICKNESS,
                    OutlineColor = DisplayLevel.resultBrdClr,
                    FillColor = DisplayLevel.resultBtnClr,
                    Position = new Vector2f(resultShape.Position.X + resultShape.Size.X - resultBtnSize.X - C.RESULT_BTN_WIDTH_MARGIN, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y - C.RESULT_BTN_BOTTOM_MARGIN)
                };
            }
        }

        public Vector2f GetLeftButtonCoords()
        {
            Vector2f btnCoords = new Vector2f(resultLeftBtnShape.Position.X, resultLeftBtnShape.Position.Y);
            return btnCoords;
        }
        public Vector2f GetRightButtonCoords()
        {
            Vector2f btnCoords = new Vector2f(resultRightBtnShape.Position.X, resultRightBtnShape.Position.Y);
            return btnCoords;
        }

        public Vector2f GetSlingshotCoords()
        {
            Vector2f slingshotCoords = new Vector2f(slingshotX, slingshotY);
            return slingshotCoords;
        }

        public Vector2f GetProjectileCoords()
        {
            Vector2f projectileCoords = new Vector2f(projectileShape.Position.X, projectileShape.Position.Y);
            return projectileCoords;
        }

        public void SetResultStrings(bool result)
        {
            if (result)
            {
                this.mainResultString = C.SUCCESS_MAIN;
                this.leftResultString = C.SUCCESS_LEFT;
                this.rightResultString = C.SUCCESS_RIGHT;
            }
            else
            {
                this.mainResultString = C.FAILURE_MAIN;
                this.leftResultString = C.FAILURE_LEFT;
                this.rightResultString = C.FAILURE_RIGHT;
            }
        }

        public void UpdateStage(ref Level.Action activity, Loop loop)
        {
            
            if (activity == Level.Action.result)
            {
                if (updatedResult == -1)
                    success = false;
                else if (updatedResult == 1)
                    success = true;
                LoadResult(success);
                updatedResult = 0;
            }
            else if (activity == Level.Action.flight)
            {
                
                if (updatedResult == 0)
                {
                    updatedResult = CheckHit();
                    projectileShape.Position = physics.UpdateCoords();
                    UpdateFlightData();
                }
                else if (updatedResult != 0)
                {
                    physics.clock.Dispose();
                    
                }
            }
            
            else if (activity == Level.Action.acc)
            {
                if (!clockRun)
                {
                    clock = new Clock();
                    clockRun = true;
                    t_prev = clock.ElapsedTime.AsSeconds();
                }
                else if (projectileShape.Position != slingSprite.Position && clockRun)
                {

                        ReturnString(clock.ElapsedTime.AsSeconds() - t_prev);
                        t_prev = clock.ElapsedTime.AsSeconds();     
                }
                else
                {
                    clock.Dispose();
                    clockRun = false;
                    activity = Level.Action.flight;
                    physics.InitShot(-angle, energy, mass, projectileShape.Position, g);
                }
                
            }
            else if (activity == Level.Action.aiming)
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    UpdateShotData();
                    UpdateString(loop);
                }
            }
  
        }

        public void Draw(Level.Action activity, Loop loop)
        {
            if (activity != Level.Action.theory)
            {
                loop.Window.Draw(this.constText);
                loop.Window.Draw(this.stageText);
            }
            loop.Window.Draw(this.slingSprite);
            loop.Window.Draw(this.targetSprite);
            loop.Window.Draw(this.stringLeftShape);
            loop.Window.Draw(this.stringRightShape);
            loop.Window.Draw(this.projectileShape);
            loop.Window.Draw(this.slingshotPlatformShape);
            loop.Window.Draw(this.targetPlatformShape);
            if (activity == Level.Action.flight && flightText != null)
                loop.Window.Draw(this.flightText);
            if (activity == Level.Action.aiming || activity == Level.Action.flight)
                loop.Window.Draw(this.shotText);
            if (activity == Level.Action.result && resultShape != null)
            {
                if (MouseEventsGame.CheckArea(loop, this) != Level.Area.leftButton && resultLeftBtnShape.OutlineColor == Color.White)
                {
                    resultLeftBtnShape.OutlineColor = DisplayLevel.resultBrdClr;
                    resultLeftBtnShape.FillColor = DisplayLevel.resultBtnClr;
                }
                else if (MouseEventsGame.CheckArea(loop, this) == Level.Area.leftButton)
                {
                    resultLeftBtnShape.OutlineColor = Color.White;
                    resultLeftBtnShape.FillColor = DisplayLevel.resultBtnClrActive;
                }
                if (MouseEventsGame.CheckArea(loop, this) != Level.Area.rightButton && resultRightBtnShape.OutlineColor == Color.White)
                {
                    resultRightBtnShape.OutlineColor = DisplayLevel.resultBrdClr;
                    resultRightBtnShape.FillColor = DisplayLevel.resultBtnClr;
                }
                else if (MouseEventsGame.CheckArea(loop, this) == Level.Area.rightButton)
                {
                    resultRightBtnShape.OutlineColor = Color.White;
                    resultRightBtnShape.FillColor = DisplayLevel.resultBtnClrActive;
                }


                loop.Window.Draw(this.resultShape);
                if (!(stageNumber == 3 && success))
                { 
                    loop.Window.Draw(this.resultRightBtnShape);
                    loop.Window.Draw(this.resultRightText);
                }
                loop.Window.Draw(this.resultLeftBtnShape);
                loop.Window.Draw(this.resultText);
                
                loop.Window.Draw(this.resultLeftText);
            }
        }
    }
}
