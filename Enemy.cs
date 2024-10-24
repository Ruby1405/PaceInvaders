using SFML.System;
using SFML.Graphics;

namespace PaceInvaders;

sealed class Enemy : Entity {
    private float health = 100;
    private const float MAX_SPEED = 100;
    private static readonly Random rng = new();
    public Vector2f Velocity => velocity;
    private readonly CircleShape thrusterExhaust = new(2);
    public Enemy() : base("enemy") {
        thrusterExhaust.Origin = new Vector2f(2,2);
    }
    public override void Create()
    {
        base.Create();
        sprite.Position = new Vector2f(rng.Next(0, 800), -20);
        velocity = new Vector2f(rng.NextSingle() -0.5f, 0.1f).Normalize() * MAX_SPEED;

        EventManager.Beat += OnBeat;
    }
    public override void Destroy()
    {
        base.Destroy();

        EventManager.Beat -= OnBeat;
    }
    private void OnBeat()
    {
        if (rng.Next(0,3) == 2)
        {
            EventManager.PublishFireBullet(this, 0);
        }
    }
    public override void Move(float deltaTime)
    {
        Position += velocity * deltaTime;

        if (Bounds.Left < 0)
        {
            Position = new Vector2f(sprite.Origin.X, Position.Y);
            velocity.X *= -1;
        }
        if (Bounds.Left + Bounds.Width > Scene.WIDTH)
        {
            Position = new Vector2f(Scene.WIDTH - sprite.Origin.X, Position.Y);
            velocity.X *= -1;
        }
        if (Bounds.Top > Scene.HEIGHT)
        {
            Position = new Vector2f(Position.X, -20);
        }

        thrusterExhaust.Position = sprite.Position + new Vector2f(0,16);
    }
    public override void Update(float deltaTime)
    {
        // If collided with player bullet
        foreach (var bullet in Scene.Entities.OfType<Bullet>().Where(bullet => bullet.good && !bullet.Dead))
        {
            if (Helpers.PointToRectCollision(bullet.Position,Bounds))
            {
                bullet.Destroy();
                health -= bullet.damage;
                if (health <= 0)
                {
                    Destroy();
                    EventManager.PublishExplosion(Position,velocity);
                    break;
                }
                sprite.Color = Color.Red;
            }
        }
        if ((Scene.Entities.Find(e => e is Player).Position - Position).Length() < 20)
        {
            Destroy();
            EventManager.PublishExplosion(Position,velocity);
        }

        thrusterExhaust.FillColor = rng.Next(0,2) == 1? Color.Red: Color.Yellow;
    }
    public override void Render(RenderWindow window){
        window.Draw(thrusterExhaust);
        base.Render(window);
    }
}