using SpawnManagers;
using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class PurchaseCowFarm : MonoBehaviour,TutorialManager.ITutorial
    {
        public bool shownDialog;
        [SerializeField] private string dialog =
            "Open asset menu and purchase Cow farm and add 3 cows to it";
        [SerializeField] private string dialog2 =
            "Cows are ready to be milked";
        [SerializeField] private int amountOfChickensToBuyForThisTutorial = 1;
        [SerializeField] private SpawnBezierWalker spawnVillagerBuyingMilk;

        private int boughtCowFarm;
    
        public void DisableSelf()
        {
            enabled = false;
        }
    
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
            CloseAllUTabs.inst.CloseEverything();
            TutorialSharedResource.inst.DeactivateDialog(null);
            EventManager.StopListening("MouseDown", OnMouseDown);
            EventManager.StartListening("BuildingPlaced",OnBuildingPlaced);
        }

        private void OnBuildingPlaced()
        {
            CloseAllUTabs.inst.CloseEverything();
            var buildingName = EventManager.GetString("BuildingPlaced");
            if (buildingName != "Cow Farm") return;
            boughtCowFarm++;
            if (boughtCowFarm >= amountOfChickensToBuyForThisTutorial)
            {
                EventManager.StopListening("BuildingPlaced",OnBuildingPlaced);
                EventManager.StartListening("CowHarvestable",OnCowHarvestable);
            }
        }

        private void OnCowHarvestable()
        {
            CloseAllUTabs.inst.CloseEverything();
            TutorialSharedResource.inst.ActivateDialog();
            TutorialSharedResource.inst.text.SetText(dialog2);
            EventManager.StopListening("CowHarvestable",OnCowHarvestable);
            EventManager.StartListening("MouseDown", OnMouseDown2);
        }

        private void OnMouseDown2()
        {
            CloseAllUTabs.inst.CloseEverything();
            EventManager.StopListening("MouseDown", OnMouseDown2);
            EventManager.StartListening("Cow",OnCow);
        }

        private void OnCow()
        {
            EventManager.StopListening("Cow",OnCow);
            EventManager.StartListening("InteractedWithVillager",OnInteractedWithVillager);
            spawnVillagerBuyingMilk.StartSpawner();
        }

        private void OnInteractedWithVillager()
        {
            CloseAllUTabs.inst.CloseEverything();
            TutorialManager.inst.TutorialComplete();
            EventManager.StopListening("InteractedWithVillager",OnInteractedWithVillager);
        }
    }
}