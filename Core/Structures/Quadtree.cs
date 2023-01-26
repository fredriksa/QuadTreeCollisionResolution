using QuadTreeCollisions.Core.Interfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Structures
{
    public class QuadTreeEntity 
    {
        public QuadTreeEntity(Vector2f position, Vector2f dimensions, object obj)
        {
            rectangle = new Rectangle(position, dimensions);
            Obj = obj;
        }

        public object Obj { get; private set; }

        public Rectangle rectangle { get; private set; }
    }

    public class Rectangle
    {
        public Rectangle(Vector2f position, Vector2f dimensions)
        {
            Position = position;
            Dimensions = dimensions;
        }

        public bool Intersects(Rectangle other)
        {
            Vector2f myTopLeft = Position;
            Vector2f myBottomRight = new Vector2f(Position.X + Dimensions.X, Position.Y + Dimensions.Y);

            Vector2f otherTopLeft = other.Position;
            Vector2f otherBottomRight = new Vector2f(other.Position.X + other.Dimensions.X, other.Position.Y + other.Dimensions.Y);

            if (myTopLeft.X > otherBottomRight.X || myBottomRight.X < otherTopLeft.X)
                return false;

            if (myTopLeft.Y > otherBottomRight.Y || myBottomRight.Y < otherTopLeft.Y)
                return false;

            return true;
        }

        public Vector2f Dimensions { get; private set; }
        public Vector2f Position { get; private set; }
    }

    public class Quadtree 
    {
        private static bool render = true;
        private static bool setupOnce = false;
        private static int visualizationThickness = 1;

        public Quadtree(Rectangle boundary, int capacity)
        {
            Capacity = capacity;
            Boundary = boundary;

            if (!setupOnce)
            {
                shape.FillColor = SFML.Graphics.Color.Transparent;
                shape.OutlineColor = SFML.Graphics.Color.White;
                shape.OutlineThickness = visualizationThickness;
                setupOnce = true;
            }
        }

        public void clear()
        {
            entities.Clear();
            this.NW = null;
            this.NE = null;
            this.SW = null;
            this.SE = null;
        }

        public bool insert(QuadTreeEntity entity)
        {
            if (!this.Boundary.Intersects(entity.rectangle))
            {
                return false;
            }

            if (this.entities.Count < Capacity)
            {
                this.entities.Add(entity);
                return true;
            }

            // If NW is null, then we have not divided the quadtree yet
            if (NW == null)
            {
                subdivide();
            }

            bool insertedNW = this.NW.insert(entity);
            bool insertedNE = this.NE.insert(entity);
            bool insertedSW = this.SW.insert(entity);
            bool insertedSE = this.SE.insert(entity);

            return insertedNW || insertedNE || insertedSW || insertedSE;
        }

        public void Draw(RenderWindow window) 
        {
            if (render)
            {
                shape.Size = new Vector2f(Boundary.Dimensions.X - 2 * visualizationThickness, Boundary.Dimensions.Y - 2 * visualizationThickness);
                shape.Position = new Vector2f(Boundary.Position.X + visualizationThickness, Boundary.Position.Y + visualizationThickness);
                window.Draw(shape);

                if (this.NW != null)
                {
                    NW.Draw(window);
                    NE.Draw(window);
                    SW.Draw(window);
                    SE.Draw(window);
                }
            }
        }
        
        private void subdivide()
        {
            float x = Boundary.Position.X;
            float y = Boundary.Position.Y;
            float w = Boundary.Dimensions.X;
            float h = Boundary.Dimensions.Y;

            NW = new Quadtree(new Rectangle(new Vector2f(x, y), new Vector2f(w / 2, h / 2)), Capacity);
            NE = new Quadtree(new Rectangle(new Vector2f(x + (w/2), y), new Vector2f(w / 2, h / 2)), Capacity);
            SW = new Quadtree(new Rectangle(new Vector2f(x, y + (h/2)), new Vector2f(w / 2, h / 2)), Capacity);
            SE = new Quadtree(new Rectangle(new Vector2f(x + (w / 2), y + (h / 2)), new Vector2f(w / 2, h / 2)), Capacity);
        }

        private Quadtree NW = null;
        private Quadtree NE = null;
        private Quadtree SW = null;
        private Quadtree SE = null;

        private IList<QuadTreeEntity> entities = new List<QuadTreeEntity>();

        public Rectangle Boundary { get; private set; }
        public int Capacity { get; private set; }
        private static RectangleShape shape = new RectangleShape();
    }
}
