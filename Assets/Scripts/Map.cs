using System.Collections.Generic;


public static class Map
{
    public static float CellSize { get; set; } = 1;
    public static float WallHeight { get; set; } = 30;

    private static readonly Dictionary<int, Wall> Walls = new Dictionary<int, Wall>();
    private static readonly Dictionary<int, Floor> Floors = new Dictionary<int, Floor>();

    public static void SetWall(int key, Wall value)
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

    public static Wall GetWall(int key)
    {
        return !Walls.ContainsKey(key) ? null : Walls[key];
    }

    public static void DeleteWall(int key)
    {
        Walls.Remove(key);
    }

    public static Dictionary<int, Wall> GetWalls()
    {
        return Walls;
    }

    public static void ResetWallSelect()
    {
        foreach (var wall in Walls)
        {
            wall.Value.ResetSelect();
        }
    }
    
    public static void SetFloor(int key, Floor value)
    {
        if (Floors.ContainsKey(key))
        {
            Floors[key] = value;
        }
        else
        {
            Floors.Add(key, value);
        }
    }

    public static Floor GetFloor(int key)
    {
        return !Floors.ContainsKey(key) ? null : Floors[key];
    }

    public static void DeleteFloor(int key)
    {
        Floors.Remove(key);
    }

    public static Dictionary<int, Floor> GetFloors()
    {
        return Floors;
    }
}