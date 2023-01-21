namespace QuadTreeCollisions.Core.Entities
{
    public class KeyboardeventListener
    {
        public KeyboardeventListener()
        {
            Registry.Instance.keyboardEventListeners.Add(this);
        }

        ~KeyboardeventListener()
        {
            Registry.Instance.keyboardEventListeners.Remove(this);
        }

        public virtual void OnKeyPressed(SFML.Window.KeyEventArgs e) { }
        public virtual void OnKeyReleased(SFML.Window.KeyEventArgs e) { }
    }
}
