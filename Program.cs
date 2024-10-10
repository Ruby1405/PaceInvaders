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
    }
}