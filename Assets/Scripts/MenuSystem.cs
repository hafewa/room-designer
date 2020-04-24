using System;
using Buttons;
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
        
        Reticule.OnMoveBtnClicked += OnMoveBtnClicked;
        Reticule.OnRotateBtnClicked += OnRotateBtnClicked;
        PlaceObject.OnPlaceButtonPressed += OnPlaceButton;
        PlayerEvents.OnTriggerPressed += OnTriggerPressed;
        PlayerEvents.OnTouchPadTouchDown += OnStartMoving;
    }

    private void Start()
    {
        SetState(new MenuClosed(this));
    }

    private void OnDestroy()
    {
        Reticule.OnMoveBtnClicked -= OnMoveBtnClicked;
        Reticule.OnRotateBtnClicked -= OnRotateBtnClicked;
        PlaceObject.OnPlaceButtonPressed -= OnPlaceButton;
        PlayerEvents.OnTriggerPressed -= OnTriggerPressed;
        PlayerEvents.OnTouchPadTouchDown -= OnStartMoving;
    }

    private void OnPlaceButton()
    {
        StartCoroutine(State.ChooseObject());
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

    private void OnMoveBtnClicked(InteriorObject obj)
    {
        StartCoroutine(State.EditBtnClicked());
        SetState(new MovingObject(this, obj, State));
    }
    
    private void OnRotateBtnClicked(InteriorObject obj)
    {
        StartCoroutine(State.EditBtnClicked());
        SetState(new RotatingObject(this, obj, State));
    }
}