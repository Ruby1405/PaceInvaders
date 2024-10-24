using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PaceInvaders;

sealed class Player : Entity {
    private const float STEERING_FORCE = 500;
    private const float MAX_SPEED = 200;
    private const float MAX_GRACE = 1;
    private float grace = 0;
    private static readonly Random rng = new();
    private readonly CircleShape thrusterExhaust = new(2);
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
        thrusterExhaust.Origin = new Vector2f(2,2);
        
        guns = [
            new Vector2f(-15, -20),
            new Vector2f(15, -20)
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
            velocity = Helpers.HadamardProduct(velocity, new Vector2f(-0.5f,1));
        }
        if (Bounds.Top < 0)
        {
            Position = new Vector2f(Position.X, sprite.Origin.Y);
            velocity = Helpers.HadamardProduct(velocity, new Vector2f(1,-0.5f));
        }
        if (Bounds.Left + Bounds.Width > Scene.WIDTH)
        {
            Position = new Vector2f(Scene.WIDTH - sprite.Origin.X, Position.Y);
            velocity = Helpers.HadamardProduct(velocity, new Vector2f(-0.5f,1));
        }
        if (Bounds.Top + Bounds.Height > Scene.HEIGHT)
        {
            Position = new Vector2f(Position.X, Scene.HEIGHT - sprite.Origin.Y);
            velocity = Helpers.HadamardProduct(velocity, new Vector2f(1,-0.5f));
        }
        thrusterExhaust.Position = sprite.Position + new Vector2f(0,16);
    }

    public override void Update(float deltaTime)
    {
        if (grace > 0) grace -= deltaTime;
        if (grace > 0) return;
        
        // if collided with enemy bullet
        foreach (var bullet in Scene.Entities.OfType<Bullet>().Where(b => !b.good && !b.Dead))
        {
            if (!Helpers.PointToRectCollision(bullet.Position,Bounds)) continue;
            bullet.Destroy();
            EventManager.PublishDecreaseHealth(1);
            grace = MAX_GRACE;
            break;
        }
        if (grace > 0) return;
        
        // if collided with enemy
        foreach (var enemy in Scene.Entities.OfType<Enemy>().Where(e => !e.Dead))
        {
            if (!Helpers.RectToRectCollision(Bounds,enemy.Bounds)) continue;
            enemy.Destroy();
            EventManager.PublishDecreaseHealth(1);
            EventManager.PublishExplosion(enemy.Position,enemy.Velocity);
            grace = MAX_GRACE;
            break;
        }
        thrusterExhaust.FillColor = rng.Next(0,2) == 1? Color.Red: Color.Yellow;
    }
    public override void Render(RenderWindow window){
        window.Draw(thrusterExhaust);
        base.Render(window);
    }
}