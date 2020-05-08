using System.Collections;
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
            var newPos = MenuSystem.cameraObj.transform.position + MenuSystem.cameraObj.transform.forward * 11f;
            newPos.y = 8f;

            var eulerAngles = MenuSystem.objectsMenu.transform.eulerAngles;
            var newRotation = Quaternion.Euler(new Vector3(eulerAngles.x, MenuSystem.cameraObj.transform.eulerAngles.y,
                eulerAngles.z));

            MenuSystem.objectsMenu.transform.SetPositionAndRotation(newPos, newRotation);
        }

        private void ShowObjectsMenu()
        {
            MenuSystem.objectsMenu.SetActive(true);
            _isActive = true;
        }

        private void HideObjectsMenu()
        {
            MenuSystem.objectsMenu.SetActive(false);
            _isActive = false;
        }
        
        public override IEnumerator Move()
        {
            HideObjectsMenu();

            yield break;
        }

        public override IEnumerator PressTrigger()
        {
            if (MenuSystem.objectsMenu.activeSelf) yield break;

            var collider = MenuSystem.reticuleObj.GetCurrentHit().collider;
            if (collider && collider.CompareTag("Selectable")) yield break;
            
            ResetObjectsMenuPos();
            ShowObjectsMenu();
        }

        public override IEnumerator ObjectBtnClicked(GameObject objPrefab)
        {
            var newObj = Object.Instantiate(objPrefab);
            newObj.AddComponent<InteriorObject>();
            var newInteriorObj = newObj.GetComponent<InteriorObject>();

            MenuSystem.SetState(new MovingObject(MenuSystem, newInteriorObj, this, true));
            
            yield break;
        }

        public override IEnumerator EditBtnClicked()
        {
            HideObjectsMenu();

            yield break;
        }

        public override IEnumerator CloseBtnClicked()
        {
            MenuSystem.objectsMenu.SetActive(false);

            MenuSystem.SetState(new ChoosingAction(MenuSystem));

            yield break;
        }
    }
}