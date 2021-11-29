using System;

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override int GetHashCode()
    {
        return Int16.MaxValue * X + Y;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Point other)) return false;
        return GetHashCode() == other.GetHashCode();
    }
}
