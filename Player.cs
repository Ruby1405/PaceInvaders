using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

sealed class Player : Entity {
    private const float STEERING_FORCE = 400;
    private const float MAX_SPEED = 200;
    private const float MAX_GRACE = 1;
    private float grace = 0;
    private Vector2f[] guns;
    public Vector2f[] Guns {
        get {
            Vector2f[] arr = new Vector2f[guns.Length];
            for (int i = 0; i < guns.Length; i++)
            {
                arr[i] = Position + guns[i];
            }
            return arr;
        }
    }
    public Player() : base("player")
    {
        sprite.FillColor = Color.Green;
        guns = [
            new Vector2f(-10, -5),
            new Vector2f(10, -5)
        ];
    }
    public override void Create()
    {
        base.Create();
        EventManager.StruckBeat += OnStruckBeat;
    }
    public override void Destroy()
    {
        base.Destroy();
        EventManager.StruckBeat -= OnStruckBeat;
    }
    private void OnStruckBeat(float damage) => EventManager.PublishFireBullet(this, damage);
    public override void Move(float deltaTime)
    {
        Vector2f steering = new Vector2f(0,0);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_RIGHT]) steering += new Vector2f(1,0);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_LEFT]) steering += new Vector2f(-1,0);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_UP]) steering += new Vector2f(0,-1);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_DOWN]) steering += new Vector2f(0,1);
        
        velocity += steering.Normalize() * STEERING_FORCE * deltaTime;
        
        if (velocity.Length() > MAX_SPEED) velocity = velocity.Normalize() * MAX_SPEED;
        Position += velocity * deltaTime;

        if (Bounds.Left < 0)
        {
            Position = new Vector2f(sprite.Origin.X, Position.Y);
            velocity = velocity.HadamardProduct(new Vector2f(-0.5f,1));
        }
        if (Bounds.Top < 0)
        {
            Position = new Vector2f(Position.X, sprite.Origin.Y);
            velocity = velocity.HadamardProduct(new Vector2f(1,-0.5f));
        }
        if (Bounds.Left + Bounds.Width > Scene.WIDTH)
        {
            Position = new Vector2f(Scene.WIDTH - sprite.Origin.X, Position.Y);
            velocity = velocity.HadamardProduct(new Vector2f(-0.5f,1));
        }
        if (Bounds.Top + Bounds.Height > Scene.HEIGHT)
        {
            Position = new Vector2f(Position.X, Scene.HEIGHT - sprite.Origin.Y);
            velocity = velocity.HadamardProduct(new Vector2f(1,-0.5f));
        }
    }

    public override void Update(float deltaTime)
    {
        if (grace > 0) grace -= deltaTime;

        if (grace <= 0)
        {
            // if collided with enemy or enemy bullet
            foreach (var bullet in Scene.Entities.OfType<Bullet>().Where(b => !b.good && !b.Dead))
            {
                // FIX
                if ((bullet.Position - Position).Length() < 20)
                {
                    bullet.Destroy();
                    EventManager.PublishDecreaseHealth(1);
                    grace = MAX_GRACE;
                    break;
                }
            }
            if (grace <= 0) foreach (var enemy in Scene.Entities.OfType<Enemy>().Where(e => !e.Dead))
            {
                // FIX
                if ((enemy.Position - Position).Length() < 20)
                {
                    enemy.Destroy();
                    EventManager.PublishDecreaseHealth(1);
                    grace = MAX_GRACE;
                    break;
                }
            }
        }
    }
}