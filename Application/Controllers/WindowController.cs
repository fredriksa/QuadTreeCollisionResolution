using QuadTreeCollisions.Core;
using QuadTreeCollisions.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Application.Controllers
{
    class WindowController : KeyboardeventListener
    {
        public override void OnKeyPressed(SFML.Window.KeyEventArgs e)
        {
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                Registry.Instance.window.WINDOW.Close();
            }
        }
    }
}
