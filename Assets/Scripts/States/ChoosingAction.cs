using System.Collections;

namespace States
{
    public class ChoosingAction : State
    {
        public ChoosingAction(MenuSystem menuSystem) : base(menuSystem)
        {
        }

        public override IEnumerator Start()
        {
            var reticuleTransform = MenuSystem.reticule.transform;
            MenuSystem.actionsMenu.transform.SetPositionAndRotation(
                reticuleTransform.position + reticuleTransform.right,
                reticuleTransform.rotation);
            MenuSystem.actionsMenu.SetActive(true);

            yield break;
        }
    }
}