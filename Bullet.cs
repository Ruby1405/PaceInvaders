using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

sealed class Bullet : Entity {
    public readonly float damage;
    public readonly bool good;
    private const float SPEED = 400;
    public Vector2f Velocity
    {
        get => velocity;
        set
        {
            velocity = value.Normalize() * SPEED;
            sprite.Rotation = float.RadiansToDegrees((float)Math.Atan2(velocity.Y, velocity.X)) -90;
        }
    }

    public Bullet(bool good, float damage) : base("bullet") {
        this.good = good;
        this.damage = damage;
        //sprite.FillColor = Color.Cyan;
        sprite.Origin = new Vector2f(32f, 0f);
        sprite.Color = good ? Color.Cyan : Color.Yellow;
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
