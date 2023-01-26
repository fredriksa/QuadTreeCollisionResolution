using SFML.Graphics;
using SFML.System;

namespace QuadTreeCollisions.Core.Structures
{
    public class Rectangle
    {
        public Rectangle(Vector2f position, Vector2f dimensions)
        {
            Position = position;
            Dimensions = dimensions;
        }

        public Rectangle() 
        {
            
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

        public Vector2f Dimensions { get; set; } = new Vector2f(0, 0);
        public Vector2f Position { get; set; } = new Vector2f(0, 0);
    }

    public class QuadTreePool
    {
        public bool Available()
        {
            return available.Count > 0;
        }

        public Quadtree Dequeue()
        {
            return available.Dequeue();
        }

        public void Enqueue(Quadtree tree)
        {
            available.Enqueue(tree);
        }

        private Queue<Quadtree> available = new Queue<Quadtree>();
    };

    public class Quadtree 
    {
        public Quadtree(Rectangle boundary, int capacity, int depth)
        {
            Capacity = capacity;
            rectangle = boundary;

            if (!setupOnce)
            {
                shape.FillColor = SFML.Graphics.Color.Transparent;
                shape.OutlineColor = SFML.Graphics.Color.White;
                shape.OutlineThickness = visualizationThickness;
                setupOnce = true;
            }

            this.depth = depth;
        }

        private static QuadTreePool pool = new QuadTreePool();

        public void clear()
        {
            entities.Clear();

            if (NW != null)
            {
                NW.clear();
                NE.clear();
                SW.clear();
                SE.clear();


                pool.Enqueue(NW);
                NW = null;

                pool.Enqueue(NE);
                NE = null;

                pool.Enqueue(SW);
                SW = null;

                pool.Enqueue(SE);
                SE = null;
            }
        }

        public bool insert(WorldObject worldObject)
        {
            if (!rectangle.Intersects(worldObject.rectangle))
            {
                return false;
            }

            if (entities.Count < Capacity || depth == maxDepth)
            {
                entities.Add(worldObject);
                return true;
            }

            // If NW is null, then we have not divided the quadtree yet
            if (NW == null)
            {
                subdivide();
            }

            bool insertedNW = NW.insert(worldObject);
            bool insertedNE = NE.insert(worldObject);
            bool insertedSW = SW.insert(worldObject);
            bool insertedSE = SE.insert(worldObject);

            return insertedNW || insertedNE || insertedSW || insertedSE;
        }

        public void Draw(RenderWindow window) 
        {
            if (render)
            {
                shape.Size = new Vector2f(rectangle.Dimensions.X - 2 * visualizationThickness, rectangle.Dimensions.Y - 2 * visualizationThickness);
                shape.Position = new Vector2f(rectangle.Position.X + visualizationThickness, rectangle.Position.Y + visualizationThickness);
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

        private IList<WorldObject> intersections = new List<WorldObject>();

        public IList<WorldObject> findIntersections(Rectangle rectangle)
        {
            intersections.Clear();
            findIntersectionsHelper(intersections, rectangle);
            return intersections;
        }

        private void findIntersectionsHelper(IList<WorldObject> intersections, Rectangle rectangle)
        {
            if (NW != null)
            {
                NW.findIntersectionsHelper(intersections, rectangle);
                NE.findIntersectionsHelper(intersections, rectangle);
                SW.findIntersectionsHelper(intersections, rectangle);
                SE.findIntersectionsHelper(intersections, rectangle);
                return;
            }

            foreach (WorldObject contained in entities)
            {
                if (rectangle.Intersects(contained.rectangle))
                {
                    intersections.Add(contained);
                }
            }
        }
        
        private void subdivide()
        {
            float x = rectangle.Position.X;
            float y = rectangle.Position.Y;
            float w = rectangle.Dimensions.X;
            float h = rectangle.Dimensions.Y;

            if (pool.Available())
            {
                NW = pool.Dequeue();
                NW.rectangle.Position = new Vector2f(x, y);
                NW.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                NW.Capacity = Capacity;
                NW.depth = depth + 1;
            }
            else
            {
                NW = new Quadtree(new Rectangle(new Vector2f(x, y), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (pool.Available())
            {
                NE = pool.Dequeue();
                NE.rectangle.Position = new Vector2f(x + (w / 2), y);
                NE.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                NE.Capacity = Capacity;
                NE.depth = depth + 1;
            }
            else
            {
                NE = new Quadtree(new Rectangle(new Vector2f(x + (w / 2), y), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (pool.Available())
            {
                SW = pool.Dequeue();
                SW.rectangle.Position = new Vector2f(x, y + (h / 2));
                SW.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                SW.Capacity = Capacity;
                SW.depth = depth + 1;
            }
            else
            {
                SW = new Quadtree(new Rectangle(new Vector2f(x, y + (h/2)), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (pool.Available())
            {
                SE = pool.Dequeue();
                SE.rectangle.Position = new Vector2f(x + (w / 2), y + (h / 2));
                SE.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                SE.Capacity = Capacity;
                SE.depth = depth + 1;
            }
            else
            {
                SE = new Quadtree(new Rectangle(new Vector2f(x + (w / 2), y + (h / 2)), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }
        }

        private Quadtree? NW = null;
        private Quadtree? NE = null;
        private Quadtree? SW = null;
        private Quadtree? SE = null;
        protected int depth = 0;

        private IList<WorldObject> entities = new List<WorldObject>();
        public Rectangle rectangle { get; private set; }
        public int Capacity { get; private set; }

        private static RectangleShape shape = new RectangleShape();
        private static bool render = true;
        private static bool setupOnce = false;
        private static int visualizationThickness = 1;
        private static int maxDepth = 10;
    }
}
