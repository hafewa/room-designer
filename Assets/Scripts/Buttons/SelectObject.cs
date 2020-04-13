using Interfaces;
using UnityEngine;

namespace Buttons
{
    public class SelectObject : MonoBehaviour, IButtonAction
    {
        public GameObject gmObj;
        
        public void OnClick(Transform btn)
        {
        }

        public bool OnHover(Transform btn)
        {
            return false;
        }
    }
}