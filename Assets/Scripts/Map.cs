using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct WallInfo
{
    public string Name;
    public Vector3 Position;
    public Vector3 EulerAngles;
    public float Length;
    public float Height;

    public WallInfo(LineRenderer line)
    {
        var a = line.GetPosition(0);
        var b = line.GetPosition(1);
        var midPoint = new Vector3((a.y + b.y) / 2, 0, (a.x + b.x) / 2);
        var lineVector = (b - a) / 25 * 10;
        
        var angle = Vector3.SignedAngle(Vector3.right, lineVector, Vector3.forward);
        Debug.Log(angle);
        Name = a.ToString() + b;
        Position = midPoint / 25 * 10;
        Debug.Log(Position);
        EulerAngles = new Vector3(0, angle - 90);
        Length = lineVector.magnitude;
        Height = 30;
    }
}

public static class Map
{
    private static readonly Dictionary<string, WallInfo> Walls = new Dictionary<string, WallInfo>();

    public static void SetWall(string key, WallInfo value)
    {
        if (Walls.ContainsKey(key))
        {
            Walls[key] = value;
        }
        else
        {
            Walls.Add(key, value);
        }
    }

    public static WallInfo? GetWall(string key)
    {
        if (!Walls.ContainsKey(key)) return null;

        return Walls[key];
    }

    public static Dictionary<string, WallInfo> GetWalls()
    {
        return Walls;
    }
}