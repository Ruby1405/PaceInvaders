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
    public const int WIDTH = 828;
    public const int HEIGHT = 900;
    private const float SCORE_TIME = 1f;
    private static float scoreTimer = SCORE_TIME;
    private static readonly GUI gui = new();
    public static readonly RythmManager rythm = new();
    private static List<Explosion> explosionBuffer = [];
    public static State State { get; set; }
    public static List<Entity> Entities { get; private set; } = [];
    public static int Score { get; private set; }
    public static int Health { get; private set; }
    private struct Difficulty
    {
        public int onBeatSpawns = 0;
        public const int MAX_N_BEATS = 4;
        public int spawnEveryNBeats = MAX_N_BEATS;
        public int beatCounter = MAX_N_BEATS;
        public Difficulty(){}
    }
    // FIX
    private static Difficulty difficulty;
    public Scene(RenderWindow window) {
        State = State.MENU;
        InputManager.InitializeInputs(window);

        EventManager.FireBullet += OnFireBullet;
        EventManager.DecreaseHealth += OnDecreaseHealth;
        EventManager.Beat += OnBeat;
        EventManager.Explosion += OnExplosion;

        NewGame();
    }
    public static void NewGame() {
        foreach (var entity in Entities)
        {
            entity.Destroy();
        }
        Entities.Clear();

        Player player = new()
        {
            Position = new Vector2f(200, 200)
        };
        Spawn(player);

        difficulty = new();

        Score = 0;
        Health = 3;
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
        if (difficulty.beatCounter > 0) difficulty.beatCounter --;
        //Console.WriteLine("BEAT" + " c" + difficulty.beatCounter + " n" + difficulty.spawnEveryNBeats + " x" + difficulty.onBeatSpawns);
        for (int i = 0; i < difficulty.onBeatSpawns; i++)
        {
            Spawn(new Enemy());
        }
        if (difficulty.beatCounter == 0)
        {
            Spawn(new Enemy());
            difficulty.spawnEveryNBeats --;
            if (difficulty.spawnEveryNBeats == 1)
            {
                difficulty.spawnEveryNBeats = Difficulty.MAX_N_BEATS;
                difficulty.onBeatSpawns ++;
            }
            difficulty.beatCounter = difficulty.spawnEveryNBeats;
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
                //FIX
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
            //FIX
            case State.QUIT:
                window.Close();
                break;
        }
        gui.Render(window);
    }
}