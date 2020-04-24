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

    #region RotateProps

    private Vector3 _startEuler;
    private const float RotateSpeed = 350f;

    #endregion

    private GameObject _reticule;    

    private void Awake()
    {
        PlayerEvents.OnTouchPadTouchUp += SetStartRotation;
    }
    
    private void OnDestroy()
    {
        PlayerEvents.OnTouchPadTouchUp -= SetStartRotation;
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

                break;

            default:
                return;
        }
    }

    private void MoveToPointer()
    {
        transform.SetPositionAndRotation(_reticule.transform.position, _reticule.transform.rotation);
        transform.Rotate(Vector3.right, -90);
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

    private void SetStartRotation()
    {
        if (_state != State.Rotating) return;
        _startEuler = transform.localEulerAngles;
    }

    private void RotateObj()
    {
        if (PlayerEvents.TouchPos.Equals(PlayerEvents.StartTouchPos)) return;
        
        var touchDiff = PlayerEvents.TouchPos - PlayerEvents.StartTouchPos;
        var newYAngle = _startEuler.y + touchDiff.x * RotateSpeed * Time.deltaTime;
        
        var localEulerAngles = transform.localEulerAngles;
        localEulerAngles = new Vector3(localEulerAngles.x, newYAngle, localEulerAngles.z);
        transform.localEulerAngles = localEulerAngles;
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