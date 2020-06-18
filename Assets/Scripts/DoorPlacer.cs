using UnityEngine;

public class DoorPlacer : MonoBehaviour
{
    public GameObject line;
    public Transform reticule;

    private LineRenderer _newLine;


    private void Awake()
    {
        Wall.OnWallClicked += PlaceDoor;
    }

    private void Update()
    {
        if (_newLine == null) return;
        // var nearestPos = GridPointer.GetNearestPos();
        _newLine.SetPosition(1, reticule.position);
    }

    private void PlaceDoor(Wall wall)
    {
        if (ToolsHandler.CurrentTool != ToolsHandler.Tool.DrawDoor) return;
        
        if (_newLine == null)
        {
            var startPos = reticule.position;

            DrawLine(startPos, startPos, wall.transform);
        }
        else if (_newLine.transform.parent == wall.transform)
        {
            _newLine.gameObject.AddComponent<Window>();

            _newLine = null;
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Transform parent)
    {
        _newLine = Instantiate(line).GetComponent<LineRenderer>();
        _newLine.transform.SetParent(parent);
        _newLine.sortingOrder = 2;
        var positions = new[] {start, end};
        _newLine.SetPositions(positions);
        _newLine.startColor = _newLine.endColor = Color.yellow;
        _newLine.startWidth = _newLine.endWidth = 10;
    }
}