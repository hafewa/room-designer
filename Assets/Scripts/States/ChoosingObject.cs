using System.Collections;
using Buttons;
using UnityEngine;

namespace States
{
    public class ChoosingObject : State
    {
        private bool _isActive = true;

        public ChoosingObject(MenuSystem menuSystem) : base(menuSystem)
        {
        }

        public override IEnumerator Start()
        {
            if (!_isActive) yield break;

            ResetObjectsMenuPos();
            ShowObjectsMenu();
        }

        private void ResetObjectsMenuPos()
        {
            var newPos = MenuSystem.cameraObj.transform.position + MenuSystem.cameraObj.transform.forward * 2;
            newPos.y = 1.24f;

            var eulerAngles = MenuSystem.objectsMenu.transform.eulerAngles;
            var newRotation = Quaternion.Euler(new Vector3(eulerAngles.x, MenuSystem.cameraObj.transform.eulerAngles.y,
                eulerAngles.z));

            MenuSystem.objectsMenu.transform.SetPositionAndRotation(newPos, newRotation);
        }

        public void ShowObjectsMenu()
        {
            MenuSystem.objectsMenu.SetActive(true);
            _isActive = true;
        }

        public void HideObjectsMenu()
        {
            MenuSystem.objectsMenu.SetActive(false);
            _isActive = false;
        }

        public override IEnumerator PressTrigger()
        {
            var reticule = MenuSystem.reticuleObj;
            var currentHit = reticule.GetCurrentHit();

            if (!currentHit.collider)
            {
                if (MenuSystem.objectsMenu.activeSelf) yield break;
                ResetObjectsMenuPos();
                ShowObjectsMenu();

                yield break;
            }

            switch (currentHit.collider.gameObject.tag)
            {
                case "SelectObject":
                    SelectObject(currentHit.collider);
                    break;
                case "CloseBtn":
                    HideObjectsMenu();

                    MenuSystem.SetState(new ChoosingAction(MenuSystem));
                    break;
                case "Selectable":
                    break;

                default:
                    if (MenuSystem.objectsMenu.activeSelf) break;
                    ResetObjectsMenuPos();
                    ShowObjectsMenu();

                    break;
            }
        }

        private void SelectObject(Component collider)
        {
            var objBtn = collider.gameObject.GetComponent<SelectObject>();

            var newObj = Object.Instantiate(objBtn.gmObj);
            newObj.AddComponent<InteriorObject>();
            var newInteriorObj = newObj.GetComponent<InteriorObject>();

            MenuSystem.SetState(new MovingObject(MenuSystem, newInteriorObj, this, true));
        }

        public override IEnumerator Move()
        {
            HideObjectsMenu();

            yield break;
        }

        public override IEnumerator EditBtnClicked()
        {
            HideObjectsMenu();
            
            yield break;
        }
    }
}