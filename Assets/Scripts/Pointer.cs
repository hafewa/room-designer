using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pointer : MonoBehaviour {
    public UnityAction<Vector3, RaycastHit> onPointerUpdate = null;

    public float m_Distance = 10.0f;
    public LayerMask m_EverythingMask = 0;
    public LayerMask m_InteractableMask = 0;

    private Transform m_CurrentOrigin = null;
    private GameObject m_CurrentObject = null;
    private LineRenderer m_LineRenderer = null;

    private void Awake()
    {
        PlayerEvents.onControllerSource += UpdateOrigin;
        PlayerEvents.onTouchPadDown += ProcessTouchpadDown;
        PlayerEvents.onTouchPadUp += ProcessTouchpadUp;
    }

    private void Start()
    {
        SetLineColor();
        m_LineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
    }

    private void OnDestroy()
    {
        PlayerEvents.onControllerSource -= UpdateOrigin;
        PlayerEvents.onTouchPadDown -= ProcessTouchpadDown;
        PlayerEvents.onTouchPadUp -= ProcessTouchpadUp;
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 hitPoint = UpdateLine(out hit);

        if (onPointerUpdate != null)
            onPointerUpdate(hitPoint, hit);
    }

    private Vector3 UpdateLine(out RaycastHit hit)
    {
        if (m_CurrentOrigin == null) {
            hit = new RaycastHit();
            return new Vector3();
        }
        gameObject.transform.SetPositionAndRotation(m_CurrentOrigin.position, m_CurrentOrigin.rotation);
        hit = CreateRaycast(m_EverythingMask);

        // Calc end
        Vector3 endPosition = m_CurrentOrigin.position + (m_CurrentOrigin.forward * m_Distance);

        if (hit.collider != null)
            endPosition = hit.point;

        m_LineRenderer.SetPosition(0, m_CurrentOrigin.position);
        m_LineRenderer.SetPosition(1, endPosition);

        return endPosition;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject gmObj)
    {
        m_CurrentOrigin = gmObj.transform;

        // Set visible/unvisible
        if (controller == OVRInput.Controller.Touchpad)
            m_LineRenderer.enabled = false;
        else
            m_LineRenderer.enabled = true;
    }

    private RaycastHit CreateRaycast(int layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(m_CurrentOrigin.position, m_CurrentOrigin.forward);
        Physics.Raycast(ray, out hit, m_Distance, layer);

        return hit;
    }

    private void SetLineColor()
    {
        if (!m_LineRenderer)
            return;

        Color endColor = Color.white;
        endColor.a = 0.0f;

        m_LineRenderer.endColor = endColor;
    }

    private void ProcessTouchpadDown()
    {
        
    }

    private void ProcessTouchpadUp()
    {

    }
}
