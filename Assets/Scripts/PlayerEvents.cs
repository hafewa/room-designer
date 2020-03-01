using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerEvents : MonoBehaviour
{
    #region Events

    public static UnityAction OnTouchPadUp = null;
    public static UnityAction OnTouchPadDown = null;
    public static UnityAction<Vector2> OnTouchPadTouchDown = null;
    public static UnityAction OnTouchPadTouchUp = null;
    public static UnityAction<Vector2> OnTouchPadTouch = null;
    public static UnityAction OnTriggerPressed = null;
    public static UnityAction<OVRInput.Controller, GameObject> OnControllerSource = null;

    #endregion

    #region Anchors

    public GameObject leftAnchor;
    public GameObject rightAnchor;
    public GameObject headAnchor;

    #endregion

    #region Input

    private Dictionary<OVRInput.Controller, GameObject> _controllerSets;
    private OVRInput.Controller _inputSource = OVRInput.Controller.None;
    private OVRInput.Controller _controller = OVRInput.Controller.None;
    private bool _inputActive = true;

    #endregion

    public Text debugText;

    private void Awake()
    {
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;

        _controllerSets = CreateControllerSets();
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    private void Update()
    {
        if (!_inputActive)
            return;

        CheckForController();

        CheckInputSource();

        InputEvents();
    }

    private void CheckForController()
    {
        OVRInput.Controller controllerCheck = _controller;

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            controllerCheck = OVRInput.Controller.LTrackedRemote;

        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.RTrackedRemote;

        // None, touchpad
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote) &&
            !OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.Touchpad;

        // Update
        _controller = UpdateController(controllerCheck, _controller);
    }

    private void CheckInputSource()
    {
        // Update
        _inputSource = UpdateController(OVRInput.GetActiveController(), _inputSource);
    }

    private void InputEvents()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            OnTouchPadDown?.Invoke();
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            OnTouchPadUp?.Invoke();
        }

        if (OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad))
        {
            Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            OnTouchPadTouchDown?.Invoke(input);
        }

        if (OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
        {
            OnTouchPadTouchUp?.Invoke();
        }

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            if (debugText != null)
            {
                debugText.text = input.ToString();
            }

            OnTouchPadTouch?.Invoke(input);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
            Input.GetMouseButtonDown(0))
        {
            Debug.Log("pressed trigger");
            OnTriggerPressed?.Invoke();
        }
    }

    private OVRInput.Controller UpdateController(OVRInput.Controller check, OVRInput.Controller previous)
    {
        if (check == previous)
            return previous;

        _controllerSets.TryGetValue(check, out var controllerObj);

        if (controllerObj == null)
            controllerObj = headAnchor;

        OnControllerSource?.Invoke(check, controllerObj);

        return check;
    }

    private void PlayerFound()
    {
        _inputActive = true;
    }

    private void PlayerLost()
    {
        _inputActive = false;
    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets()
    {
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>()
        {
            {OVRInput.Controller.LTrackedRemote, leftAnchor},
            {OVRInput.Controller.RTrackedRemote, rightAnchor},
            {OVRInput.Controller.Touchpad, headAnchor}
        };

        return newSets;
    }
}