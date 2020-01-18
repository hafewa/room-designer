using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_MoveSpeed = 5; 
    public float m_SmoothTime = 0.3F;
    public GameObject m_Pointer = null;

    private Vector3 velocity = Vector3.zero;
    public Vector3 targetPos = new Vector3();
    private Camera m_Camera = null;
    private Vector2 m_StartTouch = new Vector2();

    private void Awake()
    {
        PlayerEvents.onTouchPadTouch += ProcessTouchpadTouch;
        PlayerEvents.onTouchPadTouchUp += ProcessTouchpadTouchUp;
        PlayerEvents.onTouchPadTouchDown += ProcessTouchpadTouchDown;

        m_Camera = Camera.main;

        targetPos = transform.position;
    }

    private void OnDestroy()
    {
        PlayerEvents.onTouchPadTouch -= ProcessTouchpadTouch;
        PlayerEvents.onTouchPadTouchUp -= ProcessTouchpadTouchUp;
        PlayerEvents.onTouchPadTouchDown -= ProcessTouchpadTouchDown;
    }

    private void Update()
    {
        Quaternion savedRot = transform.GetChild(0).transform.rotation;
        
        Vector3 newRot = new Vector3(transform.eulerAngles.x, m_Pointer.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(newRot);

        transform.GetChild(0).transform.rotation = savedRot;

        // transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.z, Time.deltaTime * 1), transform.position.y, Mathf.Lerp(transform.position.z, targetPos.z, Time.deltaTime * 1));
        targetPos.y = transform.position.y;
        // transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, m_SmoothTime);
    }

    private void ProcessTouchpadTouch(Vector2 touchPoint) {
        Vector2 touchDiff = touchPoint - m_StartTouch;
        transform.Translate(new Vector3(touchDiff.x, 0, touchDiff.y) * m_MoveSpeed, Space.Self);
        targetPos = transform.position + transform.forward + new Vector3(-touchDiff.x, 0, -touchDiff.y);
    }

    private void ProcessTouchpadTouchUp() {
        m_StartTouch.Set(0, 0);
    }

    private void ProcessTouchpadTouchDown(Vector2 touchPoint) {
        m_StartTouch = touchPoint;
    }
}
