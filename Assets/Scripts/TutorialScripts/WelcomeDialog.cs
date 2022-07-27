using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class WelcomeDialog : MonoBehaviour, TutorialManager.ITutorial
    {
        public bool shownDialog;

        [SerializeField] private string dialog =
            "Bom dia! I'm Marcelo, a friend of the farm, and I'll help you get started at your new farm";
        
        public void RunTutorial()
        {
            if (shownDialog) return;
            TutorialSharedResource.inst.ActivateDialog();
            print("prikazujem dialog " + dialog);
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
