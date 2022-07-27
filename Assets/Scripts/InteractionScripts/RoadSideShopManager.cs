using System.Collections.Generic;
using BuildingScripts;
using GameManagerScripts;
using MEC;
using TigerForge;
using TMPro;
using UnityEngine;

namespace InteractionScripts
{
    public class RoadSideShopManager : MonoBehaviour, BuildingManager.IClickOnBuilding
    {
        [SerializeField] private GameObject roadSideShopUIHolder;
        [SerializeField] private GameObject roadSideAdditionUIHolder;
        [SerializeField] private TextMeshProUGUI farmNameText;
        [SerializeField] private GameObject[] slotAmounts;
        [SerializeField] private GameObject[] slotPrices;
        [SerializeField] private GameObject[] selectedItemList;
        [SerializeField] private GameObject[] roadShopSaleSlots;
        [SerializeField] private TextMeshProUGUI selectedAmountText;
        [SerializeField] private GameObject[] tabList;
        [SerializeField] private int villagerBuysAmountFromShop = 1;

        private List<string> itemsAdded = new List<string>();
        private List<int> addedIndexes = new List<int>();
        private List<int> amountAdded = new List<int>();
        private List<int> pricesAdded = new List<int>();
        private List<int> iconIndex = new List<int>();
        private int maxClamp;
        private BuildingManager.StorageLocation currentStorageLocation;
        private int currentSelectedObjectIndex;
        private int chosenSlotIndexForSale;
        private string currentItemName;
        private float waitTimeForVillager = 1f;

        private void Start()
        {
            EventManager.StartListening("ClickedSlotButton", OnClickedSlotButton);
            EventManager.StartListening("BarnAddition", OnBarnAddition);
            EventManager.StartListening("SiloAddition", OnSiloAddition);
            EventManager.StartListening("PutOnSaleButton", OnPutOnSaleButton);
            EventManager.StartListening("RoadSideBackButton", OnRoadSideBackButton);
            EventManager.StartListening("RoadSideSelectedMinus", OnRoadSideSelectedMinus);
            EventManager.StartListening("RoadSideSelectedPlus", OnRoadSideSelectedPlus);
            EventManager.StartListening("AdditionButton", OnAdditionButton);
            EventManager.StartListening("VillagerBuysRoadSideShop", OnVillagerBuysRoadSideShop);
            currentStorageLocation = BuildingManager.StorageLocation.Barn;
            LoadStartingValues();
        }

        private void OnVillagerBuysRoadSideShop()
        {
            print("kupujemo " + itemsAdded.Count);

            for (int i = 0; i < villagerBuysAmountFromShop; i++)
            {
                if (itemsAdded.Count < 1) break;
                var selectedSlot = roadShopSaleSlots[addedIndexes[0]];
                foreach (Transform child in selectedSlot.transform)
                {
                    if (child.name.Equals("SelectedItemSlot"))
                    {
                        foreach (Transform child2 in child)
                        {
                            child2.gameObject.SetActive(false);
                        }
                    }
                }

                slotAmounts[addedIndexes[0]].SetActive(false);
                slotPrices[addedIndexes[0]].SetActive(false);
                CoinManager.inst.AddCoins(pricesAdded[0]);
                itemsAdded.Remove(itemsAdded[0]);
                addedIndexes.Remove(addedIndexes[0]);
                pricesAdded.Remove(pricesAdded[0]);
                iconIndex.Remove(iconIndex[0]);
                EventManager.EmitEvent("Confirmation");
                EventManager.EmitEvent("VillagerPurchaseSuccess");
                

                SaveRoadSideShopStatus();
            }
            Timing.RunCoroutine(WaitASecondForVillagerPurchaseEvent());
        }

        private IEnumerator<float> WaitASecondForVillagerPurchaseEvent()
        {
            yield return Timing.WaitForSeconds(waitTimeForVillager);
            EventManager.EmitEvent("VillagerRoadSidePurchaseComplete");
        }


        private void LoadStartingValues()
        {
            if (!ES3.KeyExists("itemsAdded")) return;
            itemsAdded = ES3.Load<List<string>>("itemsAdded");
            addedIndexes = ES3.Load<List<int>>("addedIndexes");
            amountAdded = ES3.Load<List<int>>("amountAdded");
            pricesAdded = ES3.Load<List<int>>("pricesAdded");
            iconIndex = ES3.Load<List<int>>("iconIndex");
            for (int i = 0; i < itemsAdded.Count; i++)
            {
                LoadVisualRoadSideShopStatus(amountAdded[i], pricesAdded[i], addedIndexes[i], iconIndex[i]);
            }
        }

        private void OnAdditionButton()
        {
            var receivedString = EventManager.GetString("AdditionButton");
            if (CheckIfAboveZeroItemsToPlaceAsSelected(receivedString))
            {
                EventManager.EmitEvent("UIClick");
                ActivateChosenItemFromSelectedList(receivedString);
                maxClamp = StorageManager.inst.AmountOfItemsInInventoryString(receivedString, currentStorageLocation);
                selectedAmountText.SetText((maxClamp / 3).ToString());
            }
            else
            {
                EventManager.EmitEvent("FailedAction");
            }
        }

        private bool CheckIfAboveZeroItemsToPlaceAsSelected(string receivedString)
        {
            return StorageManager.inst.AmountOfItemsInInventoryString(receivedString, currentStorageLocation) > 0;
        }


        private void OnRoadSideSelectedMinus()
        {
            EventManager.EmitEvent("UIClick");
            ChangeSelectedAmount(-1);
        }

        private void OnRoadSideSelectedPlus()
        {
            EventManager.EmitEvent("UIClick");
            ChangeSelectedAmount(1);
        }

        private void ChangeSelectedAmount(int amount)
        {
            var currentAmount = int.Parse(selectedAmountText.text);
            currentAmount += amount;
            if (currentAmount < 0 || currentAmount > maxClamp)
            {
                EventManager.EmitEvent("FailedAction");
                return;
            }

            EventManager.EmitEvent("UIClick");
            selectedAmountText.SetText(currentAmount.ToString());
        }

        private void OnRoadSideBackButton()
        {
            EventManager.EmitEvent("UIClick");
            print("kliknuto back ");
            roadSideShopUIHolder.SetActive(true);
            roadSideAdditionUIHolder.SetActive(false);
        }

        private void OnPutOnSaleButton()
        {
            var currentAmount = int.Parse(selectedAmountText.text);
            if (currentAmount <= 0) return;
            var price = BuildingManager.inst.FindPriceBasedOnName(currentItemName);
            addedIndexes.Add(chosenSlotIndexForSale);
            amountAdded.Add(currentAmount);
            pricesAdded.Add(price * currentAmount);
            itemsAdded.Add(currentItemName);
            iconIndex.Add(currentSelectedObjectIndex);
            LoadVisualRoadSideShopStatus(currentAmount, price, chosenSlotIndexForSale, currentSelectedObjectIndex);
            StorageManager.inst.StorageRemovalString(currentItemName, currentStorageLocation, currentAmount);
            EventManager.EmitEvent("PlacedItemOnRoadSideSale");
            SaveRoadSideShopStatus();
            OnRoadSideBackButton();
        }

        private void LoadVisualRoadSideShopStatus(int currentAmount, int price, int index, int indexOfIcon)
        {
            var selectedSlot = roadShopSaleSlots[index];
            var counter = 0;
            foreach (Transform child in selectedSlot.transform)
            {
                if (child.name.Equals("SelectedItemSlot"))
                {
                    foreach (Transform child2 in child)
                    {
                        if (counter == indexOfIcon)
                        {
                            child2.gameObject.SetActive(true);
                        }

                        counter++;
                    }
                }
            }

            slotAmounts[index].SetActive(true);
            slotAmounts[index].GetComponent<TextMeshProUGUI>().SetText(currentAmount.ToString());
            slotPrices[index].SetActive(true);
            slotPrices[index].GetComponent<TextMeshProUGUI>().SetText((price * currentAmount).ToString());
        }

        private void SaveRoadSideShopStatus()
        {
            ES3.Save("itemsAdded", itemsAdded);
            ES3.Save("addedIndexes", addedIndexes);
            ES3.Save("amountAdded", amountAdded);
            ES3.Save("pricesAdded", pricesAdded);
            ES3.Save("iconIndex", iconIndex);
        }

        private void OnBarnAddition()
        {
            EventManager.EmitEvent("UIClick");
            CloseAllOtherIndexesInTabList(0);
            currentStorageLocation = BuildingManager.StorageLocation.Barn;
        }

        private void OnSiloAddition()
        {
            EventManager.EmitEvent("UIClick");
            CloseAllOtherIndexesInTabList(1);
            currentStorageLocation = BuildingManager.StorageLocation.Silo;
        }

        private void CloseAllOtherIndexesInTabList(int index)
        {
            for (int i = 0; i < tabList.Length; i++)
            {
                tabList[i].SetActive(i == index);
            }
        }

        private void OnClickedSlotButton()
        {
            chosenSlotIndexForSale = int.Parse(EventManager.GetString("ClickedSlotButton"));
            if (addedIndexes.Contains(chosenSlotIndexForSale)) return;
            EventManager.EmitEvent("UIClick");
            CloseSelectedItem();
            SetTextAmounts();
            roadSideShopUIHolder.SetActive(false);
            roadSideAdditionUIHolder.SetActive(true);
        }

        private void CloseSelectedItem()
        {
            for (int i = 0; i < selectedItemList.Length; i++)
            {
                selectedItemList[i].SetActive(false);
            }

            selectedAmountText.SetText(0.ToString());
        }

        private void SetTextAmounts()
        {
            StorageManager.inst.SetTextFromDict(0);
            StorageManager.inst.SetTextFromDict(1);
        }

        private void ActivateChosenItemFromSelectedList(string itemName)
        {
            for (int i = 0; i < selectedItemList.Length; i++)
            {
                selectedItemList[i].SetActive(false);
            }

            for (int i = 0; i < selectedItemList.Length; i++)
            {
                if (itemName.Equals(selectedItemList[i].name))
                {
                    selectedItemList[i].SetActive(true);
                    currentSelectedObjectIndex = i;
                    currentItemName = itemName;
                }
            }
        }

        public void ClickedOnBuilding()
        {
            roadSideShopUIHolder.SetActive(true);
            var farmName =
                ES3.KeyExists("FarmName") ? ES3.Load<string>("FarmName") : "Farm"; //ToDo switch to cloud solution
            farmNameText.SetText(farmName + "'s Roadside Stand");
        }
    }
}