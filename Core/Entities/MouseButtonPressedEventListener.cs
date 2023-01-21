using SFML.Window;

namespace QuadTreeCollisions.Core.Entities
{
    public class MouseButtonPressedEventListener
    {
        public MouseButtonPressedEventListener()
        {
            Registry.Instance.mousePressedEventListeners.Add(this);
        }

        ~MouseButtonPressedEventListener()
        {
            Registry.Instance.mousePressedEventListeners.Add(this);
        }

        public virtual void MouseButtonReleased(MouseButtonEventArgs e) { }
        public virtual void MouseButtonPressed(MouseButtonEventArgs e) { }
    }
}
