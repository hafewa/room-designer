using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinePlacer : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public Grid grid;
    public GameObject line;
    public Transform reticuleTransform;
    public Transform wallsGroup;

    private LineRenderer _newLine;
    private bool _onBoard;
    private Vector3 _startPoint;
    private GameObject _selectPoint;

    private void Awake()
    {
        _selectPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _selectPoint.transform.localScale = new Vector3(5f, 5f, 5f);
    }

    private void Update()
    {
#if !UNITY_EDITOR
        if (!_onBoard) return;
#endif
        var nearestPos = grid.GetNearestPoint(reticuleTransform.position);
        _selectPoint.transform.position = nearestPos;

        if (_startPoint == Vector3.zero) return;

        _newLine.SetPosition(1, nearestPos);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_startPoint == Vector3.zero)
        {
            _startPoint = grid.GetNearestPoint(reticuleTransform.position);

            DrawLine(_startPoint, _startPoint);
        }
        else
        {
            var newWallInfo = new WallInfo(_newLine.GetComponent<LineRenderer>());
            Map.SetWall(newWallInfo.Name, newWallInfo);
            
            _startPoint = Vector3.zero;
            _newLine = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onBoard = false;
        _selectPoint.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onBoard = true;
        _selectPoint.SetActive(true);
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        _newLine = Instantiate(line).GetComponent<LineRenderer>();
        _newLine.transform.SetParent(wallsGroup);
        var positions = new[] {start, end};
        _newLine.SetPositions(positions);
    }
}