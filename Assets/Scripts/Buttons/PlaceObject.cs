using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Buttons
{
    public class PlaceObject : MonoBehaviour, IButtonAction
    {
        public static UnityAction OnPlaceButtonPressed;

        public void OnClick(Transform btn)
        {
            Debug.Log("Place Object clicked!");
            OnPlaceButtonPressed?.Invoke();
        }

        public bool OnHover(Transform btn)
        {
            Debug.Log("Place Object hovered!");
            
            return false;
        }
    }
}