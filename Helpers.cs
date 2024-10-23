using SFML.Graphics;
using SFML.System;

namespace PaceInvaders;
public static class Helpers {
    /// <summary>
    /// Returns the length of a vector
    /// </summary>
    /// <param name="vector">the measured vector</param>
    /// <returns></returns>
    public static float Length(this Vector2f v) {
            return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
    }
    /// <summary>
    /// Returns the normalized vector
    /// </summary>
    /// <param name="vector">the vector to be normalized</param>
    /// <returns></returns>
    public static Vector2f Normalize(this Vector2f v) {
        if (v.X == 0 && v.Y == 0) return new Vector2f(0, 0);
        return v / v.Length();
    }
    /// <summary>
    /// Returns the Hadamard Product of two vectors
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector2f HadamardProduct(Vector2f a, Vector2f b) =>
        new Vector2f(a.X * b.X, a.Y * b.Y);
    //public static bool PointToRectCollision(Vector2f p, FloatRect r)
    //{
    //    if (
    //        p.X > 
    //        )
    //}
}