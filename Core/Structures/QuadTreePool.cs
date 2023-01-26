using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Structures
{
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
}
