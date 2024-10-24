using SFML.Audio;
using SFML.Graphics;
namespace PaceInvaders;
public static class AssetManager {
    private const string ASSET_PATH = "Assets/";
    private static readonly Dictionary<string, Texture> textures = [];
    private static readonly Dictionary<string, Font> fonts = [];
    private static readonly Dictionary<string, Music> musics = [];
    private static readonly Dictionary<string, SoundBuffer> soundBuffers = [];
    public static Texture LoadTexture(string filename) {
        if (textures.TryGetValue(filename, out Texture found)) return found;
        Texture texture = new($"{ASSET_PATH}{filename}.png");
        textures.Add(filename, texture);
        return texture;
    }
    public static Font LoadFont(string filename) {
        if (fonts.TryGetValue(filename, out Font found)) return found;
        Font font = new(filename);
        fonts.Add(filename, font);
        return font;
    }
    public static Music LoadMusic(string filename) {
        if (musics.TryGetValue(filename, out Music found)) return found;
        Music music = new(ASSET_PATH + filename);
        musics.Add(filename, music);
        return music;
    }
    public static SoundBuffer LoadSoundBuffer(string filename) {
        if (soundBuffers.TryGetValue(filename, out SoundBuffer found)) return found;
        SoundBuffer soundBuffer = new(filename);
        soundBuffers.Add(filename, soundBuffer);
        return soundBuffer;
    }
}