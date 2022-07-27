using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class FirstHarvest : MonoBehaviour, TutorialManager.ITutorial
    {    
        public bool shownDialog;
        private const string dialog =
            "Tap on field, than drag sickle";
        public int amountOfTreesToCutToCompleteThisTutorial;

        private int cutTreeAmount;
        
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
            EventManager.StopListening("MouseDown", OnMouseDown);
            EventManager.StartListening("TreeCut",OnTreeCut);
        }
        private void OnTreeCut()
        {
            cutTreeAmount++;
            if (cutTreeAmount != amountOfTreesToCutToCompleteThisTutorial) return;
            TutorialManager.inst.TutorialComplete();
            print("gotov tutorial ");
        }
        public void DisableSelf()
        {
            enabled = false;
        }
           
       
    }
}
