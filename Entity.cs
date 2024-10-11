using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;

abstract class Entity {
    private string textureName;
    protected Sprite sprite;
    protected Vector2f velocity;
    public Entity(string textureName) {
        this.textureName = textureName;
        sprite = new Sprite();
    }
    public virtual void Create() {
        sprite.Texture = AssetManager.LoadTexture(textureName);
    }
    public virtual void Move(Scene scene, float deltaTime) =>
        sprite.Position += velocity * deltaTime;
    public abstract void Update(Scene scene, float deltaTime);
    public void Render(RenderWindow window) {
        window.Draw(sprite);
    }
}