using UnityEngine;
using UnityEngine.EventSystems;

public class FloorPlacer : MonoBehaviour, IPointerClickHandler
{
    public GameObject floor;
    public Transform floorsGroup;

    private RectTransform _newFloor;


    private void Update()
    {
        if (_newFloor == null) return;
        var nearestPos = GridPointer.GetNearestPos();
        UpdateFloorSize(nearestPos);
    }

    private void UpdateFloorSize(Vector3 pointerPos)
    {
        var newSize = pointerPos - _newFloor.position;
        var xScale = newSize.x < 0 ? -1 : 1;
        var yScale = newSize.y < 0 ? 1 : -1;
        
        _newFloor.sizeDelta = new Vector2(Mathf.Abs(newSize.x), Mathf.Abs(-newSize.y));
        _newFloor.localScale = new Vector3(xScale, yScale, _newFloor.localScale.z);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_newFloor == null)
        {
            var startPos = GridPointer.GetNearestPos();

            DrawFloor(startPos);
        }
        else
        {
            var floorObj = _newFloor.gameObject.AddComponent<Floor>();
            Map.SetFloor(floorObj.floorId, floorObj);

            _newFloor = null;
        }
    }

    private void DrawFloor(Vector3 start)
    {
        _newFloor = Instantiate(floor).GetComponent<RectTransform>();
        _newFloor.SetParent(floorsGroup);

        _newFloor.position = start;
    }
}