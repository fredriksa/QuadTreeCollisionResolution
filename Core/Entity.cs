using QuadTreeCollisions.Core.Interfaces;
using SFML.Graphics;

namespace QuadTreeCollisions.Core
{
    public class Entity : WorldObject, IDrawable, IUpdateable
    {
        public Entity()
        {
            Registry.Instance.drawables.Add(this);
            Registry.Instance.updateables.Add(this);
        }

        ~Entity()
        {
            Registry.Instance.drawables.Remove(this);
            Registry.Instance.updateables.Remove(this);
        }

        public virtual void Draw(RenderWindow window) { }
        public virtual void Update(float deltaTimeSeconds) { }
    }
}
