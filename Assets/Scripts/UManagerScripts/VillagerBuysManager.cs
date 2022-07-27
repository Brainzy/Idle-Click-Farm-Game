using GameManagerScripts;
using ScriptableObjectMakingScripts;
using TigerForge;
using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class VillagerBuysManager : MonoBehaviour
    {
        public static VillagerBuysManager inst;
        
        [SerializeField] private string buyingDialogText =
            "Olá! I heard you've taken over from my favourite farmer! I used to love the eggs from the chickens here! Could I have some now?";

        [SerializeField] private int amountToBuy;
        [SerializeField] private BuildingAttributes[] building;
        [SerializeField] private GameObject visitorPurchaseUI;
        [SerializeField] private GameObject[] buyingIcons;
        [SerializeField] private TextMeshProUGUI visitorDialogText;
        [SerializeField] private TextMeshProUGUI amountToBuyText;
        [SerializeField] private TextMeshProUGUI amountCoinsGainedText;
        [SerializeField] private TextMeshProUGUI youHaveAmountText;
        [SerializeField] private int experienceGrantedForSuccess = 5;


        private int memoriezedIndex;
        private const string youHaveFirstInitialText = "You have: ";
        
        private void Awake()
        {
            inst = this;
        }

        public void OpenDialog(string dialogToShow, int indexIcon, int buyingAmount)
        {
            print(dialogToShow + " " + indexIcon + " " + buyingAmount);
            if (visitorPurchaseUI.activeSelf) return;
            DeactivateAllIcons();
            buyingDialogText = dialogToShow;
            if (indexIcon != -1)
            {
                memoriezedIndex = indexIcon;
            }
            else
            {
                memoriezedIndex = Random.Range(0, buyingIcons.Length);
            }
            if (buyingAmount != -1)
            {
                amountToBuy = buyingAmount;
            }
            else
            {
                amountToBuy = Random.Range(1, 4);
            }
            buyingIcons[memoriezedIndex].SetActive(true);
            EventManager.EmitEvent("UIClick");
            visitorPurchaseUI.SetActive(true);
            visitorDialogText.SetText(buyingDialogText);
            amountToBuyText.SetText(amountToBuy.ToString());
            amountCoinsGainedText.SetText(building[memoriezedIndex].interactionResultSellPrice.ToString());
            youHaveAmountText.SetText(youHaveFirstInitialText+StorageManager.inst.AmountOfItemsInInventory(building[memoriezedIndex]).ToString());
        }

        private void DeactivateAllIcons()
        {
            for (int i = 0; i < buyingIcons.Length; i++)
            {
                buyingIcons[i].SetActive(false);
            }
        }
        

        public void PurchaseButton()
        {
            if (CanAffordPurchase() == false)
            {
                EventManager.EmitEvent("FailedAction");
            }
            else
            {
                PurchaseSuccessful();
            }
            CloseAllUTabs.inst.CloseEverything();
        }

        private void PurchaseSuccessful()
        {
            EventManager.EmitEvent("Confirmation");
            EventManager.EmitEvent("InteractedWithVillager");
            CoinManager.inst.AddCoins(building[memoriezedIndex].interactionResultSellPrice);
            StorageManager.inst.StorageRemoval(building[memoriezedIndex],amountToBuy);
            ExperienceManager.inst.AddExperience(experienceGrantedForSuccess);
        }

        private bool CanAffordPurchase()
        {
            return StorageManager.inst.AmountOfItemsInInventory(building[memoriezedIndex]) >= amountToBuy;
        }

        public void CancelButton()
        {
            EventManager.EmitEvent("UIClick");
            CloseAllUTabs.inst.CloseEverything();
        }

        public void DeclineButton()
        {
            CloseAllUTabs.inst.CloseEverything();
            EventManager.EmitEvent("InteractedWithVillager");
        }
        
        
    }
}
