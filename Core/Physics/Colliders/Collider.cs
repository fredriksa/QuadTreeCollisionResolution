using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Physics.Colliders
{
    public class Collider
    {
        public Collider(bool register = true)
        {
            this.register = register;
            if (register)
            {
                Registry.Instance.colliders.Add(this);
            }
        }

        public virtual bool Intersects(Collider other)
        {
            return false;
        }

        public virtual void Destroy()
        {
            Registry.Instance.colliders.Remove(this);
        }

        public WorldObject? AttachedTo { get; set; }
        private bool register;
    }
}
