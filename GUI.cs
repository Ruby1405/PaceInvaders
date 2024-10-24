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
    private bool enteringName = false;
    private List<(int, string)> highScores = [];
    private const string HIGHSCORES_PATH = "Config/highscores.txt";
    private const int HIGHSCORES_DISPLAY_COUNT = 20;
    private int playerRank;
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

        EventManager.GameLost += OnGameLost;
    }
    private void OnGameLost()
    {
        InputManager.StartTextInput();
        enteringName = true;
        LoadHighScores();
        if (highScores.Count == 0)
        {
            playerRank = 0;
            return;
        }
        for (int i = 0; i < highScores.Count; i++)
        {
            if (Scene.Score > highScores[i].Item1 &&
                (i == 0 || Scene.Score < highScores[i - 1].Item1))
            {
                playerRank = i;
                return;
            }
        }
        playerRank = highScores.Count;
    }
    private void LoadHighScores()
    {
        highScores.Clear();
        if (File.Exists(HIGHSCORES_PATH))
        {
            File.ReadAllLines(HIGHSCORES_PATH, Encoding.UTF8).ToList().ForEach(e =>
            {
                string[] entry = e.Split(':');
                int score;
                if (entry.Length != 2)
                    Console.WriteLine($"Failed to read highscore; \"{entry}\" is invalid format");
                else if (!int.TryParse(entry[0], out score))
                    Console.WriteLine($"Failed to read highscore; \"{entry}\" is not a number");
                else
                {
                    highScores.Add(new(score,entry[1]));
                }
            });
        }
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
                            Scene.State = State.HIGHSCORES;
                            LoadHighScores();
                            optionSelection = 0;
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
            case State.HIGHSCORES:
                if (highScores.Count > HIGHSCORES_DISPLAY_COUNT)
                {
                    if (InputManager.InstantInputs[(int)UserActions.MOVE_DOWN] &&
                        optionSelection < highScores.Count-HIGHSCORES_DISPLAY_COUNT) optionSelection ++; 
                    if (InputManager.InstantInputs[(int)UserActions.MOVE_UP] &&
                        optionSelection != 0) optionSelection --;
                }
                if (InputManager.InstantInputs[(int)UserActions.PAUSE]) Scene.State = State.MENU;
                break;
            case State.GAME_OVER:
                if (InputManager.InstantInputs[(int)UserActions.SUBMIT] && enteringName)
                {
                    InputManager.EndTextInput();
                    enteringName = false;
                    (int, string) entry = new(Scene.Score, InputManager.GetInputString());
                    if (entry.Item2 == "") entry.Item2 = " ";
                    if (playerRank == highScores.Count) highScores.Add(entry);
                    else highScores.Insert(playerRank, entry);
                    if (highScores.Count > 100) highScores.RemoveRange(100, highScores.Count -100);
                    string fileString = "";
                    highScores.ForEach(e => fileString += $"{e.Item1}:{e.Item2}\n");
                    File.WriteAllText(HIGHSCORES_PATH, fileString);
                }
                if (InputManager.InstantInputs[(int)UserActions.PAUSE]) Scene.State = State.HIGHSCORES;
                break;
        }
    }
    public void Render(RenderWindow window) {
        switch (Scene.State)
        {
            case State.PLAY:
                window.Draw(ui);
                break;
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
                for (int i = 0; i < Math.Min(HIGHSCORES_DISPLAY_COUNT,highScores.Count); i++)
                {
                    text.DisplayedString = $"#{i + 1 + optionSelection}: {highScores[i +
                        optionSelection].Item1} {highScores[i + optionSelection].Item2}";
                    text.Position = new Vector2f(50,20 + i * (text.CharacterSize + 10));
                    text.FillColor = Color.White;
                    window.Draw(text);
                }
                break;
            case State.GAME_OVER:
                text.DisplayedString = "You died";
                text.Position = new Vector2f(50,220);
                text.FillColor = Color.Red;
                window.Draw(text);
                text.DisplayedString = $"Your rank: {playerRank+1}";
                text.Position = new Vector2f(50,260);
                text.FillColor = Color.White;
                window.Draw(text);
                text.DisplayedString = $"Your score: {Scene.Score}";
                text.Position = new Vector2f(50,300);
                text.FillColor = Color.White;
                window.Draw(text);
                text.DisplayedString = $"Enter a name: {InputManager.GetInputString()}";
                text.Position = new Vector2f(50,340);
                window.Draw(text);

                // TODO go back to main menu or highscore
                break;

        }
    }
    public void GetHighScore()
    {
        //File.ReadLines
    }
}