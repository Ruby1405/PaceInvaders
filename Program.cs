using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PaceInvaders;
class Program {
    static void Main() {
        using var window = new RenderWindow(
        new VideoMode(828, 900), "Pace Invaders");
        
        Clock clock = new();
        Scene scene = new(window);

        Player p = new Player();
        p.Position = new Vector2f(200, 200);
        scene.Spawn(p);
        
        while (window.IsOpen)
        {
            window.DispatchEvents();
            float deltaTime = clock.Restart().AsSeconds();

            // Update
            scene.Update(deltaTime);

            // Draw
            window.Clear(Color.Black);
            scene.Render(window);

            window.Display();
        }
    }
}