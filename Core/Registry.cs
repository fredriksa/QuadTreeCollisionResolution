using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Interfaces;
using QuadTreeCollisions.Core.Listeners;
using QuadTreeCollisions.Core.Physics.Colliders;
using QuadTreeCollisions.Core.Systems;

namespace QuadTreeCollisions.Core
{
    public class Registry
    {
        public static Registry Instance = new Registry();

        private Registry() { }

        public Window window { get; set; }
        public MouseTracker mouse { get; set; }
        public InputTracker input { get; set; }

        public List<Collider> colliders { get; set; } = new List<Collider>();
        public List<EngineSystem> systems { get; set; } = new List<EngineSystem>();
        public List<IDrawable> drawables { get; private set; } = new List<IDrawable>();
        public List<IUpdateable> updateables { get; private set; } = new List<IUpdateable>();
        public List<KeyboardeventListener> keyboardEventListeners { get; private set; } = new List<KeyboardeventListener>();
        public List<MouseButtonPressedEventListener> mousePressedEventListeners { get; private set; } = new List<MouseButtonPressedEventListener>();
        public List<MouseMovedEventListener> mouseMovedEventListeners { get; private set;} = new List<MouseMovedEventListener> { };
    }
}
