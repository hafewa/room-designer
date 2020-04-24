using System.Collections;
using UnityEngine;

namespace States
{
    public class RotatingObject : State
    {
        private readonly InteriorObject _rotatingObj;
        private readonly State _prevState;

        public RotatingObject(MenuSystem menuSystem, InteriorObject rotatingObj, State prevState) : base(menuSystem)
        {
            _rotatingObj = rotatingObj;
            _prevState = prevState;
        }

        public override IEnumerator Start()
        {
            MenuSystem.player.DisableMove();
            
            MenuSystem.objectsMenu.SetActive(false);
            
            _rotatingObj.StartRotating();
            _rotatingObj.gameObject.SetActive(true);
            
            yield break;
        }

        public override IEnumerator PressTrigger()
        {
            _rotatingObj.StopAnything();

            MenuSystem.player.EnableMove();
            MenuSystem.SetState(_prevState);
            
            yield break;
        }
    }
}