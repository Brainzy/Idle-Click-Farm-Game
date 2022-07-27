using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class HarvestLogicExplanation : MonoBehaviour, TutorialManager.ITutorial
    {    
       [HideInInspector] public bool shownDialog;
        private const string dialog =
            "One seed is two crops";
        
        public void RunTutorial()
        {
            if (shownDialog) return;
            TutorialSharedResource.inst.ActivateDialog();
            TutorialSharedResource.inst.text.SetText(dialog);
            shownDialog = true;
            EventManager.StartListening("MouseDown",OnMouseDown);
        }

        private void OnMouseDown()
        {
            TutorialSharedResource.inst.DeactivateDialog(null);
            TutorialManager.inst.TutorialComplete();
            EventManager.StopListening("MouseDown", OnMouseDown);
        }
        public void DisableSelf()
        {
            enabled = false;
        }
    
    }
}
