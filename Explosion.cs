using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

sealed class Explosion : Entity {
    private const float SPEED = 400;
    private float duration = 1;
    public Vector2f Velocity { get => velocity; set => velocity = value.Normalize() * SPEED; }
    public Explosion() : base("bullet") {
        sprite.FillColor = Color.Red;
    }
    public override void Update(float deltaTime)
    {
        duration -= deltaTime;
        // Destroy if out of bounds
        if (duration <= 0) Destroy();
    }
}