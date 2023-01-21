using SFML.Window;

namespace QuadTreeCollisions.Core.Entities
{
    public class MouseMovedEventListener
    {
        public MouseMovedEventListener()
        {
            Registry.Instance.mouseMovedEventListeners.Add(this);
        }

        public virtual void MouseMoved(MouseMoveEventArgs e) { }
    }
}
