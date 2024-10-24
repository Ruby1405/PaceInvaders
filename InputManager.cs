using System.Text.Json;
using System.Text.RegularExpressions;
using SFML.Graphics;
using SFML.Window;

namespace PaceInvaders;
public enum UserActions {
    MOVE_RIGHT,
    MOVE_LEFT,
    MOVE_UP,
    MOVE_DOWN,
    SHOOT,
    PAUSE,
    SUBMIT
}
public static class InputManager {
    private const string KEYBINDINGS_PATH = "Config/KeyBindings.json";
    private static readonly Regex alphaNumericRegex = new("^[a-zA-Z0-9]*$");
    public static bool[] ActiveInputs { get; private set; } = new bool[Enum.GetValues(typeof(UserActions)).Length];
    public static bool[] InstantInputs { get; private set; } = new bool[Enum.GetValues(typeof(UserActions)).Length];
    private static Keyboard.Key[] inputKeys = [
                Keyboard.Key.D, // MOVE_RIGHT
                Keyboard.Key.A, // MOVE_LEFT
                Keyboard.Key.W, // MOVE_UP
                Keyboard.Key.S, // MOVE_DOWN
                Keyboard.Key.Space, // SHOOT
                Keyboard.Key.Escape, // PAUSE
                Keyboard.Key.Enter // SUBMIT
            ];
    private static string inputString = "";
    private static UserActions bindToBeChanged;
    private static RenderWindow window;
    public static void InitializeInputs(RenderWindow w) {
        if (File.Exists(KEYBINDINGS_PATH)) inputKeys = LoadKeyBindings() ?? inputKeys;

        window = w;

        // Set up the event handlers
        window.KeyPressed += (sender, e) => {
            for (int i = 0; i < inputKeys.Length; i++)
            {
                if (e.Code == inputKeys[i])
                {
                    InstantInputs[i] = true;
                    ActiveInputs[i] = true;
                }
            }
        };

        window.KeyReleased += (sender, e) => {
            for (int i = 0; i < inputKeys.Length; i++)
            {
                if (e.Code == inputKeys[i]) ActiveInputs[i] = false;
            }
        };
    }
    public static void Update() {
        for (int i = 0; i < InstantInputs.Length; i++) InstantInputs[i] = false;
    }
    public static string GetKey(UserActions action) => inputKeys[(int)action].ToString();
    private static void RecordTextInput(object? _, TextEventArgs e)
    {
        if (e.Unicode == "\b")
        {
            if (inputString.Length > 0)
                inputString = inputString[..(inputString.Length - 1)];
            return;
        }
        if (alphaNumericRegex.IsMatch(e.Unicode)) inputString += e.Unicode;
    }
    public static void StartTextInput()
    {
        window.TextEntered += RecordTextInput;
        inputString = "";
    }
    public static void EndTextInput() => 
        window.TextEntered -= RecordTextInput;
    public static string GetInputString() => inputString;
    private static void SaveKeyBindings() => 
        File.WriteAllText(KEYBINDINGS_PATH, JsonSerializer.Serialize(inputKeys));
    public static void ChangeKeyBind(UserActions action)
    {
        bindToBeChanged = action;
        window.KeyPressed += OnKeySetBind;
    }
    private static void OnKeySetBind(object? _, KeyEventArgs e) 
    {
        inputKeys[(int)bindToBeChanged] = e.Code;
        window.KeyPressed -= OnKeySetBind;
        SaveKeyBindings();
    }
    private static Keyboard.Key[]? LoadKeyBindings() =>
        JsonSerializer.Deserialize<Keyboard.Key[]>(File.ReadAllText(KEYBINDINGS_PATH));
}