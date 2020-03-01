using UnityEngine;

namespace Interfaces
{
    public interface IButtonAction
    {
        void OnClick(Transform btn);
        bool OnHover(Transform btn);
    }
}