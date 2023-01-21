using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Interfaces;
using SFML.Graphics;
using SFML.Window;

namespace QuadTreeCollisions.Core
{
    public class Engine
    {
        public Engine()
        {
            registry = Registry.Instance;
        }

        public void Load()
        {
            registry.window = new Window();
            registry.mouse = new Mouse();
            registry.input = new InputTracker();

            window = registry.window.WINDOW;
            window.KeyPressed += KeyPressed;
            window.KeyReleased += KeyReleased;
            window.MouseMoved += MouseMoved;
            window.MouseButtonPressed += MouseButtonPressed;
            window.MouseButtonReleased += MouseButtonReleased;

            window.SetFramerateLimit(240);

            background = new RectangleShape
            {
                FillColor = Color.Black,
                Size = (SFML.System.Vector2f)registry.window.SIZE
            };
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                
                Step();
                frameClock.Restart();
                
                Render();
                window.Display();
            }
        }

        private void Step()
        {
            float deltaTime = frameClock.ElapsedTime.AsSeconds();
            foreach (IUpdateable updateable in Registry.Instance.updateables)
                updateable.Update(deltaTime);
        }

        private void Render()
        {
            window.Draw(background);
            foreach (IDrawable drawable in Registry.Instance.drawables)
                drawable.Draw(window);
        }

        private void KeyPressed(object? sender, SFML.Window.KeyEventArgs e)
        {
            foreach (KeyboardeventListener listener in Registry.Instance.keyboardEventListeners)
                listener.OnKeyPressed(e);
        }

        private void KeyReleased(object? sender, SFML.Window.KeyEventArgs e)
        {
            foreach (KeyboardeventListener listener in Registry.Instance.keyboardEventListeners)
                listener.OnKeyReleased(e);
        }
        private void MouseMoved(object? sender, MouseMoveEventArgs e)
        {
            foreach (MouseMovedEventListener listener in Registry.Instance.mouseMovedEventListeners)
                listener.MouseMoved(e);
        }

        private void MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            foreach (MouseButtonPressedEventListener listener in Registry.Instance.mousePressedEventListeners)
                listener.MouseButtonReleased(e);
        }

        private void MouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            foreach (MouseButtonPressedEventListener listener in Registry.Instance.mousePressedEventListeners)
                listener.MouseButtonPressed(e);
        }

        private RectangleShape background;
        private Registry registry;
        private RenderWindow window;
        private SFML.System.Clock frameClock = new SFML.System.Clock();
    }
}
