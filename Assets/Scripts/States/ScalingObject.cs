using System.Collections;
using UnityEngine;

namespace States
{
    public class ScalingObject : State
    {
        private readonly InteriorObject _scalingObj;
        private readonly State _prevState;

        public ScalingObject(MenuSystem menuSystem, InteriorObject scalingObj, State prevState) : base(menuSystem)
        {
            _scalingObj = scalingObj;
            _prevState = prevState;
        }

        public override IEnumerator Start()
        {
            MenuSystem.player.DisableMove();

            _scalingObj.StartScaling();

            yield break;
        }

        public override IEnumerator PressTrigger()
        {
            _scalingObj.StopAnything();

            MenuSystem.player.EnableMove();
            MenuSystem.SetState(_prevState);
            
            yield break;
        }
    }
}