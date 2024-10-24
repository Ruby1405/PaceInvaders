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
        new (a.X * b.X, a.Y * b.Y);
    /// <summary>
    /// Returns whether two rectangles overlap
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool RectToRectCollision(FloatRect a, FloatRect b)
    {
        // Minkowski difference collision
        Vector2f midPoint = new(
            MathF.Abs(b.Left - a.Left + 0.5f * (b.Width -a.Width)), 
            MathF.Abs(b.Top - a.Top + 0.5f * (b.Height -a.Height))
        );
        Vector2f halfSizeSum = 0.5f * new Vector2f(a.Width + b.Width, a.Height + b.Height);
        return MathF.Max((midPoint - halfSizeSum).X, (midPoint - halfSizeSum).Y) < 0;
    }
    /// <summary>
    /// Returns whether a point is within a rectangle
    /// </summary>
    /// <param name="p"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static bool PointToRectCollision(Vector2f p, FloatRect r)
    {
        Vector2f distanceToCenter = p - new Vector2f(r.Left + 0.5f * r.Width, r.Top + 0.5f * r.Height);
        return MathF.Abs(distanceToCenter.X) <= 0.5f * r.Width && MathF.Abs(distanceToCenter.Y) <= 0.5f * r.Height;
    }
}