using SFML.Window;

namespace QuadTreeCollisions.Core.Listeners
{
    public class MouseMovedEventListener
    {
        public MouseMovedEventListener()
        {
            Registry.Instance.mouseMovedEventListeners.Add(this);
        }

        ~MouseMovedEventListener()
        {
            Registry.Instance.mouseMovedEventListeners.Remove(this);
        }

        public virtual void MouseMoved(MouseMoveEventArgs e) { }
    }
}
