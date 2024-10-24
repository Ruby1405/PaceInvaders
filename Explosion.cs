using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

sealed class Explosion : Entity {
    private float duration = 1;
    public Vector2f Velocity { set => velocity = value; }
    public Explosion() : base("explosion") {
        //sprite.FillColor = Color.Red;
    }
    public override void Update(float deltaTime)
    {
        duration -= deltaTime;
        // Destroy if out of bounds
        if (duration <= 0) Destroy();
    }
}