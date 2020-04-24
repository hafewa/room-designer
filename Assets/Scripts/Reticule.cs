using System;
using cakeslice;
using UnityEngine;
using UnityEngine.Events;

public class Reticule : MonoBehaviour
{
    #region Actions

    public static UnityAction OnWallClicked;
    public static UnityAction<InteriorObject> OnMoveBtnClicked;
    public static UnityAction<InteriorObject> OnRotateBtnClicked;

    #endregion

    public GameObject editActions;
    
    private RaycastHit _currentHit;
    private Camera _cameraObj;
    private GameObject _hitObject;

    private InteriorObject _selectedObj;
    private Vector3 _selectedObjSize;

    private void Awake()
    {
        PlayerEvents.OnTriggerPressed += ProcessTriggerPressed;
        Pointer.OnPointerUpdate += CheckHit;

        _cameraObj = Camera.main;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTriggerPressed -= ProcessTriggerPressed;
        Pointer.OnPointerUpdate -= CheckHit;
    }

    private void Update()
    {
        if (!_selectedObj) return;
        
        editActions.transform.position = _selectedObj.transform.position + new Vector3(0, _selectedObjSize.y);
    }

    private void CheckHit(Vector3 point, RaycastHit hit)
    {
        // On blur
        if (_currentHit.collider && !Equals(hit, _currentHit))
        {
            switch (_currentHit.transform.tag)
            {
                case "Button":
                    var actionButton = _currentHit.transform.GetComponent<ActionButton>();
                    if (actionButton)
                    {
                        actionButton.OnPointerExit();
                    }

                    break;
                case "Selectable":
                    var interiorObject = _currentHit.transform.GetComponent<InteriorObject>();
                    interiorObject.OnBlur();
                    
                    break;
            }
        }

        // On hover
        if (hit.collider)
        {
            switch (hit.transform.tag)
            {
                case "Button":
                    var actionButton = hit.transform.GetComponent<ActionButton>();
                    if (actionButton)
                    {
                        actionButton.OnPointerEnter(hit.transform);
                    }

                    break;
                case "Selectable":
                    var interiorObject = hit.transform.GetComponent<InteriorObject>();
                    interiorObject.OnHover();

                    break;
            }
        }

        _currentHit = hit;

        UpdateSprite(point, hit);
    }

    private void UpdateSprite(Vector3 point, RaycastHit hit)
    {
        transform.position = point;
        if (hit.collider)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.back, hit.normal);
            var newPos = transform.position;
            newPos -= transform.forward * 0.001f;
            transform.position = newPos;
        }
        else
        {
            transform.LookAt(_cameraObj.gameObject.transform);
        }
    }

    private void ProcessTriggerPressed()
    {
        if (!_currentHit.collider) return;
        switch (_currentHit.transform.tag)
        {
            case "Button":
                var actionButton = _currentHit.transform.GetComponent<ActionButton>();
                if (!actionButton) break;
                
                actionButton.OnClick(_currentHit.transform);
                Debug.Log("pressed");
                switch (_currentHit.transform.name)
                {
                    case "Move":
                        Debug.Log("pressed Move");
                        OnMoveBtnClicked(_selectedObj);
                        
                        break;
                    case "Rotate":
                        Debug.Log("pressed Rotate");
                        OnRotateBtnClicked(_selectedObj);
                        
                        break;
                }

                break;
            case "Wall":
                OnWallClicked?.Invoke();

                break;
            case "Selectable":
                var interiorObject = _currentHit.transform.GetComponent<InteriorObject>();
                if (_selectedObj)
                {
                    if (_selectedObj.Equals(interiorObject))
                    {
                        _selectedObj.Deselect();
                        _selectedObj = null;
                    
                        editActions.SetActive(false);
                        break;
                    }
                    
                    _selectedObj.Deselect();
                }
                
                interiorObject.Select();

                _selectedObj = interiorObject;
                _selectedObjSize = _selectedObj.GetComponent<Collider>().bounds.size;
                editActions.SetActive(true);
                
                break;
        }
    }

    public RaycastHit GetCurrentHit()
    {
        return _currentHit;
    }
}