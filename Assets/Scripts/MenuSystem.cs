using System;
using States;
using UnityEngine;

public class MenuSystem : StateMachine
{
    public GameObject actionsMenu;
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
        PlayerEvents.OnTouchPadTouchDown += OnStartMoving;
    }

    private void Start()
    {
        SetState(new MenuClosed(this));
    }

    private void OnDestroy()
    {
        PlayerEvents.OnTriggerPressed -= OnTriggerPressed;
        PlayerEvents.OnTouchPadTouchDown -= OnStartMoving;
    }

    public void OnPlaceButtonClicked()
    {
        StartCoroutine(State.ChooseObject());
    }
    
    public void OnObjectBtnClicked(GameObject objPrefab)
    {
        StartCoroutine(State.ObjectBtnClicked(objPrefab));
    }
    
    public void OnCloseButtonClicked()
    {
        StartCoroutine(State.CloseBtnClicked());
    }

    private void OnChangeColorButton()
    {
        StartCoroutine(State.ChangeColor());
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
        reticuleObj.ResetSelected();
        Destroy(reticuleObj.GetSelected().gameObject);
    }
}