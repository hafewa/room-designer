using System.Collections;
using UnityEngine;

namespace States
{
    public class ChoosingAction : State
    {
        public ChoosingAction(MenuSystem menuSystem) : base(menuSystem)
        {
        }

        public override IEnumerator Start()
        {
            var newPos = MenuSystem.cameraObj.transform.position + MenuSystem.cameraObj.transform.forward * 8;
            newPos.y = 5f;

            var eulerAngles = MenuSystem.actionsMenu.transform.eulerAngles;
            var newRotation = Quaternion.Euler(new Vector3(eulerAngles.x, MenuSystem.cameraObj.transform.eulerAngles.y,
                eulerAngles.z));

            MenuSystem.actionsMenu.transform.SetPositionAndRotation(newPos, newRotation);

            MenuSystem.actionsMenu.SetActive(true);

            yield break;
        }

        public override IEnumerator ChooseObject()
        {
            MenuSystem.actionsMenu.SetActive(false);
            MenuSystem.SetState(new ChoosingObject(MenuSystem));
            
            yield break;
        }

        public override IEnumerator Move()
        {
            MenuSystem.SetState(new MenuClosed(MenuSystem));
            
            yield break;
        }
    }
}