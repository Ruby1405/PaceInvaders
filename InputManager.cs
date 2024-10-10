using System.Text.Json;
using SFML.Graphics;
using SFML.Window;

namespace PaceInvaders;
enum UserActions {
    MOVE_RIGHT,
    MOVE_LEFT,
    MOVE_UP,
    MOVE_DOWN,
    SHOOT,
    PAUSE
}
class InputManager {
    private const string KEYBINDINGS_PATH = "Config/KeyBindings.json";
    public static bool[] inputActions = new bool[6];
    private static Keyboard.Key[]? inputKeys;
    public InputManager(RenderWindow window) {
        if (File.Exists(KEYBINDINGS_PATH)) inputKeys = LoadKeyBindings();
        if (inputKeys == null)
        {
            inputKeys = [
                Keyboard.Key.Right, // MOVE_RIGHT
                Keyboard.Key.Left, // MOVE_LEFT
                Keyboard.Key.Up, // MOVE_UP
                Keyboard.Key.Down, // MOVE_DOWN
                Keyboard.Key.Space, // SHOOT
                Keyboard.Key.Escape // PAUSE
            ];
        }

        // Set up the event handlers
        window.KeyPressed += (sender, e) => {
            for (int i = 0; i < inputKeys.Length; i++)
            {
                if (e.Code == inputKeys[i]) inputActions[i] = true;
            }
        };
        window.KeyReleased += (sender, e) => {
            for (int i = 0; i < inputKeys.Length; i++)
            {
                if (e.Code == inputKeys[i]) inputActions[i] = false;
            }
        };
    }
    public static Keyboard.Key? GetKey(UserActions action) {
        if (inputKeys == null) return null;
        return inputKeys[(int)action];
    }
    private static void SaveKeyBindings(Keyboard.Key[] a) => 
        File.WriteAllText(KEYBINDINGS_PATH, JsonSerializer.Serialize(a));
    private static Keyboard.Key[]? LoadKeyBindings() =>
        JsonSerializer.Deserialize<Keyboard.Key[]>(File.ReadAllText(KEYBINDINGS_PATH));
}