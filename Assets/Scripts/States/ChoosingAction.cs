﻿using System.Collections;
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
            var newPos = MenuSystem.cameraObj.transform.position + MenuSystem.cameraObj.transform.forward;
            newPos.y = .3f;
        
            var eulerAngles = MenuSystem.actionsMenu.transform.eulerAngles;
            var newRotation = Quaternion.Euler(new Vector3(eulerAngles.x, MenuSystem.cameraObj.transform.eulerAngles.y, eulerAngles.z));
        
            MenuSystem.actionsMenu.transform.SetPositionAndRotation(newPos, newRotation);
        
            MenuSystem.actionsMenu.SetActive(true);
            
            yield break;
        }
    }
}