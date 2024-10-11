using SFML.System;

namespace PaceInvaders;
class RythmManager {
    private Clock songTime = new Clock();
    public RythmManager() {}
    public void Update(Scene scene)
    {
        if (InputManager.InstantInputs[(int)UserActions.SHOOT])
        {
            // Check distance from note and dispatch event
            EventManager.PublishFireBullet(1);
        }
    }
}