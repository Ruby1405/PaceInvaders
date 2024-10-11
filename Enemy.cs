using SFML.System;
using SFML.Window;

namespace PaceInvaders;

sealed class Enemy : Entity {
    private float health = 100;
    private static Random rng = new Random();
    public Enemy() : base("enemy") {
    }
    public override void Create()
    {
        base.Create();
        sprite.Position = new Vector2f(rng.Next(0, 800), -20);
    }
    public override void Update(Scene scene, float deltaTime)
    {
        // Bounce on walls
        // If collided with player bullet
        EventManager.PublishEnemyKilled();
    }
}