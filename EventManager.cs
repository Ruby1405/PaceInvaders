namespace PaceInvaders;
public delegate void DamageEvent(float damage);
public delegate void BulletEvent(Entity source, float damage);
public delegate void ScoreEvent(int score);
public static class EventManager {
    public static event BulletEvent? FireBullet;
    public static event DamageEvent? LooseHealth;
    public static event DamageEvent? StruckBeat;
    public static event ScoreEvent? EnemyKilled;
    public static void PublishFireBullet(Entity source, float damage) =>
        FireBullet?.Invoke(source, damage);
    public static void PublishStruckBeat(float damage) =>
        StruckBeat?.Invoke(damage);
    public static void PublishLooseHealth(float damage) =>
        LooseHealth?.Invoke(damage);
    public static void PublishEnemyKilled() =>
        EnemyKilled?.Invoke(1);
}