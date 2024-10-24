using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

public abstract class Entity {
    private string textureName;
    protected Sprite sprite;
    //protected CircleShape sprite;
    protected Vector2f velocity;
    public bool Dead { get; private set;}
    public Entity(string textureName) {
        this.textureName = textureName;
        sprite = new()
        {
            Origin = new (32f, 32f),
            Scale = new(0.5f,0.5f)
        };
        velocity = new Vector2f(0, 0);
    }
    public Vector2f Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }
    public virtual FloatRect Bounds => sprite.GetGlobalBounds();
    public virtual void Create() {
        sprite.Texture = AssetManager.LoadTexture(textureName);
    }
    public virtual void Destroy() {
        Dead = true;
    }
    public virtual void Move(float deltaTime) =>
        sprite.Position += velocity * deltaTime;
    public abstract void Update(float deltaTime);
    public void Render(RenderWindow window) {
        window.Draw(sprite);
    }
}