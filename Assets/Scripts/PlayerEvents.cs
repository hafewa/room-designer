using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerEvents : MonoBehaviour
{
    #region Events

    public static UnityAction OnTouchPadUp = null;
    public static UnityAction OnTouchPadDown = null;
    public static UnityAction OnTouchPadTouchDown = null;
    public static UnityAction OnTouchPadTouchUp = null;
    public static UnityAction OnTriggerPressed = null;
    public static UnityAction OnBackPressed = null;
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

    public static Vector2 TouchPos;
    public static Vector2 StartTouchPos;

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
        OVRInput.Controller controllerCheck;

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            controllerCheck = OVRInput.Controller.LTrackedRemote;
        else if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.RTrackedRemote;
        // None, touchpad
        else
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
        // Touchpad button pressed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            OnTouchPadDown?.Invoke();
        }

        // Touchpad button released
        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            OnTouchPadUp?.Invoke();
        }

        // Touchpad touched
        if (OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad))
        {
            StartTouchPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

            OnTouchPadTouchDown?.Invoke();
        }

        // Touchpad released
        if (OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
        {
            StartTouchPos.Set(0, 0);
            TouchPos.Set(0, 0);

            OnTouchPadTouchUp?.Invoke();
        }

        // Touchpad is touching
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            TouchPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        }

        // Trigger button pressed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
            Input.GetMouseButtonDown(0))
        {
            OnTriggerPressed?.Invoke();
        }
        
        if (OVRInput.GetDown(OVRInput.Button.Back) ||
            Input.GetMouseButtonDown(1))
        {
            OnBackPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            TouchPos = new Vector2(0, 1);

            OnTouchPadTouchDown?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            TouchPos = new Vector2(0, 0);

            OnTouchPadTouchUp?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            TouchPos = new Vector2(1, 0);

            OnTouchPadTouchDown?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            TouchPos = new Vector2(0, 0);

            OnTouchPadTouchUp?.Invoke();
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
        var newSets = new Dictionary<OVRInput.Controller, GameObject>()
        {
            {OVRInput.Controller.LTrackedRemote, leftAnchor},
            {OVRInput.Controller.RTrackedRemote, rightAnchor},
            {OVRInput.Controller.Touchpad, headAnchor}
        };

        return newSets;
    }
}