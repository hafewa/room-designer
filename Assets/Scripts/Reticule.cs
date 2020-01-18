using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour {
    public Pointer m_Pointer;
    public MeshRenderer m_CircleRenderer;
    // public Sprite m_OpenSprite;
    // public Sprite m_ClosedSprite;

    private Camera m_Camera = null;
    private GameObject hitObject = null;

    private void Awake()
    {
        m_Pointer.onPointerUpdate += UpdateSprite;

        m_Camera = Camera.main;
    }

    private void OnDestroy()
    {
        m_Pointer.onPointerUpdate -= UpdateSprite;
    }

    private void Update()
    {
        Debug.Log(m_Camera.transform.rotation.ToString());
    }

    private void UpdateSprite(Vector3 point, RaycastHit hit)
    {
        transform.position = point;

        if (hit.collider)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Vector3 newPos = transform.position;
            newPos += transform.forward * 0.001f;
            transform.position = newPos;
        } else {
            transform.LookAt(m_Camera.gameObject.transform);            
        }
    }
}
