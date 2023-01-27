using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Math;
using QuadTreeCollisions.Core.Physics;
using QuadTreeCollisions.Core.Physics.Colliders;
using QuadTreeCollisions.Core.Structures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Systems
{
    public class CollisionSystem : EngineSystem
    {
        // TODO: Support static collision tree (only has to be built once!)
        public CollisionSystem() 
        {
            Rectangle treeRect = new Rectangle(new Vector2f(0, 0), (Vector2f)Registry.Instance.window.SIZE);
            quadTree = new QuadTree(treeRect, 2, 0);
        }

        public override void Update(float deltaTimeSeconds)
        {
            quadTree.clear();

            for (int i = 0; i < Registry.Instance.colliders.Count; i++)
            {
                quadTree.insert(Registry.Instance.colliders[i]);
            }

            IList<Collision> collidingColliders = quadTree.Collisions();

            foreach (Collision collision in collidingColliders)
            {
                Entity one = collision.One.AttachedTo as Entity;
                Entity two = collision.Two.AttachedTo as Entity;

                if (one != null && two != null)
                {
                    one.OnCollide(collision.Two);
                    two.OnCollide(collision.One);
                }
            }

            Console.WriteLine($"Colliding Colliders {collidingColliders.Count}");
        }

        public override void Draw(RenderWindow window)
        {
            quadTree.Draw(window);
        }

        private QuadTree quadTree;
    }
}
