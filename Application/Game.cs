using QuadTreeCollisions.Application.Controllers;
using QuadTreeCollisions.Application.Entities;
using QuadTreeCollisions.Core;
using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Interfaces;
using QuadTreeCollisions.Core.Listeners;
using QuadTreeCollisions.Core.Structures;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuadTreeCollisions.Core.Structures;
using QuadTreeCollisions.Core.Math;

namespace QuadTreeCollisions.Application
{
    public class Game : MouseButtonPressedEventListener, IUpdateable, IDrawable
    {
        public void Load()
        {
            windowController = new WindowController();
            cubeSpawnerController = new CubeSpawnerController();
            wave = new Wave();

            Rectangle treeRect = new Rectangle(new Vector2f(0, 0), (Vector2f)Registry.Instance.window.SIZE);
            tree = new QuadTree(treeRect, 2, 0);

            Registry.Instance.updateables.Add(this);
            Registry.Instance.drawables.Add(this);
        }


        public void Update(float deltaTimeSeconds)
        {
            if (titleUpdateTimer.ElapsedTime.AsSeconds() > 1)
            {
                Registry.Instance.window.WINDOW.SetTitle($"QuadTreeCollisions {(int)(1 / deltaTimeSeconds)} FPS {Registry.Instance.updateables.Count}");
                titleUpdateTimer.Restart();
            }
        }

        public void Draw(RenderWindow window)
        {
            //tree.Draw(window);
        }

        public override void MouseButtonPressed(MouseButtonEventArgs e)
        {
            Vector2f dimensions = new Vector2f(8, 8);

            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                Cube cube = new Cube();
                cube.rectangle.Dimensions = dimensions;
                cube.rectangle.Position = Registry.Instance.mouse.lastPosition - (dimensions / 2);
            }
            else if (e.Button == SFML.Window.Mouse.Button.Right)
            {
                for (int i = 0; i < 100; i++)
                {
                    Cube cube = new Cube();
                    cube.rectangle.Position = Registry.Instance.mouse.lastPosition - (dimensions / 2);
                    cube.rectangle.Position = new Vector2f(cube.rectangle.Position.X + random.Next(-100, 100), cube.rectangle.Position.Y + random.Next(-100, 100));
                    cube.rectangle.Dimensions = dimensions;
                }
            }
        }

        private Random random = new Random();

        private HashSet<WorldObject> destroyedCubes = new HashSet<WorldObject>();
        

        private WindowController? windowController;
        private CubeSpawnerController? cubeSpawnerController;
        private Wave? wave;
        private QuadTree? tree;
        private Clock titleUpdateTimer = new Clock();
    }
}
