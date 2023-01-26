using QuadTreeCollisions.Core;
using SFML.Graphics;
using SFML.System;
using System.Transactions;

namespace QuadTreeCollisions.Application.Entities
{
    public class Wave : Entity
    {
        public Wave()
        {
            initialPos = new Vector2f(Registry.Instance.window.SIZE.X / 2, Registry.Instance.window.SIZE.Y / 2);

            shape = new RectangleShape
            {
                Position = initialPos,
                FillColor = Color.Red,
                Size = new Vector2f(10, 10)
            };

            maxYMod = Registry.Instance.window.SIZE.Y / 3;
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(shape);
        }

        public override void Update(float deltaTimeSeconds)
        {
            Vector2f pos = new Vector2f(initialPos.X, initialPos.Y);

            float sinMod = (float)Math.Sin(timeCounter.ElapsedTime.AsSeconds());

            pos.Y = initialPos.Y + (sinMod * maxYMod);

            shape.Position = pos;
        }

        public RectangleShape shape { get; private set; }

        private Clock timeCounter = new Clock();
        private float maxYMod = 0;
        private float speed = 5f;
        private Vector2f initialPos;
    }
}
