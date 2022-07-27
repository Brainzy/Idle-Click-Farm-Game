using TigerForge;
using TMPro;
using UnityEngine;

namespace TutorialScripts
{
    public class FriendBarAndFriendRoadSideShop : MonoBehaviour,TutorialManager.ITutorial
    {
        [SerializeField] private GregsShopManager gregsShopManager;
        [SerializeField] private GameObject gregsShopTransform;
        [SerializeField] private GameObject farmhouse;
        [SerializeField] private GameObject specialDialog;
        [SerializeField] private TextMeshProUGUI specialDialogText;
        public bool shownDialog;
        [SerializeField] private string dialog =
            "Great, you've made a new friend! Open the friend bar! Tap his picture to visit his farm!";
        [SerializeField] private string dialog2 =
            "Welcome! Please feel free to buy from my shop";
        [SerializeField] private string dialog3 =
            "Thank you for visiting! Don't forget, I'm always ready to help if you need anything done on your farm";

        private int buildingsPlaced;
        
        public void RunTutorial()
        {
            if (shownDialog) return;
            CloseAllUTabs.inst.CloseEverything();
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
            CloseAllUTabs.inst.CloseEverything();
            TutorialSharedResource.inst.DeactivateDialog(null);
            EventManager.StopListening("MouseDown", OnMouseDown);
            EventManager.StartListening("Villager1Icon",OnVillager1Icon);
          
        }

        private void OnVillager1Icon()
        {
            CloseAllUTabs.inst.CloseEverything();
            specialDialog.SetActive(true);
            specialDialogText.SetText(dialog2);
            EventManager.StopListening("Villager1Icon",OnVillager1Icon);
            EventManager.StartListening("MouseDown",OnMouseDown2);
        }
        private void OnMouseDown2()
        {
            CloseAllUTabs.inst.CloseEverything();
            TutorialSharedResource.inst.DeactivateDialog(gregsShopTransform);
            EventManager.StopListening("MouseDown", OnMouseDown2);
            gregsShopManager.ClickedOnBuilding();
            EventManager.StartListening("BoughtGregShopItem",OnBoughtGregShopItem);
        }

        private void OnBoughtGregShopItem()
        {
            CloseAllUTabs.inst.CloseEverything();
            specialDialog.SetActive(true);
            specialDialogText.SetText(dialog3);
            EventManager.StopListening("BoughtGregShopItem",OnBoughtGregShopItem);
            EventManager.StartListening("MouseDown",OnMouseDown3);
        }
        
        private void OnMouseDown3()
        {
            TutorialSharedResource.inst.DeactivateDialog(farmhouse);
            specialDialog.SetActive(false);
            EventManager.StopListening("MouseDown", OnMouseDown3);
            TutorialManager.inst.TutorialComplete();
        }
        
        
    }
}