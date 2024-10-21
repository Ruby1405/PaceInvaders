using SFML.System;

namespace PaceInvaders;
public sealed class RythmManager {
    private Clock songTime = new Clock();
    public RythmManager() {}
    public void Update(Scene scene)
    {
        if (InputManager.InstantInputs[(int)UserActions.SHOOT])
        {
            // Check distance from note and dispatch event
            EventManager.PublishStruckBeat(1);
        }
    }
}