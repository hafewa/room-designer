using System.Collections;

namespace States
{
    public class MovingObject : State
    {
        public MovingObject(MenuSystem menuSystem) : base(menuSystem)
        {
        }

        public override IEnumerator Start()
        {
            MenuSystem.actionsMenu.SetActive(false);
            MenuSystem.reticule.GetComponent<Reticule>().ShowObject();

            yield break;
        }
    }
}