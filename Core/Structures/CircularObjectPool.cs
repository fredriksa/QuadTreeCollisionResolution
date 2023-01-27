using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Structures
{
    public class CircularObjectPool<T>
    {
        public bool Available()
        {
            return available.Count > 0;
        }

        public T Get()
        {
            T last = available.Last();
            available.Remove(last);
            return last;
        }

        public void Add(T tree)
        {
            available.Add(tree);
        }

        private List<T> available = new List<T>();
    };
}
