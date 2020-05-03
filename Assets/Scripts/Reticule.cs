using UnityEngine;
using UnityEngine.Events;

public class Reticule : MonoBehaviour
{
    #region Actions

    public static UnityAction OnWallClicked;

    #endregion

    public GameObject editActions;
    
    private RaycastHit _currentHit;
    private GameObject _hitObject;

    private InteriorObject _selectedObj;
    private Vector3 _selectedObjSize;

    private void Awake()
    {
        PlayerEvents.OnTriggerPressed += ProcessTriggerPressed;
        Pointer.OnPointerUpdate += CheckHit;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTriggerPressed -= ProcessTriggerPressed;
        Pointer.OnPointerUpdate -= CheckHit;
    }

    private void Update()
    {
        if (!_selectedObj) return;
        
        editActions.transform.position = _selectedObj.transform.position + new Vector3(0, _selectedObjSize.y);
    }

    private void CheckHit(RaycastHit hit)
    {
        // On blur
        if (_currentHit.collider && !Equals(hit, _currentHit))
        {
            switch (_currentHit.transform.tag)
            {
                case "Selectable":
                    var interiorObject = _currentHit.transform.GetComponent<InteriorObject>();
                    interiorObject.OnBlur();
                    
                    break;
            }
        }

        // On hover
        if (hit.collider)
        {
            switch (hit.transform.tag)
            {
                case "Selectable":
                    var interiorObject = hit.transform.GetComponent<InteriorObject>();
                    interiorObject.OnHover();

                    break;
            }
        }

        _currentHit = hit;
    }

    private void ProcessTriggerPressed()
    {
        if (!_currentHit.collider) return;
        switch (_currentHit.transform.tag)
        {
            case "Wall":
                OnWallClicked?.Invoke();

                break;
            case "Selectable":
                var interiorObject = _currentHit.transform.GetComponent<InteriorObject>();
                if (_selectedObj)
                {
                    if (_selectedObj.Equals(interiorObject))
                    {
                        ResetSelected();
                        break;
                    }
                    
                    _selectedObj.Deselect();
                }
                
                interiorObject.Select();

                _selectedObj = interiorObject;
                _selectedObjSize = _selectedObj.GetComponent<Collider>().bounds.size;
                editActions.SetActive(true);
                
                break;
        }
    }

    public void ResetSelected()
    {
        _selectedObj.Deselect();
        _selectedObj = null;
        
        editActions.SetActive(false);
    }
    
    public InteriorObject GetSelected()
    {
        return _selectedObj;
    }

    public RaycastHit GetCurrentHit()
    {
        return _currentHit;
    }
}