using QuadTreeCollisions.Core.Interfaces;
using QuadTreeCollisions.Core.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Physics.Colliders
{
    public class RectangleCollider : Collider
    {
        public RectangleCollider(Rectangle rectangle, bool register = true) : base(register)
        {
            this.rectangle = rectangle;
        }

        public override bool Intersects(Collider other)
        {
            if (rectangle == null) return false;

            RectangleCollider? otherCollider = other as RectangleCollider;
            if (otherCollider != null)
            {
                return rectangle.Intersects(otherCollider.rectangle);
            }

            Console.WriteLine("WARNING! Rectangle could not be tested against other collider");
            return false;
        }

        private Rectangle? rectangle;
    }
}
