using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Grid grid;
    public Transform reticuleTransform;
    private bool _onBoard;
    private static GameObject _selectPoint;

    private void Awake()
    {
        _selectPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _selectPoint.GetComponent<Collider>().enabled = false;
        _selectPoint.transform.localScale = new Vector3(5f, 5f, 5f);
    }

    private void Update()
    {
#if !UNITY_EDITOR
        if (!_onBoard) return;
#endif
        var nearestPos = grid.GetNearestPoint(reticuleTransform.position);
        _selectPoint.transform.position = nearestPos;
    }

    public static Vector3 GetNearestPos()
    {
        return _selectPoint.transform.position;
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
}