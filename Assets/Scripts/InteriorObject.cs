using System;
using System.Configuration;
using System.Text.RegularExpressions;
using cakeslice;
using UnityEngine;

public class InteriorObject : MonoBehaviour
{
    #region State

    private enum State
    {
        Idle,
        Moving,
        Rotating,
        Scaling
    };

    private State _state = State.Idle;
    private bool _selected = false;

    #endregion

    #region EditProps

    private Vector3 _startEuler;
    private Vector3 _startScale;
    private const float RotateSpeed = 20f;
    private const float ScaleSpeed = 0.2f;

    #endregion

    private GameObject _reticule;    

    private void Awake()
    {
        PlayerEvents.OnTouchPadTouchDown += SetStartTransform;
    }
    
    private void OnDestroy()
    {
        PlayerEvents.OnTouchPadTouchDown -= SetStartTransform;
    }

    private void Update()
    {
        if (!_selected) return;

        switch (_state)
        {
            case State.Moving:
                MoveToPointer();

                break;

            case State.Rotating:
                RotateObj();

                break;

            case State.Scaling:
                ScaleObj();
                
                break;

            default:
                return;
        }
    }

    private void MoveToPointer()
    {
        transform.SetPositionAndRotation(_reticule.transform.position, _reticule.transform.rotation);
        transform.Rotate(Vector3.right, 90);
    }

    public void StartMoving(GameObject reticule)
    {
        GetComponent<BoxCollider>().enabled = false;
        _reticule = reticule;
        _state = State.Moving;
    }

    public void StartRotating()
    {
        _state = State.Rotating;
    }

    public void StartScaling()
    {
        _state = State.Scaling;
    }

    public void StopAnything()
    {
        GetComponent<BoxCollider>().enabled = true;
        _state = State.Idle;
    }

    private void SetStartTransform()
    {
        if (_state != State.Rotating && _state != State.Scaling) return;
        _startScale = transform.localScale;
        _startEuler = transform.localEulerAngles;
    }

    private void RotateObj()
    {
        if (PlayerEvents.TouchPos.Equals(PlayerEvents.StartTouchPos)) return;
        
        var touchDiff = PlayerEvents.TouchPos - PlayerEvents.StartTouchPos;
        var newYAngle = _startEuler.y + touchDiff.x * RotateSpeed;
        
        var localEulerAngles = transform.localEulerAngles;
        localEulerAngles = new Vector3(localEulerAngles.x, newYAngle, localEulerAngles.z);
        transform.localEulerAngles = localEulerAngles;
    }
    
    private void ScaleObj()
    {
        if (PlayerEvents.TouchPos.Equals(PlayerEvents.StartTouchPos)) return;
        
        var touchDiff = PlayerEvents.TouchPos - PlayerEvents.StartTouchPos;
        var newScale = _startScale.y + touchDiff.y * ScaleSpeed;
        
        var localScale = new Vector3(newScale, newScale, newScale);
        transform.localScale = localScale;
    }
    
    private void UpdateOutline(bool enable, int color)
    {
        foreach (Transform mesh in transform)
        {
            var objOutline = mesh.GetComponent<Outline>();
            objOutline.color = color;
            objOutline.enabled = enable;
        }
    }

    public void OnHover()
    {
        if (_selected) return;
        
        UpdateOutline(true, 2);
    }

    public void OnBlur()
    {
        if (_selected) return;
        
        UpdateOutline(false, 2);
    }
    
    public void Select()
    {
        if (_selected) return;
        
        _selected = true;
        
        UpdateOutline(true, 0);
    }
    
    public void Deselect()
    {
        _selected = false;
        
        UpdateOutline(false, 0);
    }
}