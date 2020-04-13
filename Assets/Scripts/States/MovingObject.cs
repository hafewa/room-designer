using System.Collections;
using UnityEngine;

namespace States
{
    public class MovingObject : State
    {
        private readonly InteriorObject _movingObj;
        
        public MovingObject(MenuSystem menuSystem, GameObject movingObj) : base(menuSystem)
        {
            var newObj = Object.Instantiate(movingObj);
            newObj.AddComponent<InteriorObject>();
            _movingObj = newObj.GetComponent<InteriorObject>();
        }

        public override IEnumerator Start()
        {
            MenuSystem.objectsMenu.SetActive(false);
            
            _movingObj.StartMoving(MenuSystem.reticule);
            _movingObj.gameObject.SetActive(true);
            
            yield break;
        }

        public override IEnumerator PressTrigger()
        {
            _movingObj.StopMoving();
            
            MenuSystem.SetState(new ChoosingObject(MenuSystem));
            
            yield break;
        }
    }
}