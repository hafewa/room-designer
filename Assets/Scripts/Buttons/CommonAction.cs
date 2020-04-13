using Interfaces;
using UnityEngine;

namespace Buttons
{
    public class CommonAction : MonoBehaviour, IButtonAction
    {
        public void OnClick(Transform btn)
        {
        }

        public bool OnHover(Transform btn)
        {
            return false;
        }
    }
}