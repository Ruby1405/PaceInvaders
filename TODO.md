- [x] Player ship
    - [x] Can be moved by player, up, down, left and right
    - [x] Can't go outside the screen
    - [x] Player can shoot bullets (2 at a time)
    - [x] Takes damage by colliding with enemy bullets or enemy ships
    - [x] Doesn't take damage by colliding with own bullets
    - [x] Is immortal for a short time after taking damage
- [ ] Enemy Ship
    - [x] Moves diagonally down, "bouncing" on the edges of the screen
    - [x] Shoots bullets randomly, but not every frame
    - [x] Takes damage by colliding with player bullets or player ship
    - [x] Doesn't take damage by colliding with enemy bullets
    - [x] Spawns at a random position, but always fully outside the screen at the top (not too far away)
    - [x] If it exits the bottom of the screen, it moves back up, making it even more difficult
    - [ ] As time goes by, new enemies are spawned faster and faster, but never fast enough to become impossible (There are different approaches to this, pick one that you can argue makes the game fun)
- [x] Bullets
    - [x] Movement direction is based on the ship that fired it
    - [x] Is destroyed when colliding with a differently allied ship
    - [x] Both player and enemies shoot bullets of the same class
- [x] Points
    - [x] Player gets score for each second that they stay alive
    - [x] Score is displayed on screen
    - [x] Is reset when the game is lost
- [x] Explosions
    - [x] Enemy ship explodes when damaged
    - [x] Colliding with explosions does not affect any ships or bullets
- [x] Health
    - [x] Players health is displayed on screen
    - [x] Player loses health when taking damage
    - [x] When health == 0, the game is lost. (You shouldn't need to restart the game executable to play again.)
- [x] Graphics
    - [x] All of the above game elements have a functional graphical representation
    - [x] Both textures and Fonts are used
- [x] Optimization
    - [x] The player should theoretically be able to play “forever” without the game breaking
            hint: clear bullets from memory when they exit the screen
- [ ] Code
    - [ ] Should be structured appropriately and follow Object Oriented principles with reasonable coupling and cohesion
    - [ ] Shouldn't have unnecessary repetition of code, or code that isn't used
    - [ ] Should be clear and well-structured enough to understand for another programmer, in regards to both readability and usability
    - [ ] It should be sufficiently easy to add new features or make changes to existing ones, as well as reuse different parts for other games
- [ ] Bonus Features (for grade VG)
    - [x] Let the player enter a name
    - [x] A High-Score list that is persistent between sessions
    - [x] A main menu with (at least) the alternatives “New Game”, “High score” and “Quit”
    - [ ] Use sound effect and/or music in the game

# notes
spawn enemies only on some beats
player bullets deal more damage the closer they are to beat
3d from a "45" degree angle

- [ ] UI input tips
- [ ] use submit or space
- [ ] fix collision
- [ ] fix bouncing