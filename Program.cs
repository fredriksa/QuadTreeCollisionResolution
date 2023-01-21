using QuadTreeCollisions.Application;
using QuadTreeCollisions.Core;

class Program
{
    public static void Main(string[] args)
    {
        Engine engine = new Engine();
        engine.Load();
        Game game = new Game();
        game.Load();
        engine.Run();
    }
}