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
        public Game()
        {

        }

        public void Load()
        {
            windowController = new WindowController();
            cubeSpawnerController = new CubeSpawnerController();
            wave = new Wave();

            Rectangle treeRect = new Rectangle(new Vector2f(0, 0), (Vector2f)Registry.Instance.window.SIZE);
            tree = new Quadtree(treeRect, 2);

            Registry.Instance.updateables.Add(this);
            Registry.Instance.drawables.Add(this);
        }

        private IList<Cube> cubes = new List<Cube>();
        private IList<QuadTreeEntity> quadTreeEntities = new List<QuadTreeEntity>(500);

        public void Update(float deltaTimeSeconds)
        {
            tree.clear();
            foreach (Cube cube in cubes)
            {
                tree.insert(new QuadTreeEntity(cube.shape.Position, cube.shape.Size, cube));
            }
        }

        public void Draw(RenderWindow window)
        {
            tree.Draw(window);
        }

        public override void MouseButtonPressed(MouseButtonEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2f dimensions = new Vector2f(5, 5);
                Cube myCube = new Cube();
                myCube.shape.Position = Registry.Instance.mouse.lastPosition - (dimensions / 2);
                cubes.Add(myCube);
            }
        }

        private void addEntity(Vector2f position, Vector2f dimensions)
        {
            Console.Write("Insert Main");
            bool result = tree.insert(new QuadTreeEntity(position, dimensions, new String("1")));
            if (!result) Console.WriteLine("Could not insert!");
        }

        private WindowController windowController;
        private CubeSpawnerController cubeSpawnerController;
        private Wave wave;
        private Quadtree tree;
    }
}
