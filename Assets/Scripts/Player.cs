using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public GameObject pointer;
    public Transform cameraObj;

    private Vector2 _startTouch;

    private void Awake()
    {
        PlayerEvents.OnTouchPadTouch += ProcessTouchpadTouch;
        PlayerEvents.OnTouchPadTouchUp += ProcessTouchpadTouchUp;
        PlayerEvents.OnTouchPadTouchDown += ProcessTouchpadTouchDown;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTouchPadTouch -= ProcessTouchpadTouch;
        PlayerEvents.OnTouchPadTouchUp -= ProcessTouchpadTouchUp;
        PlayerEvents.OnTouchPadTouchDown -= ProcessTouchpadTouchDown;
    }

    private void Update()
    {
        Quaternion savedRot = cameraObj.rotation;

        var eulerAngles = transform.eulerAngles;
        Vector3 newRot = new Vector3(eulerAngles.x, pointer.transform.eulerAngles.y, eulerAngles.z);
        transform.rotation = Quaternion.Euler(newRot);
        cameraObj.rotation = savedRot;
    }

    private void ProcessTouchpadTouch(Vector2 touchPoint) {
        Vector2 touchDiff = touchPoint - _startTouch;
        transform.Translate(new Vector3(touchDiff.x, 0, touchDiff.y) * (moveSpeed * Time.deltaTime), Space.Self);
    }

    private void ProcessTouchpadTouchUp() {
        _startTouch.Set(0, 0);
    }

    private void ProcessTouchpadTouchDown(Vector2 touchPoint) {
        _startTouch = touchPoint;
    }
}
