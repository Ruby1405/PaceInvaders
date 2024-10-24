using System.Text;
using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;
sealed class GUI {
    private int optionSelection = 0;
    private readonly Font font;
    private readonly Text ui;
    private readonly Text text;
    private readonly Vector2f OPTION_POSITION = new (50,200);
    private static readonly string[] MENU_OPTIONS = [
        "New Game",
        "Highscores",
        "Settings",
        "Quit"
    ];
    private static readonly string[] PAUSE_OPTIONS = [
        "Resume",
        "Highscores",
        "Settings",
        "Main menu"
    ];
    private bool showHighScores = false;
    public GUI() {
        font = new Font("Assets/pixel-font.ttf");

        ui = new(){
            Font = font,
            CharacterSize = 30,
            Position = new Vector2f(10,10)
        };

        text = new(){
            Font = font,
            CharacterSize = 30,
            Position = OPTION_POSITION
        };
    }
    public void Update() {
        switch (Scene.State)
        {
            case State.PAUSE or State.MENU:
                string[] options = Scene.State == State.MENU? MENU_OPTIONS : PAUSE_OPTIONS;

                if (InputManager.InstantInputs[(int)UserActions.MOVE_DOWN]) optionSelection ++; 
                if (InputManager.InstantInputs[(int)UserActions.MOVE_UP]) optionSelection --;

                optionSelection += options.Length;
                optionSelection %= options.Length;

                if (InputManager.InstantInputs[(int)UserActions.PAUSE] && Scene.State == State.PAUSE) Scene.State = State.PLAY;

                if (InputManager.InstantInputs[(int)UserActions.SHOOT])
                    switch (options[optionSelection])
                    {
                        case "New Game":
                            Scene.State = State.PLAY;
                            Scene.NewGame();
                            break;
                        case "Resume":
                            Scene.State = State.PLAY;
                            break;
                        case "Highscores":
                            InputManager.StartTextInput();
                            Scene.State = State.HIGHSCORES;
                            break;
                        case "Settings":
                            break;
                        case "Quit":
                            Scene.State = State.QUIT;
                            break;
                        case "Main menu":
                            Scene.State = State.MENU;
                            optionSelection = 0;
                            break;
                    }
                break;
            case State.PLAY:
                if (InputManager.InstantInputs[(int)UserActions.PAUSE]) Scene.State = State.PAUSE;
                ui.DisplayedString = Scene.Score + "\n" + Scene.Health;
                break;
            case State.GAME_OVER:
                if (InputManager.InstantInputs[(int)UserActions.SUBMIT]) InputManager.EndTextInput();
                break;
        }
    }
    public void Render(RenderWindow window) {
        switch (Scene.State)
        {
            case State.MENU or State.PAUSE:
                string[] options = Scene.State == State.MENU? MENU_OPTIONS : PAUSE_OPTIONS;

                for (int i = 0; i < options.Length; i++)
                {
                    text.DisplayedString = options[i];
                    text.Position = OPTION_POSITION + new Vector2f(0,i * (text.CharacterSize + 20));
                    text.FillColor = optionSelection == i? Color.Cyan : Color.White;
                    window.Draw(text);
                }
                break;
            case State.HIGHSCORES:
                // text.DisplayedString = InputManager.GetInputString();
                // text.Position = OPTION_POSITION + new Vector2f(0,20);
                // text.FillColor = Color.White;
                // window.Draw(text);
                break;
            case State.GAME_OVER:
                text.DisplayedString = "You died";
                text.Position = new Vector2f(50,220);
                text.FillColor = Color.Red;
                window.Draw(text);
                text.DisplayedString = $"Your score: {Scene.Score}";
                text.Position = new Vector2f(50,260);
                text.FillColor = Color.White;
                window.Draw(text);
                text.DisplayedString = $"Enter a name: {InputManager.GetInputString()}";
                text.Position = new Vector2f(50,300);
                window.Draw(text);

                // TODO go back to main menu or highscor
                break;

        }
        window.Draw(ui);
    }
    public void GetHighScore()
    {
        //File.ReadLines
    }
}