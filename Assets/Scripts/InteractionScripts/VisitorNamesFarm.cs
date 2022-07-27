using System;
using BuildingScripts;
using TigerForge;
using UManagerScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class VisitorNamesFarm : MonoBehaviour, BuildingManager.IClickOnBuilding
    {
        private bool finishedInteraction;

        private void OnEnable()
        {
            finishedInteraction = false;
        }

        public void ClickedOnBuilding()
        {
            if (finishedInteraction) return;
            if (!ES3.KeyExists("FarmName"))
            {
                EventManager.EmitEvent("VisitorClicked");
                VisitorNamesFarmManager.inst.OpenDialog();
            }
            else
            {
                EventManager.EmitEvent("Visitor1EndClicked");
                Visitor1End.inst.OpenEndDialogue();
            }
            finishedInteraction = true;
        }
    }
}
