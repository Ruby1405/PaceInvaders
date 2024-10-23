using SFML.System;

namespace PaceInvaders;
public sealed class RythmManager {
    private Clock songTime = new ();
    private const float MAX_BEAT_TIME = 1;
    private float beatTimer = MAX_BEAT_TIME;
    public RythmManager() {}
    public void Update(float deltaTime)
    {
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
}