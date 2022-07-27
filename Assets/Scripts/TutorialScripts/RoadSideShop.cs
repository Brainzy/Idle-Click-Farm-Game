using GameManagerScripts;
using SpawnManagers;
using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class RoadSideShop : MonoBehaviour, TutorialManager.ITutorial
    {    
        public bool shownDialog;
        private const string dialog =
            "Great! How about putting all your new produce up for sale? Put your products in the Roadside Shop and they will be bought by neighbours and other players!";
        [SerializeField] private GameObject roadSideShop;
        [SerializeField] private SpawnBezierWalker spawnVillagerBuyingFromRoadSideShop;

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
            OrtoFocusController.inst.FocusCameraOnTarget(roadSideShop);
            EventManager.StopListening("MouseDown", OnMouseDown);
            EventManager.StartListening("PlacedItemOnRoadSideSale",OnPlacedItemOnRoadSideSale);
        }
        private void OnPlacedItemOnRoadSideSale()
        {
            
            TutorialManager.inst.TutorialComplete();
            EventManager.StopListening("PlacedItemOnRoadSideSale",OnPlacedItemOnRoadSideSale);
            spawnVillagerBuyingFromRoadSideShop.StartSpawner();
            print("gotov tutorial ");
        }
    }
}
