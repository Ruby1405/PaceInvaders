namespace PaceInvaders;

sealed class Bullet : Entity {
    public float damage = 10;
    private bool good = true;
    public Bullet() : base("bullet")
    {
    }

    public override void Create()
    {
        base.Create();
    }
    public override void Update(Scene scene, float deltaTime)
    {
        // Destroy if out of bounds
        // Destroy if colliding with enemy
    }
}
