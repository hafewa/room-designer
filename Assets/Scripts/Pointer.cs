using UnityEngine;
using UnityEngine.Events;

public class Pointer : MonoBehaviour {
    public static UnityAction<RaycastHit> OnPointerUpdate;

    public float distance = 10.0f;
    public LayerMask everythingMask = 0;
    public LineRenderer lineRenderer;
    public Transform reticule;
    
    public static Ray Ray;

    private Transform _currentOrigin;
    private GameObject _currentObject;

    private void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
    }

    private void Update()
    {
        UpdateLine(out var hit);

        OnPointerUpdate?.Invoke(hit);
    }

    private void UpdateLine(out RaycastHit hit)
    {
        if (_currentOrigin == null) {
            hit = new RaycastHit();
            return;
        }
        gameObject.transform.SetPositionAndRotation(_currentOrigin.position, _currentOrigin.rotation);
        hit = CreateRaycast(everythingMask);

        lineRenderer.SetPosition(0, _currentOrigin.position);
        lineRenderer.SetPosition(1, reticule.position);
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject gmObj)
    {
        _currentOrigin = gmObj.transform;

        // Set visible/unvisible
        lineRenderer.enabled = controller != OVRInput.Controller.Touchpad;
    }

    private RaycastHit CreateRaycast(int layer)
    {
        Ray = new Ray(_currentOrigin.position, _currentOrigin.forward);
        Physics.Raycast(Ray, out var hit, 20, layer);
        
        return hit;
    }
}
