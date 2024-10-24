using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PaceInvaders;
class Program {
    static void Main() {
        using RenderWindow window = new(
        new VideoMode(828, 900), "Pace Invaders");
        window.Closed += (o, e) => window.Close();
        
        Clock clock = new();
        Scene scene = new(window);
        
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