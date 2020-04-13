using System.Collections;
using Buttons;
using UnityEngine;

namespace States
{
    public class ChoosingObject : State
    {
        public ChoosingObject(MenuSystem menuSystem) : base(menuSystem)
        {
        }

        public override IEnumerator Start()
        {
            ResetObjectsMenuPos();

            yield break;
        }

        private void ResetObjectsMenuPos()
        {
            var newPos = MenuSystem.cameraObj.transform.position + MenuSystem.cameraObj.transform.forward * 2;
            newPos.y = 1.24f;

            var eulerAngles = MenuSystem.objectsMenu.transform.eulerAngles;
            var newRotation = Quaternion.Euler(new Vector3(eulerAngles.x, MenuSystem.cameraObj.transform.eulerAngles.y,
                eulerAngles.z));

            MenuSystem.objectsMenu.transform.SetPositionAndRotation(newPos, newRotation);

            MenuSystem.objectsMenu.SetActive(true);
        }

        public override IEnumerator PressTrigger()
        {
            if (!MenuSystem.objectsMenu.activeSelf)
            {
                ResetObjectsMenuPos();
                yield break;
            }
            
            var reticule = MenuSystem.reticule.GetComponent<Reticule>();
            var currentHit = reticule.GetCurrentHit();

            if (!currentHit.collider)
            {
                yield break;
            }

            switch (currentHit.collider.gameObject.tag)
            {
                case "SelectObject":
                    SelectObject(currentHit);
                    break;
                case "CloseBtn":
                    MenuSystem.objectsMenu.SetActive(false);
                    MenuSystem.SetState(new ChoosingAction(MenuSystem));
                    break;
            }
        }

        private void SelectObject(RaycastHit currentHit)
        {
            var btn = currentHit.collider.gameObject.GetComponent<SelectObject>();
            if (btn == null)
            {
                return;
            }

            MenuSystem.SetState(new MovingObject(MenuSystem, btn.gmObj));
        }

        public override IEnumerator Move()
        {
            MenuSystem.objectsMenu.SetActive(false);
            
            yield break;
        }
    }
}