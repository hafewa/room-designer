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

    private Reticule _reticule;

    private void Awake()
    {
        PlaceObject.OnPlaceButtonPressed += OnPlaceButton;
        PlayerEvents.OnTriggerPressed += OnTriggerPressed;
        PlayerEvents.OnTouchPadTouch += OnStartMoving;

        _reticule = reticule.GetComponent<Reticule>();
    }

    private void Start()
    {
        SetState(new MenuClosed(this));
    }

    private void OnDestroy()
    {
        PlaceObject.OnPlaceButtonPressed -= OnPlaceButton;
        PlayerEvents.OnTriggerPressed -= OnTriggerPressed;
        PlayerEvents.OnTouchPadTouch -= OnStartMoving;
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

    private void OnStartMoving(Vector2 input)
    {
        StartCoroutine(State.Move());
    }
}