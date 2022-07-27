using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class FirstPlanting : MonoBehaviour,TutorialManager.ITutorial
    {    
        public bool shownDialog;
        private const string dialog =
            "Tap on building menu, switch to Tree tab and drag a banana tree to an empty space";
        [SerializeField] private int amountOfBuildingsToCutToCompleteThisTutorial=1;
        [SerializeField] private string buildingToPlace;

        private int buildingsPlaced;
        
        public void RunTutorial()
        {
            if (shownDialog) return;
            TutorialSharedResource.inst.ActivateDialog();
            TutorialSharedResource.inst.text.SetText(dialog);
            shownDialog = true;
            EventManager.StartListening("MouseDown",OnMouseDown);
        }
        public void DisableSelf()
        {
            enabled = false;
        }

        private void OnMouseDown()
        {
            TutorialSharedResource.inst.DeactivateDialog(null);
            EventManager.StopListening("MouseDown", OnMouseDown);
            EventManager.StartListening("BuildingPlaced",OnBuildingPlaced);
        }
        private void OnBuildingPlaced()
        {
            var receivedString = EventManager.GetString("BuildingPlaced");
            print("postavljena " + receivedString);
            if (receivedString.Equals(buildingToPlace))
            {
                buildingsPlaced++;
            }
            if (buildingsPlaced < amountOfBuildingsToCutToCompleteThisTutorial) return;
            TutorialManager.inst.TutorialComplete();
            EventManager.StopListening("BuildingPlaced",OnBuildingPlaced);
            print("gotov tutorial ");
        }
    }
}
