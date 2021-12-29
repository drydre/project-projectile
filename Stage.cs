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
    {   // klasa odpowiadająca za wygląd i charakterystykę etapu na danym poziomie
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
        private Color textColor;
        private Color shotTextcolor;
        public uint stageNumber;
        private int slingshotPlatformHeight;
        private int targetPlatformHeight;
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
        public float energy;
        public float velocity;
        public float maxEnergy;
        private string mainResultString;
        private string leftResultString;
        private string rightResultString;
        private bool clockRun = false;
        public bool success = false;
        public Physics physics = new Physics();
        private Random random = new Random();

        public Stage(uint stageNumber, float slingshotX, float targetX, float g, float baseAlt, Color StageFontColor, Color ShotFontColor)
        {   // konstruktor etapu losowo generuje zmienne wysokości platform, maksymalnej energii strzału etc.
            this.textColor = StageFontColor;
            this.shotTextcolor = ShotFontColor;
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
        {   // metoda wczytująca procę z pliku graficznego
            this.slingTexture = new Texture(C.SLINGSHOT_PATH);
            this.slingSprite = new Sprite(slingTexture)
            {   // punkt origin odnosi się do położenia pocisku (w środku cięciwy)
                // pomaga to na etapie późniejszych obliczeń jego położenia
                Origin = new Vector2f(slingTexture.Size.X / 2, 0),
                Position = new Vector2f(slingshotX, slingshotY)
            };
        }

        private void LoadTarget()
        {   // metoda wczytująca cel z pliku graficznego
            this.targetTexture = new Texture(C.TARGET_PATH);
            this.targetSprite = new Sprite(targetTexture)
            {
                Position = new Vector2f(targetX, targetY)
            };
        }

        private void LoadProjectile()
        {   // metoda wczytująca kształt pocisku
            this.projectileShape = new CircleShape(C.PROJECTILE_RAD)
            {
                OutlineThickness = C.PROJ_BRD_THICKNESS,
                OutlineColor = DisplayLevel.projBrdClr,
                FillColor = DisplayLevel.projFillClr,
                Origin = new Vector2f(C.PROJECTILE_RAD, C.PROJECTILE_RAD),
                Position = new Vector2f(slingshotX, slingshotY)
            };
        }

        private void LoadStageNumber()
        {   // metoda wczytująca napis pod nazwą poziomu
            // informujący o aktualnie rozgrywanym etapie
            this.stageText = new Text($"Stage {stageNumber}/3", Projectile.mainFont, C.STAGE_NUM_TEXT_SIZE);
            FloatRect textRect = stageText.GetLocalBounds();
            stageText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            stageText.Position = new Vector2f(C.DEFAULT_WIDTH / 2, C.STAGE_TOP_MARGIN);
            stageText.FillColor = textColor;
        }



        private void LoadPlatforms()
        {   // metoda wczytująca kształty platform, na których
            // w etapach 2 i 3 umieszczone są proca i cel
            Vector2f slingshotPlatformSize = new Vector2f(C.SLINGSHOT_PLATFORM_WIDTH, this.slingshotPlatformHeight);
            this.slingshotPlatformShape = new RectangleShape(slingshotPlatformSize)
            {   // środek platformy znajduje się pod środkiem obiektu, który podtrzymuje
                Origin = new Vector2f(C.SLINGSHOT_PLATFORM_WIDTH / 2, 0),
                Position = new Vector2f(slingshotX, slingshotY + slingTexture.Size.Y),
                FillColor = DisplayLevel.platformClr
            };
            Vector2f targetPlatformSize = new Vector2f(C.TARGET_PLATFORM_WIDTH, this.targetPlatformHeight);
            this.targetPlatformShape = new RectangleShape(targetPlatformSize)
            {
                Origin = new Vector2f(C.TARGET_PLATFORM_WIDTH / 2, 0),
                Position = new Vector2f(targetX + targetTexture.Size.X / 2, targetY + targetTexture.Size.Y),
                FillColor = DisplayLevel.platformClr
            };
        }

        private void LoadString()
        {   // metoda wczytująca kształt cięciwy procy
            // jako dwie jej połowy zaczepione do odpowiednich ramion
            Vector2f stringSize = new Vector2f(slingTexture.Size.X / 2 - C.STRING_DISTANCE, C.STRING_THICKNESS);
            this.stringLeftShape = new RectangleShape(stringSize)
            {
                Origin = new Vector2f(0, C.STRING_THICKNESS / 2),
                Position = new Vector2f(slingSprite.Position.X - slingTexture.Size.X / 2 + C.STRING_DISTANCE, slingSprite.Position.Y),
                FillColor = DisplayLevel.stringClr
            };
            this.stringRightShape = new RectangleShape(stringSize)
            {   // prawa połowa cięciwy jest odwrócona o 180 stopni względem ramienia
                Origin = new Vector2f(0, C.STRING_THICKNESS / 2),
                Rotation = 180,
                Position = new Vector2f(slingSprite.Position.X + slingTexture.Size.X / 2 - C.STRING_DISTANCE, slingSprite.Position.Y),
                FillColor = DisplayLevel.stringClr
            };
        }

        private void LoadConst()
        {   // metoda wczytująca stałe wylosowane wraz z utworzeniem obiektu etapu
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
            constText.FillColor = textColor;
        }

        private void UpdateFlightData()
        {   // metoda odświeżająca informacje o aktualnym położeniu pocisku
            // oraz prędkości, z jaką się porusza
            Vector2f coords = physics.UpdateCoords();
            velocity = physics.UpdateVelocity();
            string flightTextString = $"X position:\t{(coords.X-slingshotX)/ C.DISTANCE_SCALE} m\n" +
                                    $"Y position:\t{(C.DEFAULT_HEIGHT-coords.Y-baseAlt) / C.DISTANCE_SCALE} m\n" +
                                    $"Velocity:\t{velocity / C.DISTANCE_SCALE} m/s";
            this.flightText = new Text(flightTextString, Projectile.mainFont, C.FLIGHT_TEXT_SIZE);
            flightText.Position = new Vector2f(C.FLIGHT_TEXT_LEFT_MARGIN, C.FLIGHT_TEXT_TOP_MARGIN);
            flightText.FillColor = textColor;
        }

        private void UpdateShotData()
        {   // metoda odświeżająca informacje o kącie nachylenia
            // oraz energii kinetycznej, jaką otrzyma pocisk w momencie
            // puszczenia cięciwy procy w danym momencie
            string shotTextString = $"Energy:\t{energy} J\n" +
                                    $"Angle:\t{-angle*180 / (float)Math.PI}\u00B0\n";
            this.shotText = new Text(shotTextString, Projectile.mainFont, C.SHOT_TEXT_SIZE);
            shotText.Position = new Vector2f(C.SHOT_TEXT_LEFT_MARGIN, C.SHOT_TEXT_TOP_MARGIN);
            shotText.FillColor = shotTextcolor;
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
                alpha = (slingshotY - mousePos.Y <= 0) ? (float)-Math.PI / 2 : (float)Math.PI / 2;
                mousePos.X = (int)slingshotX;
            }
            // ograniczenie rozciągania cięciwy
            if (length > stringLimit)
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

        private void CreatePlatforms()
        {   // generowanie wysokości platform odpowiednich dla aktualnego etapu
            switch (stageNumber)
            {
                case 1:
                    this.slingshotPlatformHeight = 0;
                    this.targetPlatformHeight = 0;
                    break;
                case 2:
                    // -150 / -250 ogranicza wysokość platformy, aby nie nachodzić na obszary stałych
                    // ani przycisku wyświetlania teorii
                    // wysokość platformy zmieniana jest co 25px czyli co 0,5m przy programowym przeliczniku 50 px/m
                    this.targetPlatformHeight = (int)((random.Next((int)((C.DEFAULT_HEIGHT - baseAlt-150 - targetTexture.Size.Y) / C.DISTANCE_SCALE*2)) + 1) * C.DISTANCE_SCALE / 2);
                    this.slingshotPlatformHeight = 0;
                    break;
                case 3:
                    this.slingshotPlatformHeight = (int)((random.Next((int)((C.DEFAULT_HEIGHT - baseAlt - 250 - slingTexture.Size.Y) / C.DISTANCE_SCALE * 2)) + 1) * C.DISTANCE_SCALE / 2);
                    do
                    {
                        this.targetPlatformHeight = (int)((random.Next((int)((C.DEFAULT_HEIGHT - baseAlt - 150 - targetTexture.Size.Y) / C.DISTANCE_SCALE * 2)) + 1) * C.DISTANCE_SCALE / 2);
                    } while (this.slingshotPlatformHeight == this.targetPlatformHeight);
                    break;
                default:
                    break;
            } 
        }

        private void GenerateConsts()
        {   // metoda generująca masę pocisku oraz pseudo-stałą sprężystości gumy
            // (nie odpowiada jej ona dokładnie w fizycznym znaczeniu ale pomaga w dalszych obliczeniach)
            this.mass = (random.Next(9)) / 2f + 1;
            do
            {   // dla skrajnych warunków k=~550, dodane zostało 100 dla zapasu dla
                // różnorodności konfiguracji w skrajnych warunkach
                this.stringK = (random.Next(650000) + 1) / 1000f;
            } while (!CheckK());
        }

        private bool CheckK()
        {   // metoda sprawdzająca czy przy wylosowanym K oddanie strzału nie będzie trywialne
            // (przy kącie <15 st. względem linii proca-pocisk pocisk na pewno nie doleci)
            // oraz czy dla nachylenia co najmniej 45 st. względem linii proca-pocisk
            // trafienie jest możliwe (granice 15 i 45 st. możliwe do regulacji przez stałe)
            float diffAngle = (float)Math.Atan2((int)(targetPlatformHeight - slingshotPlatformHeight - slingTexture.Size.Y + targetTexture.Size.Y), (int)(targetX - slingshotX));
            float checkLowAngle = diffAngle + C.ANGLE_CUTOFF * (float)Math.PI / 180;
            float checkHighAngle = diffAngle + C.ANGLE_SAFE_K * (float)Math.PI / 180; 
            // podstawienie skrajnych danych do wzorów opisujących ruch poziomy
            float tLow = (targetX - slingshotX) / (float)(Math.Cos(checkLowAngle) * Math.Sqrt(stringK * Math.Pow(stringLimit, 2) / mass));
            float tSafe = (targetX - slingshotX) / (float)(Math.Cos(checkHighAngle) * Math.Sqrt(stringK * Math.Pow(stringLimit, 2) / mass));
            float yLow = (float)(Math.Sqrt(stringK * Math.Pow(stringLimit, 2) / mass) * Math.Sin(checkLowAngle) * tLow - C.DISTANCE_SCALE * g * Math.Pow(tLow, 2) / 2);
            float ySafe = (float)(Math.Sqrt(stringK * Math.Pow(stringLimit, 2) / mass) * Math.Sin(checkHighAngle) * tSafe - C.DISTANCE_SCALE * g * Math.Pow(tSafe, 2) / 2);
            if (yLow + slingshotPlatformHeight + (int)slingTexture.Size.Y >= (float)targetPlatformHeight || ySafe + slingshotPlatformHeight + (int)slingTexture.Size.Y < (float)(targetPlatformHeight + targetTexture.Size.Y ))
            {
                return false;
            }
            else
                return true;
        }

        private void ReturnString(float dt)
        {   // metoda obliczająca położenie cięciwy zaraz po oddaniu strzału
            // zanim pocisk zacznie opuszczać procę
            // poniższy wzór to przybliżona różniczka zmiany naciągu po czasie - równanie różnicowe
            // wyprowadzone z energii kx2^2/2 - kx1^2/2 = mv^2/2, gdzie v = ds/dt, a x1, x2 to naciągi w punkcie początkowym i aktualnym
            // widać tu uproszczenie kx^2/2 nie jest energią naciągu, ponieważ pominięta zostaje strata na uniesienie
            // pocisku z powrotem do pozycji wylotowej
            stretch = ((mass * str_prev - dt * (float)Math.Sqrt(stringK * (stringK * (float)Math.Pow(length, 2) * (float)Math.Pow(dt, 2) - mass * (float)Math.Pow(str_prev, 2) + mass * (float)Math.Pow(length, 2)))) / (stringK * (float)Math.Pow(dt, 2) + mass));
            if (stretch<0)
                stretch = 0;
            // przepisywanie poprzedniego naciągu do wykorzystania w równaniu różnicowym
            this.str_prev = stretch;
            Vector2f projectilePos = new Vector2f(slingshotX - (float)Math.Cos(angle) * stretch, slingshotY - (float)Math.Sin(angle) * stretch);
            // gdyby wyliczona kolejna pozycja procy i gumy wychodziła za punkt wylotu, ustaw pozycję na punkt wylotu
            if (projectilePos.X > slingshotX)
                projectilePos = new Vector2f(slingshotX, slingshotY);
            // obliczenia długości i kątów nachylenia poszczególnych połówek cięciwy procy
            float Y = slingshotY - projectilePos.Y;
            float leftX = slingshotX - slingTexture.Size.X / 2 + C.STRING_DISTANCE - projectilePos.X;
            float leftLength = (float)Math.Sqrt(Math.Pow(leftX, 2) + Math.Pow(Y, 2));
            float rightX = slingshotX + slingTexture.Size.X / 2 - C.STRING_DISTANCE - projectilePos.X;
            float rightLength = (float)Math.Sqrt(Math.Pow(rightX, 2) + Math.Pow(Y, 2));
            float leftAlpha = (Y < 0) ? -(float)(Math.Acos(leftX / leftLength) + Math.PI) : -(float)(Math.PI - Math.Acos(leftX / leftLength));
            float rightAlpha = -(float)(Math.PI - Math.Asin(Y / rightLength));
            float leftScale = leftLength / stringLeftShape.Size.X;
            float rightScale = rightLength / stringRightShape.Size.X;
            // skalowanie i obracanie połówek cięciwy wg powyższych wyliczeń
            stringLeftShape.Scale = new Vector2f(leftScale, 1);
            stringLeftShape.Rotation = leftAlpha * 180 / (float)Math.PI;
            stringRightShape.Scale = new Vector2f(rightScale, 1);
            stringRightShape.Rotation = rightAlpha * 180 / (float)Math.PI;
            projectileShape.Position = projectilePos;
        }

        private int CheckHit()
        {   // metoda sprawdzająca czy środek pocisku znajduje się w obszarze celu
            Vector2f projectilePosition = projectileShape.Position;
            Vector2f targetSize = (Vector2f)targetTexture.Size;
            if (projectilePosition.X >= targetSprite.Position.X && projectilePosition.X <= targetSprite.Position.X + targetSize.X &&
                 projectilePosition.Y >= targetSprite.Position.Y && projectilePosition.Y <= targetSprite.Position.Y + targetSize.Y)
            {   // przypadek trafienia - głośność zależna od energii strzału i planety
                Level.hitSound.Volume = 100 * (float)Math.Sqrt(g / C.G_KEPLER) * energy / maxEnergy;
                Level.hitSound.Play();
                return 1;
            }
            else if (projectilePosition.X > targetSprite.Position.X + targetSize.X || projectilePosition.Y > C.DEFAULT_HEIGHT - baseAlt)
            {   // przypadek wylecenia pocisku poza obszar, w którym możliwe jest trafienie
                if (projectilePosition.Y > C.DEFAULT_HEIGHT - baseAlt)
                {   // upadek na ziemię - odtworzenie dźwięku
                    Level.missSound.Volume = 100 * (float)Math.Sqrt(g / C.G_KEPLER) * energy / maxEnergy;
                    Level.missSound.Play();
                }
                return -1;
            }
            // pocisk ciągle w trakcie lotu
            else
                return 0;
        }

        private void LoadResult(bool result)
        {   // metoda wczytująca wynik strzału
            // jest rozbudowana ze względu na różne możliwe
            // do uzyskania rezultaty w zależności od numeru poziomu
            // i sukcesu bądź porażki
            Vector2f resultSize = new Vector2f(C.RESULT_WIDTH, C.RESULT_HEIGHT);
            Vector2f resultBtnSize = new Vector2f(C.RESULT_BTN_WIDTH, C.RESULT_BTN_HEIGHT);
            this.resultShape = new RectangleShape(resultSize)
            {   // kształt okna wyniku - wspólny dla wszystkich wyników
                OutlineThickness = C.RESULT_BRD_THICKNESS,
                OutlineColor = DisplayLevel.resultBrdClr,
                FillColor = DisplayLevel.resultFillClr,
                Position = new Vector2f((C.DEFAULT_WIDTH - C.RESULT_WIDTH) / 2, (C.DEFAULT_HEIGHT - C.RESULT_HEIGHT) / 2)
            };
            // ustawienie napisów odpowiednich dla uzyskanego wyniku
            SetResultStrings(result);
            // dla ostatniego etapu inny rozmiar czcionki ze względu na więcej znaków
            if (stageNumber == 3 && result)
                this.resultText = new Text(mainResultString, Projectile.mainFont, C.RESULT_LAST_TEXT_SIZE);
            else
                this.resultText = new Text(mainResultString, Projectile.mainFont, C.RESULT_TEXT_SIZE);
            // wycentrowanie napisu dzięki informacją uzyskaną z GetLocalBounds()
            FloatRect textRect = resultText.GetLocalBounds();
            resultText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            resultText.Position = new Vector2f(C.DEFAULT_WIDTH / 2, resultShape.Position.Y+C.RESULT_TEXT_TOP_MARGIN+textRect.Height/2);
            resultText.OutlineColor = Color.Black;
            resultText.OutlineThickness = 2f;
            // zmiana koloru napisu w zależności od wyniku
            if (result)
                resultText.FillColor = DisplayLevel.successClr;
            else
                resultText.FillColor = DisplayLevel.failureClr;
            // lewy przycisk - powrót do menu (MAIN MENU)
            this.resultLeftText = new Text(leftResultString, Projectile.mainFont, C.RESULT_BTN_TEXT_SIZE);
            FloatRect resultLeftTextRect = resultLeftText.GetLocalBounds();
            // dla ostatniego etapu położenie tekstu na środku (brak prawego tekstu w razie sukcesu)
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
            // dla ostatniego etapu położenie przycisku na środku (brak prawego przycisku w razie sukcesu)
            if (stageNumber == 3 && result)
                resultLeftBtnShape.Position = new Vector2f(resultShape.Position.X + resultShape.Size.X / 2 - resultLeftBtnShape.Size.X / 2, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y - C.RESULT_BTN_BOTTOM_MARGIN);
            else
                resultLeftBtnShape.Position = new Vector2f(resultShape.Position.X + C.RESULT_BTN_WIDTH_MARGIN, resultShape.Position.Y + resultShape.Size.Y - resultBtnSize.Y - C.RESULT_BTN_BOTTOM_MARGIN);
            // jeśli to nie ostatni etap zakończony sukcesem wyświetl przycisk ponownej próby (TRY AGAIN)
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
        {   // zwracanie koordynatów lewego przycisku w trakcie
            // wyświetlania wyniku (może być po lewej lub na środku)
            Vector2f btnCoords = new Vector2f(resultLeftBtnShape.Position.X, resultLeftBtnShape.Position.Y);
            return btnCoords;
        }
        public Vector2f GetRightButtonCoords()
        {   // zwracanie koordynatów prawego przycisku w trakcie
            // wyświetlania wyniku (może nie być zdefiniowany)
            Vector2f btnCoords = new Vector2f(resultRightBtnShape.Position.X, resultRightBtnShape.Position.Y);
            return btnCoords;
        }

        public Vector2f GetSlingshotCoords()
        {   // zwracanie koordynatów procy (program umożliwia zmianę jej położenia)
            Vector2f slingshotCoords = new Vector2f(slingshotX, slingshotY);
            return slingshotCoords;
        }

        public Vector2f GetProjectileCoords()
        {   // zwracanie koordynatów pocisku (zmienia położenie w trakcie celowania)
            Vector2f projectileCoords = new Vector2f(projectileShape.Position.X, projectileShape.Position.Y);
            return projectileCoords;
        }

        private void SetResultStrings(bool result)
        {   // metoda ustawiająca napisy w zależności od wyniku strzału
            if (result && stageNumber == 3)
            {   // sukces - ostatni etap
                this.mainResultString = C.SUCCESS_LAST_MAIN;
                this.leftResultString = C.SUCCESS_LEFT;
                this.rightResultString = C.SUCCESS_RIGHT;
            }
            else if (result)
            {   // sukces - pozostałe przypadki
                this.mainResultString = C.SUCCESS_MAIN;
                this.leftResultString = C.SUCCESS_LEFT;
                this.rightResultString = C.SUCCESS_RIGHT;
            }
            else
            {   // porażka
                this.mainResultString = C.FAILURE_MAIN;
                this.leftResultString = C.FAILURE_LEFT;
                this.rightResultString = C.FAILURE_RIGHT;
            }
        }

        public void UpdateStage(ref Level.Action activity, Loop loop)
        {   // odświeżanie danych kolejnego cyklu przez wykonanie obliczeń
            // charakterystycznych do procesu, który jest aktualnie wykonywany
            // w trakcie rozgrywki, metoda wykonuje różne obliczenia w zależności
            // od zmiennej activity
            if (activity == Level.Action.result)
            {   // w przypadku czynności wyświetlania rezultatu wczytaj
                // go z odpowiednimi danymi
                if (updatedResult == -1)
                    success = false;
                else if (updatedResult == 1)
                    success = true;
                LoadResult(success);
                updatedResult = 0;
            }
            else if (activity == Level.Action.flight)
            {   // w przypadku lotu pocisku sprawdzaj, czy dostępny
                // jest wynik strzału
                if (updatedResult == 0)
                {
                    updatedResult = CheckHit();
                    projectileShape.Position = physics.UpdateCoords();
                    UpdateFlightData();
                }
                else if (updatedResult != 0)
                {   // wyłączenie zegara stanowiącego licznik
                    // czasu w module fizyki
                    physics.clock.Dispose();   
                }
            }
            
            else if (activity == Level.Action.acc)
            {
                if (!clockRun)
                {   // zegar służy do obliczeń zachowania cięciwy
                    // i włączany jest tylko raz
                    clock = new Clock();
                    clockRun = true;
                    t_prev = clock.ElapsedTime.AsSeconds();
                }
                else if (projectileShape.Position != slingSprite.Position && clockRun)
                {   // odświeżenie długości powracającej cięciwy z przekazaniem
                    // przyrostu czasu do równiania różnicowego
                    ReturnString(clock.ElapsedTime.AsSeconds() - t_prev);
                    t_prev = clock.ElapsedTime.AsSeconds();     
                }
                else
                {   // wyłączenie zegara po powrocie cięciwy do puntu zerowego
                    // z przejściem do procesu lotu
                    clock.Dispose();
                    clockRun = false;
                    activity = Level.Action.flight;
                    // inicjalizacja fizyki z podaniem parametrów ustalonych przez gracza
                    // oraz stałych charakterystycznych dla obecnego poziomu
                    physics.InitShot(-angle, energy, mass, projectileShape.Position, g);
                }
            }
            else if (activity == Level.Action.aiming)
            {   // w procesie celowania odświeżaj położenie cięciwy
                // i pocisku dopóki wciśnięty jest lewy przycisk myszy
                // a także aktualizuj dane strzału wyświetlane obok procy
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    UpdateShotData();
                    UpdateString(loop);
                }
            }
  
        }

        public void Draw(Level.Action activity, Loop loop)
        {   // umieść w oknie elementy etapu
            if (activity != Level.Action.theory)
            {   // chowanie stałych i napisu podczas
                // wyświetlania planszy teorii
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
            // dane lotu wyświetlane są dopiero po rozpoczęciu lotu
            if ((activity == Level.Action.result || activity == Level.Action.flight) && flightText != null)
                loop.Window.Draw(this.flightText);
            // dane celowania wyświetlane są dopiero po rozpoczęciu celowania
            if (activity == Level.Action.aiming || activity == Level.Action.acc || activity == Level.Action.flight || activity == Level.Action.result)
                loop.Window.Draw(this.shotText);
            // okno wyniku
            if (activity == Level.Action.result && resultShape != null)
            {   // reakcja przycisków na obecność nad nimi kursora
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
                {   // prawy przycisk nie jest wyświetlany
                    // w przypadku sukcesu w ostatnim etapie
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
