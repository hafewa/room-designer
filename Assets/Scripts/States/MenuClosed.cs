using System.Collections;

namespace States
{
    public class MenuClosed : State
    {
        public MenuClosed(MenuSystem menuSystem) : base(menuSystem)
        {
        }

        public override IEnumerator Start()
        {
            MenuSystem.actionsMenu.SetActive(false);
            MenuSystem.objectsMenu.SetActive(false);
            
            yield break;
        }

        public override IEnumerator PressTrigger()
        {
            MenuSystem.SetState(new ChoosingAction(MenuSystem));
            
            yield break;
        }
    }
}