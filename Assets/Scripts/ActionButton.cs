using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    private Button _buttonComp;
    private IButtonAction _buttonAction;

    private void Awake()
    {
        _buttonComp = GetComponent<Button>();
        _buttonAction = GetComponent<IButtonAction>();
    }

    public void OnPointerEnter(Transform btn)
    {
        if (_buttonAction.OnHover(btn)) return;
        
        _buttonComp.OnPointerEnter(new PointerEventData(EventSystem.current));
    }

    public void OnPointerExit()
    {
        _buttonComp.OnPointerExit(new PointerEventData(EventSystem.current));
    }

    public void OnClick(Transform btn)
    {
        _buttonAction.OnClick(btn);
    }
}
