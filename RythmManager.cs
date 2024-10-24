using SFML.System;
using SFML.Audio;

namespace PaceInvaders;
public sealed class RythmManager {
    private const float MAX_BEAT_TIME = 0.6f;
    private float beatTimer = MAX_BEAT_TIME;
    private Music music;
    public RythmManager() {
        music = AssetManager.LoadMusic("Kevin MacLeod - Impact.wav");
        music.Volume = 80;
        music.Loop = true;
    }
    public void Update(float deltaTime)
    {
        if (music.Status == SoundStatus.Paused) music.Play();
        if (beatTimer > 0) beatTimer -= deltaTime;

        if (beatTimer <= 0)
        {
            beatTimer = MAX_BEAT_TIME;
            EventManager.PublishBeat();
        }

        if (InputManager.InstantInputs[(int)UserActions.SHOOT])
        {
            // Check distance from note and dispatch event
            EventManager.PublishStruckBeat(50);
        }
    }
    public void Pause()
    {
        if (music.Status == SoundStatus.Playing) music.Pause();
    }
    public void Play() => music.Play();
}