using QuadTreeCollisions.Application.Entities;
using QuadTreeCollisions.Core;
using QuadTreeCollisions.Core.Entities;
using SFML.System;
using SFML.Window;

namespace QuadTreeCollisions.Application.Controllers
{
    public class CubeSpawnerController : MouseButtonPressedEventListener
    {
        public override void MouseButtonPressed(MouseButtonEventArgs e) 
        {
            /*Console.WriteLine($"{Registry.Instance.mouse.lastPosition.X}:{Registry.Instance.mouse.lastPosition.Y}");

            for (int i = 0; i < 20; i++)
            {
                Cube cube = new Cube();
                cube.shape.Position = Registry.Instance.mouse.lastPosition;
            }*/
        }
    }
}
