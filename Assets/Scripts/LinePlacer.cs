using UnityEngine;
using UnityEngine.EventSystems;

public class LinePlacer : MonoBehaviour, IPointerClickHandler
{
    public GameObject line;
    public Transform wallsGroup;

    private LineRenderer _newLine;

    
    private void Update()
    {
        if (_newLine == null) return;
        var nearestPos = GridPointer.GetNearestPos();
        _newLine.SetPosition(1, nearestPos);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_newLine == null)
        {
            var startPos = GridPointer.GetNearestPos();

            DrawLine(startPos, startPos);
        }
        else
        {
            var wallObj = _newLine.gameObject.AddComponent<Wall>();
            Map.SetWall(wallObj.wallId, wallObj);

            _newLine = null;
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        _newLine = Instantiate(line).GetComponent<LineRenderer>();
        _newLine.transform.SetParent(wallsGroup);
        var positions = new[] {start, end};
        _newLine.SetPositions(positions);
    }
}