using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;
public enum State {
    MENU,
    PAUSE,
    PLAY,
    HIGHSCORES,
    GAME_OVER,
    QUIT
}
public sealed class Scene
{
    public const int WIDTH = 720;
    public const int HEIGHT = 960;
    private const float SCORE_TIME = 1f;
    private static float scoreTimer = SCORE_TIME;
    private static readonly GUI gui = new();
    public static RythmManager rythm = new();
    private static List<Explosion> explosionBuffer = [];
    public static State State { get; set; }
    public static List<Entity> Entities { get; private set; } = [];
    public static int Score { get; private set; }
    public static int Health { get; private set; }
    private struct Difficulty
    {
        public const int BEAT_COUNTER_TARGET = 8;
        public int beatCounter = 0;
        public int rythmSpawns = 1;
        public int MAX_SPAWNS = 20;
        public int incrementCounterTarget = 4;
        public int incrementCounter = 0;
        public Difficulty(){}
    }
    private static Difficulty difficulty;
    public Scene(RenderWindow window) {
        State = State.MENU;
        InputManager.InitializeInputs(window);

        EventManager.FireBullet += OnFireBullet;
        EventManager.DecreaseHealth += OnDecreaseHealth;
        EventManager.Beat += OnBeat;
        EventManager.Explosion += OnExplosion;
    }
    public static void NewGame() {
        foreach (var entity in Entities)
        {
            entity.Destroy();
        }
        Entities.Clear();
        explosionBuffer.Clear();

        Player player = new()
        {
            Position = new Vector2f(200, 200)
        };
        Spawn(player);

        difficulty = new();

        Score = 0;
        Health = 3;

        rythm = new();
        rythm.Play();
    }
    public static void Spawn(Entity entity) {
        Entities.Add(entity);
        entity.Create();
        //Console.WriteLine($"added {entity}");
    }
    private void OnDecreaseHealth(int health)
    {
        Health -= health;
        if (Health <= 0)
        {
            State = State.GAME_OVER;
            EventManager.PublishGameLost();
        }
    }
    private void OnBeat() {
        if (difficulty.beatCounter < Difficulty.BEAT_COUNTER_TARGET)
        {
            difficulty.beatCounter ++;
            return;
        }
        difficulty.beatCounter = 0;
        if (difficulty.rythmSpawns < difficulty.MAX_SPAWNS)
        {
            if (difficulty.incrementCounter < difficulty.incrementCounterTarget) difficulty.incrementCounter ++;
            else 
            {
                difficulty.incrementCounter = 0;
                difficulty.incrementCounterTarget ++;
                difficulty.rythmSpawns ++;
            }
        }
        for (int i = 0; i < difficulty.rythmSpawns; i++)
        {
            Spawn(new Enemy());
        }
    }
    private void OnFireBullet(Entity source, float damage)
    {
        if (source is Player p)
        {
            foreach (Vector2f gun in p.Guns)
            {
                Spawn(new Bullet(true, damage)
                {
                    Position = gun,
                    Velocity = new Vector2f(0, -1)
                });
            }
        }
        else
        {
            Spawn(new Bullet(false, damage)
            {
                Position = source.Position,
                Velocity = Entities.Find(entity => entity is Player).Position - source.Position
            });
        }
    }
    private void OnExplosion(Vector2f p, Vector2f v)
    {
        explosionBuffer.Add(new Explosion()
        {
            Position = p,
            Velocity = v
        });
    }
    public void Update(float deltaTime) {
        switch (State)
        {
            case State.PLAY:
                rythm.Update(deltaTime);
                while (explosionBuffer.Count > 0)
                {
                    Spawn(explosionBuffer[0]);
                    explosionBuffer.RemoveAt(0);
                }
                foreach (Entity entity in Entities) entity.Move(deltaTime);
                foreach (Entity entity in Entities) entity.Update(deltaTime);
                for (int i = 0; i < Entities.Count;)
                {
                    if (Entities[i].Dead)
                    {
                        Entities.RemoveAt(i);
                    }
                    else i++;
                }
                if (scoreTimer <= 0)
                {
                    scoreTimer = SCORE_TIME;
                    Score++;
                }
                else scoreTimer -= deltaTime;
                break;
            default:
                rythm.Pause();
                break;
        }
        gui.Update();
        InputManager.Update();
    }
    public void Render(RenderWindow window) {
        switch (State)
        {
            case State.PLAY:
            case State.PAUSE:
                foreach (Entity entity in Entities) entity.Render(window);
                break;
            case State.QUIT:
                window.Close();
                break;
        }
        gui.Render(window);
    }
}