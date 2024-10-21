using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

public abstract class Entity {
    private string textureName;
    //protected Sprite sprite;
    protected CircleShape sprite;
    public Vector2f velocity;
    public Entity(string textureName) {
        this.textureName = textureName;
        //sprite = new Sprite();
        sprite = new CircleShape(10);
        sprite.Origin = new Vector2f(10f, 10f);
        sprite.FillColor = Color.Green;
        velocity = new Vector2f(0, 0);
        Console.WriteLine(Bounds);
    }
    public Vector2f Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }
    public virtual FloatRect Bounds => sprite.GetGlobalBounds();
    public virtual void Create() {
        //sprite.Texture = AssetManager.LoadTexture(textureName);
    }
    public virtual void Move(Scene scene, float deltaTime) =>
        sprite.Position += velocity * deltaTime;
    public abstract void Update(Scene scene, float deltaTime);
    public virtual void Render(RenderWindow window) {
        window.Draw(sprite);
    }
}