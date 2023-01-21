using QuadTreeCollisions.Core.Entities;

namespace QuadTreeCollisions.Core
{
    public class InputTracker : KeyboardeventListener
    {
        public bool isKeyPressed(SFML.Window.Keyboard.Key key)
        {
            return keyToPressed.GetValueOrDefault(key, false);
        }

        public override void OnKeyPressed(SFML.Window.KeyEventArgs e) 
        {
            keyToPressed[e.Code] = true;
        }
        
        public override void OnKeyReleased(SFML.Window.KeyEventArgs e) 
        {
            keyToPressed[e.Code] = false;
        }

        private Dictionary<SFML.Window.Keyboard.Key, bool> keyToPressed = new Dictionary<SFML.Window.Keyboard.Key, bool>();
    }
}
