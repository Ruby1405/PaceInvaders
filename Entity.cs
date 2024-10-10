using SFML.Graphics;

namespace PaceInvaders;

class Entity {
    private string textureName;
    protected Sprite sprite;

    public Entity(string textureName) {
        this.textureName = textureName;
        sprite = new Sprite();
    }

    public void Create() {
        sprite.Texture = AssetManager.LoadTexture(textureName);
    }

    public void Move(float deltaTime) {
    }

    public void Update(float deltaTime) {
    }
}