using SFML.Graphics;

namespace PaceInvaders;
public enum State {
    MENU,
    PLAY,
    PAUSE,
    GAME_OVER
}
class Scene
{
    private List<Entity> entities;
    private State state;
    public readonly RythmManager Rythm = new();
    private readonly GUI gui = new();
    public Scene(RenderWindow window) {
        entities = [];
        state = State.MENU;
        InputManager.InitializeInputs(window);
    }
    public void Spawn(Entity entity) {
        entities.Add(entity);
        entity.Create();
    }

    public void Update(float deltaTime) {
        Rythm.Update(this);
        switch (state)
        {
            case State.PLAY:
                Rythm.Update(this);
                foreach (Entity entity in entities) entity.Move(this, deltaTime);
                foreach (Entity entity in entities) entity.Update(this, deltaTime);
                break;
        }
        gui.Update(this);
        InputManager.Update();
    }

    public void Render(RenderWindow window) {
        switch (state)
        {
            case State.PLAY:
            case State.PAUSE:
                foreach (Entity entity in entities) entity.Render(window);
                foreach (Entity entity in entities) entity.Render(window);
                break;
        }
        gui.Render(window);
    }
}