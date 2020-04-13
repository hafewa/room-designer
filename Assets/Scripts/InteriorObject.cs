using System;
using UnityEngine;

public class InteriorObject : MonoBehaviour
{
    private GameObject _reticule;
    private bool _isMoving;

    private void Update()
    {
        if (!_isMoving) return;

        transform.SetPositionAndRotation(_reticule.transform.position, _reticule.transform.rotation);
        transform.Rotate(Vector3.right, -90);
    }

    public void StartMoving(GameObject reticule)
    {
        _reticule = reticule;
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }
}