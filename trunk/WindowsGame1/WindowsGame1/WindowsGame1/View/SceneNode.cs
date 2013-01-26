using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    public class SceneNode
    {
        private Texture2D texture;
        private Animation animation;
        private Vector2 worldPosition;
        Rectangle worldRect;
        public Vector2 Position
        {
            get { return worldPosition; }
            set { worldPosition = value; }
        }

        public SceneNode(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.worldPosition = position;
        }
        /// <summary>
        /// called by our camera class.
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="drawPosition"></param>
        public void Draw(SpriteBatch renderer, Vector2 drawPosition)
        {
            renderer.Draw(texture, drawPosition, Color.White);

        }
        public void Draw(SpriteBatch renderer, Vector2 drawPosition,int width, int height)
        {
            renderer.Draw(texture, drawPosition,new Rectangle((int)Position.X,(int)Position.Y,width,height), Color.White);

        }

    }
}
