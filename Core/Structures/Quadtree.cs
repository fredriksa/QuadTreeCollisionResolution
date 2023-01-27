using QuadTreeCollisions.Core.Entities;
using SFML.Graphics;
using SFML.System;

namespace QuadTreeCollisions.Core.Structures
{
    public class QuadTree 
    {
        
        /**
         * Note: Collision resolution performance can be improved by tweaking capacity and max depth depending on
         *       the collision resolution cost.
         */
        public QuadTree(Rectangle boundary, int capacity, int depth)
        {
            Capacity = capacity;
            rectangle = boundary;

            if (!setupOnce)
            {
                shape.FillColor = Color.Transparent;
                shape.OutlineColor = Color.White;
                shape.OutlineThickness = visualizationThickness;
                setupOnce = true;
            }

            this.depth = depth;
        }

        public void clear()
        {
            worldObjects.Clear();

            if (NW != null)
            {
                NW.clear();
                NE.clear();
                SW.clear();
                SE.clear();


                pool.Add(NW);
                NW = null;

                pool.Add(NE);
                NE = null;

                pool.Add(SW);
                SW = null;

                pool.Add(SE);
                SE = null;
            }
        }

        public bool insert(WorldObject worldObject)
        {
            if (!rectangle.Intersects(worldObject.rectangle))
            {
                return false;
            }

            if (!isLeaf())
            {
                return insertIntoSubtrees(worldObject);
            }

            if (worldObjects.Count < Capacity || depth == maxDepth)
            {
                worldObjects.Add(worldObject);
                return true;
            }

            subdivide();
            return insertIntoSubtrees(worldObject);
        }

        public void Draw(RenderWindow window) 
        {
            if (render)
            {
                shape.Size = new Vector2f(rectangle.Dimensions.X - 2 * visualizationThickness, rectangle.Dimensions.Y - 2 * visualizationThickness);
                shape.Position = new Vector2f(rectangle.Position.X + visualizationThickness, rectangle.Position.Y + visualizationThickness);
                window.Draw(shape);

                if (!isLeaf())
                {
                    NW.Draw(window);
                    NE.Draw(window);
                    SW.Draw(window);
                    SE.Draw(window);
                }
            }
        }

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

            foreach (WorldObject contained in worldObjects)
            {
                if (rectangle == contained.rectangle)
                    continue;

                if (rectangle.Intersects(contained.rectangle))
                    intersections.Add(contained);
            }
        }

        private bool isLeaf()
        {
            return NW == null;
        }

        private bool insertIntoSubtrees(WorldObject worldObject)
        {
            bool insertedNW = NW.insert(worldObject);
            bool insertedNE = NE.insert(worldObject);
            bool insertedSW = SW.insert(worldObject);
            bool insertedSE = SE.insert(worldObject);

            return insertedNW || insertedNE || insertedSW || insertedSE;
        }
        
        private void subdivide()
        {
            float x = rectangle.Position.X;
            float y = rectangle.Position.Y;
            float w = rectangle.Dimensions.X;
            float h = rectangle.Dimensions.Y;

            if (pool.Available())
            {
                NW = pool.Get();
                NW.rectangle.Position = new Vector2f(x, y);
                NW.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                NW.Capacity = Capacity;
                NW.depth = depth + 1;
            }
            else
            {
                NW = new QuadTree(new Rectangle(new Vector2f(x, y), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (pool.Available())
            {
                NE = pool.Get();
                NE.rectangle.Position = new Vector2f(x + (w / 2), y);
                NE.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                NE.Capacity = Capacity;
                NE.depth = depth + 1;
            }
            else
            {
                NE = new QuadTree(new Rectangle(new Vector2f(x + (w / 2), y), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (pool.Available())
            {
                SW = pool.Get();
                SW.rectangle.Position = new Vector2f(x, y + (h / 2));
                SW.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                SW.Capacity = Capacity;
                SW.depth = depth + 1;
            }
            else
            {
                SW = new QuadTree(new Rectangle(new Vector2f(x, y + (h/2)), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (pool.Available())
            {
                SE = pool.Get();
                SE.rectangle.Position = new Vector2f(x + (w / 2), y + (h / 2));
                SE.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                SE.Capacity = Capacity;
                SE.depth = depth + 1;
            }
            else
            {
                SE = new QuadTree(new Rectangle(new Vector2f(x + (w / 2), y + (h / 2)), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            foreach (WorldObject worldObject in worldObjects)
            {
                NW.insert(worldObject);
                NE.insert(worldObject);
                SW.insert(worldObject);
                SE.insert(worldObject);
            }
        }

        public Rectangle rectangle { get; private set; }
        public int Capacity { get; private set; }
        protected int depth = 0;
        private QuadTree? NW = null;
        private QuadTree? NE = null;
        private QuadTree? SW = null;
        private QuadTree? SE = null;
        private IList<WorldObject> worldObjects = new List<WorldObject>();
        private IList<WorldObject> intersections = new List<WorldObject>();

        private static RectangleShape shape = new RectangleShape();
        private static CircularObjectPool<QuadTree> pool = new CircularObjectPool<QuadTree>();
        private static bool render = true;
        private static bool setupOnce = false;
        private static int visualizationThickness = 1;
        private static int maxDepth = 7;
    }
}
 