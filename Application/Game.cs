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

            /*Rectangle rect = new Rectangle(new Vector2f(0, 0), new Vector2f(0, 0));
            foreach (Cube cube in cubes)
            {
                rect.Position = cube.shape.Position;
                rect.Dimensions = cube.shape.Size;
                IList<QuadTreeEntity> intersectedWith = tree.findIntersections(rect);
            }*/
        }

        public void Draw(RenderWindow window)
        {
            tree.Draw(window);
        }

        public override void MouseButtonPressed(MouseButtonEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2f dimensions = new Vector2f(8, 8);
                Cube myCube = new Cube();
                myCube.rectangle.Position = Registry.Instance.mouse.lastPosition - (dimensions / 2);
                myCube.rectangle.Dimensions = dimensions;
                cubes.Add(myCube);
            }
        }

        private IList<Cube> cubes = new List<Cube>(100);

        private WindowController? windowController;
        private CubeSpawnerController? cubeSpawnerController;
        private Wave? wave;
        private Quadtree? tree;
        private Clock titleUpdateTimer = new Clock();
    }
}
