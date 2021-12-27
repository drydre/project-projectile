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
    static class MouseEventsGame
    {
        public static Level.Area CheckAreaUp(Level level, Stage stage, Loop loop)

        {  
            if (level.activity == Level.Action.result)
                if (Mouse.GetPosition(loop.Window).Y >= stage.GetLeftButtonCoords().Y && Mouse.GetPosition(loop.Window).Y <= stage.GetLeftButtonCoords().Y+C.RESULT_BTN_HEIGHT)
                {
                    if (Mouse.GetPosition(loop.Window).X >= stage.GetLeftButtonCoords().X && Mouse.GetPosition(loop.Window).X <= stage.GetLeftButtonCoords().X + C.RESULT_BTN_WIDTH)
                        return Level.Area.leftButton;
                    else if (!(stage.success == true && stage.stageNumber == 3) && Mouse.GetPosition(loop.Window).X >= stage.GetRightButtonCoords().X && Mouse.GetPosition(loop.Window).X <= stage.GetRightButtonCoords().X + C.RESULT_BTN_WIDTH)
                        return Level.Area.rightButton;
                }
            if (Math.Pow(Mouse.GetPosition(loop.Window).X - (float)C.LEFT_THEORY_BTN_MARGIN, 2) / Math.Pow((float)C.THEORY_X_RAD, 2) +
                Math.Pow(Mouse.GetPosition(loop.Window).Y - (float)C.TOP_THEORY_BTN_MARGIN, 2) / Math.Pow((float)C.THEORY_Y_RAD, 2) <= 1)
                return Level.Area.theory;
            if (Math.Pow(Mouse.GetPosition(loop.Window).X - stage.GetProjectileCoords().X, 2) +
                Math.Pow(Mouse.GetPosition(loop.Window).Y - stage.GetProjectileCoords().Y, 2) <= (float)Math.Pow(C.PROJECTILE_RAD, 2))
                return Level.Area.projectile;
            else
                return Level.Area.none;
        }

        public static Level.Area CheckAreaDown(Stage stage, Loop loop)
        {
            if (Math.Pow(Mouse.GetPosition(loop.Window).X - stage.GetSlingshotCoords().X, 2) +
                Math.Pow(Mouse.GetPosition(loop.Window).Y - stage.GetSlingshotCoords().Y, 2) <= (float)Math.Pow(C.PROJECTILE_RAD, 2))
                return Level.Area.projectile;
            else
                return Level.Area.none;
        }

        public static Level.Area CheckArea(Loop loop, Stage stage)
        {
            if (Math.Pow(Mouse.GetPosition(loop.Window).X - (float)C.LEFT_THEORY_BTN_MARGIN, 2) / Math.Pow((float)C.THEORY_X_RAD, 2) +
                     Math.Pow(Mouse.GetPosition(loop.Window).Y - (float)C.TOP_THEORY_BTN_MARGIN, 2) / Math.Pow((float)C.THEORY_Y_RAD, 2) <= 1)
                return Level.Area.theory;
            if (Mouse.GetPosition(loop.Window).Y >= stage.GetLeftButtonCoords().Y && Mouse.GetPosition(loop.Window).Y <= stage.GetLeftButtonCoords().Y + C.RESULT_BTN_HEIGHT)
            {
                if (Mouse.GetPosition(loop.Window).X >= stage.GetLeftButtonCoords().X && Mouse.GetPosition(loop.Window).X <= stage.GetLeftButtonCoords().X + C.RESULT_BTN_WIDTH)
                    return Level.Area.leftButton;
                else if (!(stage.success == true && stage.stageNumber == 3) && Mouse.GetPosition(loop.Window).X >= stage.GetRightButtonCoords().X && Mouse.GetPosition(loop.Window).X <= stage.GetRightButtonCoords().X + C.RESULT_BTN_WIDTH)
                    return Level.Area.rightButton;
            }
                return Level.Area.none;
        }

        public static Level.Area CheckArea(Loop loop)
        {
            if (Math.Pow(Mouse.GetPosition(loop.Window).X - (float)C.LEFT_THEORY_BTN_MARGIN, 2) / Math.Pow((float)C.THEORY_X_RAD, 2) +
                     Math.Pow(Mouse.GetPosition(loop.Window).Y - (float)C.TOP_THEORY_BTN_MARGIN, 2) / Math.Pow((float)C.THEORY_Y_RAD, 2) <= 1)
                return Level.Area.theory;
            return Level.Area.none;
        }

    }
}
