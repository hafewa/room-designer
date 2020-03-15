using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraObj;

    private Vector3 _initPos;
    
    private void Awake()
    {
        _initPos = transform.position;

        PlayerEvents.OnTriggerPressed += ShowForwardCamera;
        PlayerEvents.OnTouchPadTouch += Hide;
    }
    
    private void OnDestroy()
    {
        PlayerEvents.OnTriggerPressed -= ShowForwardCamera;
        PlayerEvents.OnTouchPadTouch -= Hide;
    }

    private void ShowForwardCamera()
    {
        var newPos = cameraObj.position + cameraObj.forward;
        newPos.y = _initPos.y;
        
        var eulerAngles = transform.eulerAngles;
        var newRotation = Quaternion.Euler(new Vector3(eulerAngles.x, cameraObj.eulerAngles.y, eulerAngles.z));
        
        transform.SetPositionAndRotation(newPos, newRotation);
        
        gameObject.SetActive(true);
    }

    private void Hide(Vector2 input)
    {
        gameObject.SetActive(false);
    }
}
