using System;
using Buttons;
using States;
using UnityEngine;

public class MenuSystem : StateMachine
{
    public GameObject actionsMenu;
    public GameObject reticule;
    
    private void Awake()
    {
        PlaceObject.OnPlaceButtonPressed += OnPlaceButton;
        Reticule.OnWallClicked += OnWallClicked;
    }
    
    private void Start()
    {
        SetState(new MenuClosed(this));
    }

    private void OnDestroy()
    {
        PlaceObject.OnPlaceButtonPressed -= OnPlaceButton;
        Reticule.OnWallClicked -= OnWallClicked;
    }

    private void OnPlaceButton()
    {
        StartCoroutine(State.PlaceObj());
    }
    
    private void OnChangeColorButton()
    {
        StartCoroutine(State.ChangeColor());
    }

    private void OnWallClicked()
    {
        if (actionsMenu.activeSelf)
        {
            SetState(new MenuClosed(this));
        }
        else
        {
            SetState(new ChoosingAction(this));
        }
    }
}