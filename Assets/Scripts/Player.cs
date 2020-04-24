using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public GameObject pointer;
    public Transform cameraObj;

    private bool _canMove = true;

    private void Update()
    {
        var savedRot = cameraObj.rotation;

        var eulerAngles = transform.eulerAngles;
        var newRot = new Vector3(eulerAngles.x, pointer.transform.eulerAngles.y, eulerAngles.z);
        transform.rotation = Quaternion.Euler(newRot);
        cameraObj.rotation = savedRot;

        if (!_canMove) return;
        
        UpdatePos();
    }

    private void UpdatePos()
    {
        Vector2 touchDiff = PlayerEvents.TouchPos - PlayerEvents.StartTouchPos;
        transform.Translate(new Vector3(touchDiff.x, 0, touchDiff.y) * (moveSpeed * Time.deltaTime), Space.Self);
    }

    public void DisableMove()
    {
        _canMove = false;
    }

    public void EnableMove()
    {
        _canMove = true;
    }
}