using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Interfaces;
using QuadTreeCollisions.Core.Math;
using QuadTreeCollisions.Core.Physics;
using QuadTreeCollisions.Core.Physics.Colliders;
using SFML.Graphics;
using SFML.System;

namespace QuadTreeCollisions.Core.Structures
{
    public class QuadTree : IDrawable
    {
        
        /**
         * Note: Collision resolution performance can be improved by tweaking capacity and max depth depending on
         *       the collision resolution cost.
         */
        public QuadTree(Rectangle boundary, int capacity, int depth)
        {
            Capacity = capacity;
            rectangle = boundary;
            treeCollider = new RectangleCollider(rectangle, false); 

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
            colliders.Clear();

            if (NW != null)
            {
                NW.clear();
                NE.clear();
                SW.clear();
                SE.clear();

                quadTreePool.Add(NW);
                NW = null;

                quadTreePool.Add(NE);
                NE = null;

                quadTreePool.Add(SW);
                SW = null;

                quadTreePool.Add(SE);
                SE = null;
            }
        }

        public bool insert(Collider otherCollider)
        {
            if (!treeCollider.Intersects(otherCollider))
            {
                return false;
            }

            if (!isLeaf())
            {
                return insertIntoSubtrees(otherCollider);
            }

            if (colliders.Count < Capacity || depth == maxDepth)
            {
                colliders.Add(otherCollider);
                return true;
            }

            subdivide();
            return insertIntoSubtrees(otherCollider);
        }

        public IList<Collision> Collisions()
        {
            foreach (Collision collision in collisions)
            {
                collisionPool.Add(collision);
            }

            collisions.Clear();
            findIntersectionsHelper(collisions);
            return collisions;
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

        private void findIntersectionsHelper(IList<Collision> collisions)
        {
            if (!isLeaf())
            {
                NW.findIntersectionsHelper(collisions);
                NE.findIntersectionsHelper(collisions);
                SW.findIntersectionsHelper(collisions);
                SE.findIntersectionsHelper(collisions);
                return;
            }

            foreach (Collider colliderOne in colliders)
            {
                foreach (Collider colliderTwo in colliders)
                {
                    if (colliderOne == colliderTwo)
                    {
                        continue;
                    }

                    if (colliderOne.Intersects(colliderTwo))
                    {
                        Collision? collision = null;
                        if (collisionPool.Available())
                        {
                            collision = collisionPool.Get();
                        }
                        else
                        {
                            Console.WriteLine("New collider");
                            collision = new Collision();
                        }

                        Collision collisionVal = collision.Value;
                        collisionVal.One = colliderOne;
                        collisionVal.Two = colliderTwo;
                        collisions.Add(collisionVal);
                    }
                }
            }
        }

        private bool isLeaf()
        {
            return NW == null;
        }

        private bool insertIntoSubtrees(Collider collider)
        {
            bool insertedNW = NW.insert(collider);
            bool insertedNE = NE.insert(collider);
            bool insertedSW = SW.insert(collider);
            bool insertedSE = SE.insert(collider);

            return insertedNW || insertedNE || insertedSW || insertedSE;
        }
        
        private void subdivide()
        {
            float x = rectangle.Position.X;
            float y = rectangle.Position.Y;
            float w = rectangle.Dimensions.X;
            float h = rectangle.Dimensions.Y;

            if (quadTreePool.Available())
            {
                NW = quadTreePool.Get();
                NW.rectangle.Position = new Vector2f(x, y);
                NW.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                NW.Capacity = Capacity;
                NW.depth = depth + 1;
            }
            else
            {
                NW = new QuadTree(new Rectangle(new Vector2f(x, y), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (quadTreePool.Available())
            {
                NE = quadTreePool.Get();
                NE.rectangle.Position = new Vector2f(x + (w / 2), y);
                NE.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                NE.Capacity = Capacity;
                NE.depth = depth + 1;
            }
            else
            {
                NE = new QuadTree(new Rectangle(new Vector2f(x + (w / 2), y), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (quadTreePool.Available())
            {
                SW = quadTreePool.Get();
                SW.rectangle.Position = new Vector2f(x, y + (h / 2));
                SW.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                SW.Capacity = Capacity;
                SW.depth = depth + 1;
            }
            else
            {
                SW = new QuadTree(new Rectangle(new Vector2f(x, y + (h / 2)), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            if (quadTreePool.Available())
            {
                SE = quadTreePool.Get();
                SE.rectangle.Position = new Vector2f(x + (w / 2), y + (h / 2));
                SE.rectangle.Dimensions = new Vector2f(w / 2, h / 2);
                SE.Capacity = Capacity;
                SE.depth = depth + 1;
            }
            else
            {
                SE = new QuadTree(new Rectangle(new Vector2f(x + (w / 2), y + (h / 2)), new Vector2f(w / 2, h / 2)), Capacity, depth + 1);
            }

            foreach (Collider collider in colliders)
            {
                NW.insert(collider);
                NE.insert(collider);
                SW.insert(collider);
                SE.insert(collider);
            }
        }

        public Rectangle rectangle { get; private set; }
        public RectangleCollider treeCollider;
        
        public int Capacity { get; private set; }
        protected int depth = 0;
        private QuadTree? NW = null;
        private QuadTree? NE = null;
        private QuadTree? SW = null;
        private QuadTree? SE = null;

        private IList<Collider> colliders = new List<Collider>();
        private IList<Collision> collisions = new List<Collision>();

        private static RectangleShape shape = new RectangleShape();
        private static CircularObjectPool<Collision> collisionPool = new CircularObjectPool<Collision>();
        private static CircularObjectPool<QuadTree> quadTreePool = new CircularObjectPool<QuadTree>();
        private static bool render = true;
        private static bool setupOnce = false;
        private static int visualizationThickness = 1;
        private static int maxDepth = 7;
    }
}
 