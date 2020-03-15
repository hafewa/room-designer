using System;
using Buttons;
using States;
using UnityEngine;

public class MenuSystem : StateMachine
{
    public GameObject actionsMenu;
    public GameObject reticule;
    public GameObject cameraObj;

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
        SetState(new MovingObject(this));
    }

    private void OnChangeColorButton()
    {
        StartCoroutine(State.ChangeColor());
    }

    private void OnTriggerPressed()
    {
        var currentHit = _reticule.GetCurrentHit();

        if (!currentHit.collider) return;
        if (currentHit.collider.tag.Equals("Button")) return;
        
        if (actionsMenu.activeSelf)
        {
            SetState(new MenuClosed(this));
        }
        else
        {
            SetState(new ChoosingAction(this));
        }
    }

    private void OnStartMoving(Vector2 input)
    {
        SetState(new MenuClosed(this));
    }
}