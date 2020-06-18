using States;
using UnityEngine;

public class MenuSystem : StateMachine
{
    public GameObject reticule;
    public GameObject cameraObj;
    public GameObject objectsMenu;
    public Player player;
    
    [HideInInspector]
    public Reticule reticuleObj;

    private void Awake()
    {
        reticuleObj = reticule.GetComponent<Reticule>();

        PlayerEvents.OnTriggerPressed += OnTriggerPressed;
        PlayerEvents.OnBackPressed += OnBackPressed;
        PlayerEvents.OnTouchPadTouchDown += OnStartMoving;
        Reticule.OnWallClicked += OnWallClicked;
    }



    private void Start()
    {
        SetState(new MenuClosed(this));
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTriggerPressed -= OnTriggerPressed;
        PlayerEvents.OnBackPressed -= OnBackPressed;
        PlayerEvents.OnTouchPadTouchDown -= OnStartMoving;
        Reticule.OnWallClicked -= OnWallClicked;
    }
    
    private void OnBackPressed()
    {
        StartCoroutine(State.PressBack());
    }
    
    private void OnWallClicked(GameObject wall)
    {
        StartCoroutine(State.WallClicked(wall));
    }

    public void OnObjectBtnClicked(GameObject objPrefab)
    {
        StartCoroutine(State.ObjectBtnClicked(objPrefab));
    }
    
    public void OnMaterialBtnClicked(Material material)
    {
        StartCoroutine(State.MaterialBtnClicked(material));
    }
    
    public void OnCloseButtonClicked()
    {
        StartCoroutine(State.CloseBtnClicked());
    }

    private void OnTriggerPressed()
    {
        StartCoroutine(State.PressTrigger());
    }

    private void OnStartMoving()
    {
        StartCoroutine(State.Move());
    }

    public void OnMoveBtnClicked()
    {
        StartCoroutine(State.EditBtnClicked());
        SetState(new MovingObject(this, reticuleObj.GetSelected(), State));
    }
    
    public void OnRotateBtnClicked()
    {
        StartCoroutine(State.EditBtnClicked());
        SetState(new RotatingObject(this, reticuleObj.GetSelected(), State));
    }
    
    public void OnScaleBtnClicked()
    {
        StartCoroutine(State.EditBtnClicked());
        SetState(new ScalingObject(this, reticuleObj.GetSelected(), State));
    }
    
    public void OnDeleteBtnClicked()
    {
        StartCoroutine(State.EditBtnClicked());
        Destroy(reticuleObj.GetSelected().gameObject);
        reticuleObj.ResetSelected();
    }
}