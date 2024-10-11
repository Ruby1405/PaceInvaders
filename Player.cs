using SFML.System;

namespace PaceInvaders;

sealed class Player : Entity {
    private const float STEERING_FORCE = 100;
    private const float MAX_SPEED = 100;
    private const float MAX_GRACE = 100;
    private float grace = 0;
    public Player() : base("player") {
    }
    public override void Move(Scene scene, float deltaTime)
    {
        Vector2f steering = new Vector2f(0,0);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_RIGHT]) steering += new Vector2f(1,0);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_LEFT]) steering += new Vector2f(-1,0);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_UP]) steering += new Vector2f(0,-1);
        if (InputManager.ActiveInputs[(int)UserActions.MOVE_DOWN]) steering += new Vector2f(0,1);

        velocity += steering.Normalize() * STEERING_FORCE * deltaTime;
        if (velocity.Length() > MAX_SPEED) velocity = velocity.Normalize() * MAX_SPEED;

        sprite.Position += velocity * deltaTime;
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (grace > 0) grace -= deltaTime;

        // if collided with enemy or enemy bullet
        EventManager.PublishLooseHealth(10);
    }
}