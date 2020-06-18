using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static UnityAction<Wall> OnWallClicked;
    
    [HideInInspector] public int wallId;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 eulerAngles;
    [HideInInspector] public float length;
    [HideInInspector] public List<HoleBase> holes = new List<HoleBase>();

    private LineRenderer _line;
    private Outline _outline;

    private void Awake()
    {
        _outline = gameObject.AddComponent<Outline>();
        _line = GetComponent<LineRenderer>();

        InitProps();
    }

    private void Start()
    {
        AddColliderToLine(_line.GetPosition(0), _line.GetPosition(1));
    }

    private void InitProps()
    {
        var a = _line.GetPosition(0);
        var b = _line.GetPosition(1);
        // Calculate middle position for wall in 3D
        var midPoint = new Vector3((a.y + b.y) / 2, 0, (a.x + b.x) / 2);
        var lineVector = ((b - a) / 25) * 10;

        var angle = Vector3.SignedAngle(Vector3.right, lineVector, Vector3.forward);
        wallId = GetInstanceID();
        position = (midPoint / 25) * 10;
        eulerAngles = new Vector3(0, angle - 90);
        length = lineVector.magnitude;
    }

    private void AddColliderToLine(Vector3 start, Vector3 end)
    {
        if (start == end) return;

        var startPos = start;
        var endPos = end;
        var col = new GameObject("Collider").AddComponent<BoxCollider>();
        col.transform.parent = _line.transform;
        var lineLength = Vector3.Distance(startPos, endPos);
        col.size = new Vector3(lineLength, 0.175f, 0.25f);
        var midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint;
        var angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }

        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);
        col.transform.localScale = new Vector3(1, _line.endWidth * 5, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnWallClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        _outline.color = 0;
        _outline.enabled = true;
#endif
    }

    public void OnPointerExit(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        ResetSelect();
#endif
    }

    public void ResetSelect()
    {
        _outline.enabled = false;
    }

    public void AddNewHole(HoleBase newHole)
    {
        if (holes.Count == 0)
        {
            holes.Add(newHole);
            return;
        }
        
        var insertIndex = -1;
        for (var i = 0; i < holes.Count; i++)
        {
            if (newHole.startDistance >= holes[i].startDistance) continue;
            
            insertIndex = i;
            break;
        }
        
        if (insertIndex == -1)
        {
            holes.Add(newHole);
            return;
        }
        
        holes.Insert(insertIndex, newHole);
    }
}