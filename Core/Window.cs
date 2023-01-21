using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace QuadTreeCollisions.Core
{
    public class Window
    {
        public Window()
        {
            SIZE = new Vector2u(1600, 1000);
            WINDOW = new RenderWindow(new VideoMode(SIZE.X, SIZE.Y), "Foo");
            WINDOW.Size = SIZE;
        }

        public Vector2u SIZE { get; private set; }
        public RenderWindow WINDOW { get; private set; }
    }
}
