using GameManagerScripts;
using SpawnManagers;
using TigerForge;
using UManagerScripts;
using UnityEngine;

namespace TutorialScripts
{
    public class JobBoardTutorial : MonoBehaviour, TutorialManager.ITutorial
    {
        [SerializeField] private SpawnBezierWalker mySpawner;
        [SerializeField] private string dialog="Truck arived. Click on it, or on Job Board and deliver goods to Kindergarden. Wait for the truck to return to receive your rewards";
        [SerializeField] private string dialog2="Great, wait for the truck to come back to pick up the reward";
        [SerializeField] private string dialog3="Look the kindergarden sent a thank you note";
        [SerializeField] private string dialog4="These people can't get enough of our goods. Let's produce another order";
        [SerializeField] private GameObject jobBoard;
        [SerializeField] private JobBoardManager jobBoardManager;

        private bool startedSpawner;
    
        public void RunTutorial()
        {
            if (startedSpawner) return;
            startedSpawner = true;
            mySpawner.StartSpawner();
            EventManager.StartListening("Truck", OnTruck);
            jobBoardManager.ChangeJobBoardIndex(0);
        }

        private void OnTruck()
        {
            EventManager.StopListening("Truck", OnTruck);
            TutorialSharedResource.inst.ActivateDialog();
            OrtoFocusController.inst.FocusCameraOnTarget(jobBoard);
            TutorialSharedResource.inst.text.SetText(dialog);
            EventManager.StartListening("MouseDown",OnMouseDown);
        }
        private void OnMouseDown()
        {
            TutorialSharedResource.inst.DeactivateDialog(null);
            EventManager.StopListening("MouseDown", OnMouseDown);
            EventManager.StartListening("StartTruckDelivery", OnTruckDelivery);
        }

        private void OnTruckDelivery()
        {
            CloseAllUTabs.inst.CloseEverything();
            TutorialSharedResource.inst.ActivateDialog();
            TutorialSharedResource.inst.text.SetText(dialog2);
            EventManager.StartListening("MouseDown",OnMouseDown2);
            EventManager.StopListening("StartTruckDelivery", OnTruckDelivery);
            jobBoardManager.ChangeJobBoardIndex(1);
        }
        
        

        private void OnMouseDown2()
        {
            TutorialSharedResource.inst.DeactivateDialog(null);
            EventManager.StopListening("MouseDown",OnMouseDown2);
            EventManager.StartListening("TruckRewardPickedUp",OnTruckRewardPickedUp);
        }

        private void OnTruckRewardPickedUp()
        {
            TutorialSharedResource.inst.ActivateDialog();
            TutorialSharedResource.inst.text.SetText(dialog3);
            EventManager.StartListening("MouseDown",OnMouseDown3);
            EventManager.StopListening("TruckRewardPickedUp",OnTruckRewardPickedUp);
        }
        private void OnMouseDown3()
        {
            TutorialSharedResource.inst.DeactivateDialog(null);
            EventManager.StopListening("MouseDown",OnMouseDown3);
            EventManager.StartListening("Truck", OnTruck2);
        }

        private void OnTruck2()
        {
            TutorialSharedResource.inst.ActivateDialog();
            TutorialSharedResource.inst.text.SetText(dialog4);
            EventManager.StopListening("Truck",OnTruck2);
            EventManager.StartListening("MouseDown",OnMouseDown4);
        }
        
        private void OnMouseDown4()
        {
            TutorialSharedResource.inst.DeactivateDialog(null);
            EventManager.StopListening("MouseDown",OnMouseDown4);
            EventManager.StartListening("StartTruckDelivery", OnTruckDelivery2);
        }

        private void OnTruckDelivery2()
        {
            EventManager.StopListening("StartTruckDelivery",OnTruckDelivery2);
            TutorialManager.inst.TutorialComplete();
            print("gotov tutorial");
            jobBoardManager.ChangeJobBoardIndex(2);
        }

        public void DisableSelf()
        {
            enabled = false;
        }
    }
}
