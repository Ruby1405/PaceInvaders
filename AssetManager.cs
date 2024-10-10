using System.Reflection.Metadata;
using SFML.Audio;
using SFML.Graphics;

namespace PaceInvaders;

class AssetManager {
    private const string ASSET_PATH = "Assets/";
    private readonly Dictionary<string, Texture> textures;
    private readonly Dictionary<string, Font> fonts;
    private readonly Dictionary<string, Music> music;
    private readonly Dictionary<string, SoundBuffer> soundBuffers;
    public AssetManager() {
        textures = new Dictionary<string, Texture>();
        fonts = new Dictionary<string, Font>();
        music = new Dictionary<string, Music>();
        soundBuffers = new Dictionary<string, SoundBuffer>();
    }
    public static Texture LoadTexture(string filename) {
        return new Texture(filename);
    }
    public static Font LoadFont(string filename) {
        return new Font(filename);
    }
    public static Music LoadMusic(string filename) {
        return new Music(filename);
    }
    public static SoundBuffer LoadSoundBuffer(string filename) {
        return new SoundBuffer(filename);
    }
}