using QuadTreeCollisions.Core.Interfaces;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Math
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
}
