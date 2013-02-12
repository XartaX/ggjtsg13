using System;
using WindowsGame1.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class CapCameraPosition
    {
       public void CapCameraPosition
           (
           ref Camera camera,
           ref int worldWidth,
           ref int worldHeight,
           int Width,
           int Height
           )
        {//graphics.GraphicsDevice.Viewport.Width
            //graphics.GraphicsDevice.Viewport.Height
            Vector2 cameraPosition = camera.Position;
            if (cameraPosition.X > worldWidth - Width)
            {
                cameraPosition.X = worldWidth - Width;
            }
            if (cameraPosition.X < 0)
            {
                cameraPosition.X = 0;
            }
            if (cameraPosition.Y > worldHeight - Height)
            {
                cameraPosition.Y = worldHeight - Height;
            }
            if (cameraPosition.Y < 0)
            {
                cameraPosition.Y = 0;
            }
            camera.Position = cameraPosition;
        }

    }
}
