using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Interfaces;

namespace QuadTreeCollisions.Core
{
    public class Registry
    {
        public static Registry Instance = new Registry();

        private Registry() { }

        public Window window { get; set; }
        public Mouse mouse { get; set; }
        public InputTracker input { get; set; }

        public List<IDrawable> drawables { get; private set; } = new List<IDrawable>();
        public List<IUpdateable> updateables { get; private set; } = new List<IUpdateable>();
        public List<KeyboardeventListener> keyboardEventListeners { get; private set; } = new List<KeyboardeventListener>();
        public List<MouseButtonPressedEventListener> mousePressedEventListeners { get; private set; } = new List<MouseButtonPressedEventListener>();
        public List<MouseMovedEventListener> mouseMovedEventListeners { get; private set;} = new List<MouseMovedEventListener> { };
    }
}
