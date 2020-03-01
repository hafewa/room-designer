using UnityEngine;
using UnityEngine.Events;

public class Pointer : MonoBehaviour {
    public static UnityAction<Vector3, RaycastHit> OnPointerUpdate;

    public float distance = 10.0f;
    public LayerMask everythingMask = 0;

    private Transform _currentOrigin;
    private GameObject _currentObject;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
    }

    private void Start()
    {
        SetLineColor();
        _lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
    }

    private void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
    }

    private void Update()
    {
        Vector3 hitPoint = UpdateLine(out var hit);

        OnPointerUpdate?.Invoke(hitPoint, hit);
    }

    private Vector3 UpdateLine(out RaycastHit hit)
    {
        if (_currentOrigin == null) {
            hit = new RaycastHit();
            return new Vector3();
        }
        gameObject.transform.SetPositionAndRotation(_currentOrigin.position, _currentOrigin.rotation);
        hit = CreateRaycast(everythingMask);

        // Calc end
        Vector3 endPosition = _currentOrigin.position + (_currentOrigin.forward * distance);

        if (hit.collider != null)
            endPosition = hit.point;

        _lineRenderer.SetPosition(0, _currentOrigin.position);
        _lineRenderer.SetPosition(1, endPosition);

        return endPosition;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject gmObj)
    {
        _currentOrigin = gmObj.transform;

        // Set visible/unvisible
        _lineRenderer.enabled = controller != OVRInput.Controller.Touchpad;
    }

    private RaycastHit CreateRaycast(int layer)
    {
        var ray = new Ray(_currentOrigin.position, _currentOrigin.forward);
        Physics.Raycast(ray, out var hit, distance, layer);
        
        return hit;
    }

    private void SetLineColor()
    {
        if (!_lineRenderer)
            return;

        var endColor = Color.white;
        endColor.a = 0.0f;

        _lineRenderer.endColor = endColor;
    }
}
