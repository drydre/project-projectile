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
    class MouseEventsMenu 
    {

        public MouseEventsMenu() { }

        public static DisplayMenu.Area CheckArea(Loop loop)
        {
            if (Mouse.GetPosition(loop.Window).X >= C.LEFT_MARGIN && Mouse.GetPosition(loop.Window).X <= C.LEFT_MARGIN + C.MIN_X)
            {
                if (Mouse.GetPosition(loop.Window).Y >= C.TOP_MARGIN && Mouse.GetPosition(loop.Window).Y <= C.TOP_MARGIN + C.MIN_Y)
                    return DisplayMenu.Area.earth;
                else if (Mouse.GetPosition(loop.Window).Y >= C.TOP_MARGIN + C.MIN_Y + C.V_SPACING &&
                         Mouse.GetPosition(loop.Window).Y <= C.TOP_MARGIN + 2 * C.MIN_Y + C.V_SPACING)
                    return DisplayMenu.Area.moon;
            }
            else if (Mouse.GetPosition(loop.Window).X >= C.LEFT_MARGIN + C.MIN_X + C.H_SPACING &&
                     Mouse.GetPosition(loop.Window).X <= C.LEFT_MARGIN + 2 * C.MIN_X + C.H_SPACING)
            {
                if (Mouse.GetPosition(loop.Window).Y >= C.TOP_MARGIN && Mouse.GetPosition(loop.Window).Y <= C.TOP_MARGIN + C.MIN_Y)
                    return DisplayMenu.Area.mars;
                else if (Mouse.GetPosition(loop.Window).Y >= C.TOP_MARGIN + C.MIN_Y + C.V_SPACING &&
                         Mouse.GetPosition(loop.Window).Y <= C.TOP_MARGIN + 2 * C.MIN_Y + C.V_SPACING)
                    return DisplayMenu.Area.kepler;
            }
            
            if (Math.Pow(Mouse.GetPosition(loop.Window).X - (float)C.DEFAULT_WIDTH + (float)C.RIGHT_RESET_MARGIN, 2) / Math.Pow((float)C.RST_X_RAD, 2) +
                     Math.Pow(Mouse.GetPosition(loop.Window).Y - (float)C.TOP_RESET_MARGIN, 2) / Math.Pow((float)C.RST_Y_RAD, 2) <= 1)
                return DisplayMenu.Area.reset;

            return DisplayMenu.Area.none;
        }

    }
}
