using SFML.System;

namespace PaceInvaders;
public static class Helpers {
    /// <summary>
    /// Returns the length of a vector
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float Length(this Vector2f v) {
            return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
    }
    /// <summary>
    /// Returns the normalized vector
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2f Normalize(this Vector2f v) {
        return v / v.Length();
    }
}