using QuadTreeCollisions.Core;
using QuadTreeCollisions.Core.Entities;
using QuadTreeCollisions.Core.Physics.Colliders;
using QuadTreeCollisions.Core.Structures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Application.Entities
{
    public class Cube : Entity
    {
        public Cube() 
        {
            Random random = new Random();

            shape = new RectangleShape
            {
                FillColor = new Color((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256), 255),
            };

            rectCollider = new RectangleCollider(rectangle);
            rectCollider.AttachedTo = this;

            float angle = random.Next(360);
            direction.X = (float)Math.Cos(angle * (180 / Math.PI));
            direction.Y = (float)Math.Sin(angle * (180 / Math.PI));

            float totalDistance = Math.Abs(direction.X) + Math.Abs(direction.Y);
            direction.X = direction.X / totalDistance;
            direction.Y = direction.Y / totalDistance;
        }

        public override void Destroy()
        {
            base.Destroy();
            rectCollider.Destroy();

            new Explosion(this.rectangle.Position);
        }

        public override void Draw(RenderWindow window) 
        {
            shape.Position = rectangle.Position;
            shape.Size = rectangle.Dimensions;
            window.Draw(shape);
        }
        
        public override void Update(float deltaTimeSeconds) 
        {
            Vector2f toMove = (direction * speed * deltaTimeSeconds);

            if (rectangle.Position.X + toMove.X < 0)
            {
                float diff = rectangle.Position.X + toMove.X;

                toMove.X *= -1;
                toMove.X += diff;

                direction.X *= -1;
            }
            else if (rectangle.Position.X + toMove.X + rectangle.Dimensions.X > Registry.Instance.window.SIZE.X)
            {
                float diff = (rectangle.Position.X + toMove.X + rectangle.Dimensions.X) - Registry.Instance.window.SIZE.X;

                toMove.X *= -1;
                toMove.X += diff;

                direction.X *= -1;
            }

            if (rectangle.Position.Y + toMove.Y < 0)
            {
                float diff = rectangle.Position.Y + toMove.Y;

                toMove.Y *= -1;
                toMove.Y += diff;

                direction.Y *= -1;
            }
            else if (rectangle.Position.Y + toMove.Y + rectangle.Dimensions.Y > Registry.Instance.window.SIZE.Y)
            {
                float diff = (rectangle.Position.Y + toMove.Y + rectangle.Dimensions.Y) - Registry.Instance.window.SIZE.Y;

                toMove.Y *= -1;
                toMove.Y += diff;

                direction.Y *= -1;
            }

            rectangle.Position += toMove;
        }

        private RectangleCollider rectCollider;
        private RectangleShape shape;
        private Random random = new Random();
        private uint speed = 100;
        private Vector2f direction = new Vector2f(0, 0);
    }
}
