using Stride.Engine;

namespace custom_renderer
{
    class custom_rendererApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
