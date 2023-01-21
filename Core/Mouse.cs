using QuadTreeCollisions.Core.Entities;
using SFML.System;
using SFML.Window;

namespace QuadTreeCollisions.Core
{
    public class Mouse : MouseMovedEventListener
    {
        public Mouse()
        {
            lastPosition = new Vector2f();
        }
        
        public override void MouseMoved(MouseMoveEventArgs e) 
        {
            lastPosition = Registry.Instance.window.WINDOW.MapPixelToCoords(new Vector2i(e.X, e.Y));
        }

        public Vector2f lastPosition { get; private set; }
    }
}
