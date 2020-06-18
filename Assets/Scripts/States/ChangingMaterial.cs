using System.Collections;
using UnityEngine;

namespace States
{
    public class ChangingMaterial : State
    {
        private Material _material;

        public ChangingMaterial(MenuSystem menuSystem, Material material) : base(menuSystem)
        {
            _material = material;
        }

        public override IEnumerator WallClicked(GameObject wall)
        {
            Debug.Log("kek");
            wall.GetComponent<MeshRenderer>().material = _material;
            
            yield break;
        }

        public override IEnumerator PressBack()
        {
            MenuSystem.SetState(new ChoosingObject(MenuSystem));
            
            yield break;
        }
    }
}