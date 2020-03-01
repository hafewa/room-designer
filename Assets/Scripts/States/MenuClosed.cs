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
            
            yield break;
        }
    }
}