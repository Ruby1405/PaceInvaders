using SFML.Graphics;

namespace PaceInvaders;

sealed class Bullet : Entity {
    public float damage = 10;
    private bool good = true;
    public Bullet() : base("bullet") {
        sprite.FillColor = Color.Red;
    }
    public override void Create()
    {
        base.Create();
    }
    public override void Update(Scene scene, float deltaTime)
    {
        // Destroy if colliding with enemy

        // Destroy if out of bounds
        if (Bounds.Left < -Bounds.Width ||
            Bounds.Top < -Bounds.Height ||
            Bounds.Left > Scene.WIDTH ||
            Bounds.Top > Scene.HEIGHT
        ) Destroy();
    }
}
