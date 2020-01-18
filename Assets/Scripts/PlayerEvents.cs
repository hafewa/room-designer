using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerEvents : MonoBehaviour {
    #region Events
    public static UnityAction onTouchPadUp = null;
    public static UnityAction onTouchPadDown = null;
    public static UnityAction<Vector2> onTouchPadTouchDown = null;
    public static UnityAction onTouchPadTouchUp = null;
    public static UnityAction<Vector2> onTouchPadTouch = null;
    public static UnityAction onTriggerPressed = null;
    public static UnityAction<OVRInput.Controller, GameObject> onControllerSource = null;
    #endregion

    #region Anchors
    public GameObject m_LeftAnchor;
    public GameObject m_RightAnchor;
    public GameObject m_HeadAnchor;
    #endregion

    #region Input
    private Dictionary<OVRInput.Controller, GameObject> m_ControllerSets = null;
    private OVRInput.Controller m_InputSource = OVRInput.Controller.None;
    private OVRInput.Controller m_Controller = OVRInput.Controller.None;
    private bool m_InputActive = true;
    #endregion

    public Text debugText = null;

    private void Awake ()
    {
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;

        m_ControllerSets = CreateControllerSets();
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    private void Update ()
    {
        if (!m_InputActive)
            return;

        CheckForController();

        CheckInputSource();

        InputEvents();
	}

    private void CheckForController()
    {
        OVRInput.Controller controllerCheck = m_Controller;

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            controllerCheck = OVRInput.Controller.LTrackedRemote;

        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.RTrackedRemote;

        // None, touchpad
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote) &&
            !OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.Touchpad;

        // Update
        m_Controller = UpdateController(controllerCheck, m_Controller);
    }

    private void CheckInputSource()
    {
        // Update
        m_InputSource = UpdateController(OVRInput.GetActiveController(), m_InputSource);
    }

    private void InputEvents()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            if (onTouchPadDown != null)
            {
                onTouchPadDown();
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            if (onTouchPadUp != null)
            {
                onTouchPadUp();
            }
        }

        if (OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad)) {
            Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            if (onTouchPadTouchDown != null) {
                onTouchPadTouchDown(input);
            }
        }

        if (OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad)) {
            if (onTouchPadTouchUp != null) {
                onTouchPadTouchUp();
            }
        }

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
            Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            debugText.text = input.ToString(); 
            if (onTouchPadTouch != null) {
                onTouchPadTouch(input);
            }
        }

        // if (Input.GetKeyDown("w")) {
        //     onTouchPadTouch()
        // }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) {
            if (onTriggerPressed != null) {
                onTriggerPressed();
            }
        }

    }

    private OVRInput.Controller UpdateController(OVRInput.Controller check, OVRInput.Controller previous)
    {
        if (check == previous)
            return previous;

        GameObject controllerObj = null;
        m_ControllerSets.TryGetValue(check, out controllerObj);

        if (controllerObj == null)
            controllerObj = m_HeadAnchor;

        if (onControllerSource != null)
            onControllerSource(check, controllerObj);

        return check;
    }

    private void PlayerFound()
    {
        m_InputActive = true;
    }

    private void PlayerLost()
    {
        m_InputActive = false;
    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets()
    {
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>()
        {
            { OVRInput.Controller.LTrackedRemote, m_LeftAnchor },
            { OVRInput.Controller.RTrackedRemote, m_RightAnchor },
            { OVRInput.Controller.Touchpad, m_HeadAnchor }
        };

        return newSets;
    }
}
