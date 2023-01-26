using QuadTreeCollisions.Application.Controllers;
using QuadTreeCollisions.Application.Entities;
using QuadTreeCollisions.Core;
using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Interfaces;
using QuadTreeCollisions.Core.Structures;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            tree = new Quadtree(treeRect, 2, 0);

            Registry.Instance.updateables.Add(this);
            Registry.Instance.drawables.Add(this);
        }


        public void Update(float deltaTimeSeconds)
        {
            if (titleUpdateTimer.ElapsedTime.AsSeconds() > 1)
            {
                Registry.Instance.window.WINDOW.SetTitle($"QuadTreeCollisions {(int)(1 / deltaTimeSeconds)} FPS");
                titleUpdateTimer.Restart();
            }

            tree.clear();
            for (int i = 0; i < cubes.Count; i++)
            {
                Cube cube = cubes[i];
                tree.insert(cube);
            }

            Rectangle rect = new Rectangle(new Vector2f(0, 0), new Vector2f(0, 0));

            destroyedCubes.Clear();
            foreach (Cube cube in cubes)
            {
                IList<WorldObject> intersectedWith = tree.findIntersections(cube.rectangle);
                foreach (WorldObject worldObject in intersectedWith)
                    destroyedCubes.Add(worldObject);
            }

            foreach (Cube cube in destroyedCubes)
            {
                new Explosion(cube.rectangle.Position);
                cubes.Remove(cube);
                Registry.Instance.updateables.Remove(cube);
                Registry.Instance.drawables.Remove(cube);
            }
        }

        public void Draw(RenderWindow window)
        {
            tree.Draw(window);
        }

        public override void MouseButtonPressed(MouseButtonEventArgs e)
        {
            Vector2f dimensions = new Vector2f(8, 8);

            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                Cube cube = new Cube();
                cube.rectangle.Dimensions = dimensions;
                cube.rectangle.Position = Registry.Instance.mouse.lastPosition - (dimensions / 2);
                cubes.Add(cube);
            }
            else if (e.Button == SFML.Window.Mouse.Button.Right)
            {
                for (int i = 0; i < 10; i++)
                {
                    Cube cube = new Cube();
                    cube.rectangle.Position = Registry.Instance.mouse.lastPosition - (dimensions / 2);
                    cube.rectangle.Position = new Vector2f(cube.rectangle.Position.X + random.Next(-100, 100), cube.rectangle.Position.Y + random.Next(-100, 100));
                    cube.rectangle.Dimensions = dimensions;
                    cubes.Add(cube);
                }
            }
        }

        private Random random = new Random();

        private IList<Cube> cubes = new List<Cube>(100);
        private HashSet<WorldObject> destroyedCubes = new HashSet<WorldObject>();
        

        private WindowController? windowController;
        private CubeSpawnerController? cubeSpawnerController;
        private Wave? wave;
        private Quadtree? tree;
        private Clock titleUpdateTimer = new Clock();
    }
}
