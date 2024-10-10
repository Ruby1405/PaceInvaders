using SFML.Graphics;

namespace PaceInvaders;

class Scene
{
    private List<Entity> entities;
    public readonly InputManager Inputs;
    public readonly AssetManager Assets = new();


    public Scene(RenderWindow window) {
        entities = [];

        Inputs = new InputManager(window);
    }

    public void Spawn(Entity entity) {
        entities.Add(entity);
        entity.Create();
    }

    public void Update(float deltaTime) {
        foreach (Entity entity in entities) entity.Move(deltaTime);
        foreach (Entity entity in entities) entity.Update(deltaTime);
    }
}