using System;
using UnityEngine;
using UnityEngine.Events;

public class Reticule : MonoBehaviour
{
    #region Actions
    public static UnityAction OnWallClicked;
    #endregion

    private RaycastHit _currentHit;
    private Camera _cameraObj;
    private GameObject _hitObject;

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

    private void CheckHit(Vector3 point, RaycastHit hit)
    {
        if (hit.collider)
        {
            if (_currentHit.collider)
            {
                if (!Equals(hit, _currentHit))
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
                    }
                }
            }
            
            switch (hit.transform.tag)
            {
                case "Button":
                    var actionButton = hit.transform.GetComponent<ActionButton>();
                    if (actionButton)
                    {
                        actionButton.OnPointerEnter(hit.transform);
                    }

                    break;
            }

            _currentHit = hit;
        }
        else
        {
            _currentHit = new RaycastHit();
        }

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
                if (actionButton)
                {
                    actionButton.OnClick(_currentHit.transform);
                }

                break;
            case "Wall":
                OnWallClicked?.Invoke();
                
                break;
        }
    }

    public RaycastHit GetCurrentHit()
    {
        return _currentHit;
    }
}