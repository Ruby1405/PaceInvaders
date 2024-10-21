using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;
public enum State {
    MENU,
    PLAY,
    PAUSE,
    GAME_OVER
}
public sealed class Scene
{
    private List<Entity> entities;
    public const int WIDTH = 828;
    public const int HEIGHT = 900;
    private State state;
    public readonly RythmManager Rythm = new();
    private readonly GUI gui = new();
    public Scene(RenderWindow window) {
        entities = [];
        state = State.PLAY;
        InputManager.InitializeInputs(window);
        EventManager.FireBullet += OnFireBullet;
    }
    public void Spawn(Entity entity) {
        entities.Add(entity);
        Console.WriteLine("added entity" + entity);
        entity.Create();
    }

    private void OnFireBullet(Entity source, float damage)
    {
        if (source is Player)
        {
            Player p = source as Player;
            foreach (Vector2f gun in p.guns)
            {
                Bullet bullet = new Bullet();
                bullet.damage = damage;
                bullet.Position = gun;
                bullet.velocity = new Vector2f(0, -400);
                Spawn(bullet);
            }
        }
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