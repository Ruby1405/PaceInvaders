using SFML.System;

namespace PaceInvaders;
public delegate void DamageEvent(float damage);
public delegate void BulletEvent(Entity source, float damage);
public delegate void ExplosionEvent(Vector2f position, Vector2f velocity);
public delegate void HealthEvent(int score);
public static class EventManager {
    public static BulletEvent FireBullet;
    public static DamageEvent StruckBeat;
    public static Action Beat;
    public static ExplosionEvent Explosion;
    public static Action GameLost;
    public static HealthEvent DecreaseHealth;
    public static void PublishFireBullet(Entity source, float damage) =>
        FireBullet.Invoke(source, damage);
    public static void PublishStruckBeat(float damage) =>
        StruckBeat.Invoke(damage);
    public static void PublishBeat() => Beat.Invoke();
    public static void PublishExplosion(Vector2f p, Vector2f v) => Explosion.Invoke(p,v);
    public static void PublishGameLost() => GameLost.Invoke();
    public static void PublishDecreaseHealth(int damage) =>
        DecreaseHealth.Invoke(damage);
}