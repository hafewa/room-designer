using Interfaces;
using UnityEngine;

namespace Buttons
{
    public class SetWallColor : MonoBehaviour, IButtonAction
    {
        public void OnClick(Transform btn)
        {
            Debug.Log("Set Wall Color pressed!");
        }

        public bool OnHover(Transform btn)
        {
            Debug.Log("Set Wall Color hovered!");
            return false;
        }
    }
}