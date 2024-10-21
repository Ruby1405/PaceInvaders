using System.Text.Json;
using SFML.Graphics;
using SFML.Window;

namespace PaceInvaders;
public enum UserActions {
    MOVE_RIGHT,
    MOVE_LEFT,
    MOVE_UP,
    MOVE_DOWN,
    SHOOT,
    PAUSE
}
public static class InputManager {
    private const string KEYBINDINGS_PATH = "Config/KeyBindings.json";
    public static bool[] ActiveInputs { get; private set; } = new bool[Enum.GetValues(typeof(UserActions)).Length];
    public static bool[] InstantInputs { get; private set; } = new bool[Enum.GetValues(typeof(UserActions)).Length];
    private static Keyboard.Key[] inputKeys = [
                Keyboard.Key.D, // MOVE_RIGHT
                Keyboard.Key.A, // MOVE_LEFT
                Keyboard.Key.W, // MOVE_UP
                Keyboard.Key.S, // MOVE_DOWN
                Keyboard.Key.Space, // SHOOT
                Keyboard.Key.Escape // PAUSE
            ];
    private static string inputString = "";
    public static void InitializeInputs(RenderWindow window) {
        if (File.Exists(KEYBINDINGS_PATH)) inputKeys = LoadKeyBindings() ?? inputKeys;

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
    public static Keyboard.Key? GetKey(UserActions action) {
        if (inputKeys == null) return null;
        return inputKeys[(int)action];
    }
    private static void RecordKeyToString(object? _, KeyEventArgs e) =>
        inputString += e.Code.ToString();
    public static void StartStringInput(RenderWindow window) => 
        window.KeyPressed += RecordKeyToString;
    public static void EndStringInput(RenderWindow window)
    {
        window.KeyPressed -= RecordKeyToString;
        inputString = "";
    }
    public static string GetInputString() => inputString;
    private static void SaveKeyBindings(Keyboard.Key[] bindings) => 
        File.WriteAllText(KEYBINDINGS_PATH, JsonSerializer.Serialize(bindings));
    private static Keyboard.Key[]? LoadKeyBindings() =>
        JsonSerializer.Deserialize<Keyboard.Key[]>(File.ReadAllText(KEYBINDINGS_PATH));
}