using System.Collections;
using UnityEngine;

namespace States
{
    public class MovingObject : State
    {
        private readonly InteriorObject _movingObj;
        private readonly State _prevState;
        private readonly bool _isNew;

        public MovingObject(MenuSystem menuSystem, InteriorObject movingObj, State prevState) : base(menuSystem)
        {
            _movingObj = movingObj;
            _prevState = prevState;
        }

        public MovingObject(MenuSystem menuSystem, InteriorObject movingObj, State prevState, bool isNew) : base(
            menuSystem)
        {
            _movingObj = movingObj;
            _prevState = prevState;
            _isNew = isNew;
        }

        public override IEnumerator Start()
        {
            MenuSystem.objectsMenu.SetActive(false);

            _movingObj.Select();
            _movingObj.StartMoving(MenuSystem.reticule);
            _movingObj.gameObject.SetActive(true);

            yield break;
        }

        public override IEnumerator PressTrigger()
        {
            _movingObj.StopAnything();
            if (_isNew)
            {
                _movingObj.Deselect();
            }

            MenuSystem.SetState(_prevState);

            yield break;
        }
    }
}