using QuadTreeCollisions.Application.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeCollisions.Application
{
    public class Game
    {
        public Game()
        {

        }

        public void Load()
        {
            windowController = new WindowController();
            cubeSpawnerController = new CubeSpawnerController();
        }

        private WindowController windowController;
        private CubeSpawnerController cubeSpawnerController;
    }
}
