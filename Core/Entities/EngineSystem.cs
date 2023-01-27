using QuadTreeCollisions.Core.Interfaces;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Core.Entities
{
    public class EngineSystem : IUpdateable, IDrawable
    {
        public EngineSystem() 
        {
            Registry.Instance.systems.Add(this);
        }

        ~EngineSystem()
        {
            Registry.Instance.systems.Remove(this);
        }

        public virtual void Draw(RenderWindow window) { }
        public virtual void Update(float deltaTimeSeconds) { }
    }
}
