using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

sealed class Bullet : Entity {
    public readonly float damage;
    public readonly bool good;
    private const float SPEED = 400;
    public Vector2f Velocity { get => velocity; set => velocity = value.Normalize() * SPEED; }
    public Bullet(bool good, float damage) : base("bullet") {
        sprite.FillColor = Color.Cyan;
        this.good = good;
        this.damage = damage;
    }
    public override void Update(float deltaTime)
    {
        // Destroy if out of bounds
        if (Bounds.Left < -Bounds.Width ||
            Bounds.Top < -Bounds.Height ||
            Bounds.Left > Scene.WIDTH ||
            Bounds.Top > Scene.HEIGHT
        ) Destroy();
    }
}
