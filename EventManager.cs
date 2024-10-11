namespace PaceInvaders;
public delegate void DamageEvent(float damage);
public delegate void ScoreEvent(int score);
public static class EventManager {
    public static event DamageEvent? FireBullet;
    public static event DamageEvent? LooseHealth;
    public static event ScoreEvent? EnemyKilled;
    public static void PublishFireBullet(float damage) =>
        FireBullet?.Invoke(damage);
    public static void PublishLooseHealth(float damage) =>
        LooseHealth?.Invoke(damage);
    public static void PublishEnemyKilled() =>
        EnemyKilled?.Invoke(1);
}