using System.Linq;
using UnityEngine;

public class Window : MonoBehaviour
{
    [HideInInspector] public float startDistance;
    [HideInInspector] public float endDistance;
    [HideInInspector] public float length;

    private LineRenderer _wall;
    private LineRenderer _line;

    private void Awake()
    {
        _wall = transform.parent.GetComponent<LineRenderer>();
        _line = GetComponent<LineRenderer>();

        InitProps();
        
        _wall.GetComponent<Wall>().windows.Add(this);
    }

    private void InitProps()
    {
        var windowStartPos = _line.GetPosition(0);
        var windowEndPos = _line.GetPosition(1);

        var wallStartWindowStartDist = Vector3.Distance(_wall.GetPosition(0), windowStartPos);
        var wallStartWindowEndDist = Vector3.Distance(_wall.GetPosition(0), windowEndPos);
        if (wallStartWindowStartDist < wallStartWindowEndDist)
        {
            startDistance = wallStartWindowStartDist;
            endDistance = wallStartWindowEndDist;
        }
        else
        {
            startDistance = wallStartWindowEndDist;
            endDistance = wallStartWindowStartDist;
        }
        startDistance = startDistance / 25 * 10;
        endDistance = endDistance / 25 * 10;

        length = Vector3.Distance(windowStartPos, windowEndPos);
    }
}