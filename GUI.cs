using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;
class GUI {
    private int menuSelection = 0;
    private readonly Font font;
    private readonly Text ui;
    public GUI() {
        font = new Font("Assets/pixel-font.ttf");

        ui = new(){
            Font = font,
            CharacterSize = 30,
            Position = new Vector2f(10,10)
        };
    }
    public void Update() {
        ui.DisplayedString = Scene.Score + "\n" + Scene.Health;
    }
    public void Render(RenderWindow window) {
        window.Draw(ui);
    }
}