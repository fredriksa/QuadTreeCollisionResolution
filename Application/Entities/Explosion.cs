using QuadTreeCollisions.Core;
using SFML.Graphics;
using SFML.System;

namespace QuadTreeCollisions.Application.Entities
{
    public class Explosion : Entity
    {
        public Explosion(Vector2f position)
        {
            rectangle.Position = position;

            if (explosionTexture == null)
            {
                // Todo: remove hardcoded path
                explosionTexture = new Texture("C:\\Users\\fredd\\source\\repos\\QuadTreeCollisions\\QuadTreeCollisions\\Assets\\Sprites\\explosion.png");
                sprite = new Sprite(explosionTexture);
                sprite.Scale = new Vector2f(0.1f, 0.1f);
            }
        }

        public override void Draw(RenderWindow window)
        {
            sprite.Position = new Vector2f(rectangle.Position.X - (sprite.TextureRect.Width * sprite.Scale.X * 0.5f),
                                           rectangle.Position.Y - (sprite.TextureRect.Height * sprite.Scale.Y * 0.5f));
            sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, (byte)alpha);
            window.Draw(sprite);
        }

        public override void Update(float deltaTimeSeconds)
        {
            alpha -= (255/3) * deltaTimeSeconds;

            if (destroyClock.ElapsedTime.AsSeconds() > 3)
            {
                Registry.Instance.updateables.Remove(this);
                Registry.Instance.drawables.Remove(this);
            }
        }

        private float alpha = 255;
        private Clock destroyClock = new Clock();
        private float rotation = 0;
        private static Texture explosionTexture = null;
        private static Sprite sprite = null;
    }
}
