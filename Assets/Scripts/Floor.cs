using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Floor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static UnityAction<Floor> OnFloorClicked;
    
    [HideInInspector] public int floorId;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 size;
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        InitProps();
    }

    private void InitProps()
    {
        floorId = GetInstanceID();
        
        // Find position for the floor in 3d space
        var initPosition = transform.position;
        var rect = GetComponent<RectTransform>().rect;
        position = new Vector3(initPosition.y - rect.height / 2, 0, -(initPosition.x + rect.width / 2)) / 25 * 10;

        size = new Vector3(rect.width, 1, rect.height) / 25;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnFloorClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        _image.color = Color.green;
#endif
    }

    public void OnPointerExit(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        ResetSelect();
#endif
    }

    private void ResetSelect()
    {
        _image.color = new Color(255, 255, 255, 127);
    }
}