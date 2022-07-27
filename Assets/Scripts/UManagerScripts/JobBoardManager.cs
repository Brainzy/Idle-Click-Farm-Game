using System.Collections.Generic;
using System.Text;
using ButtonScripts;
using ChanglebleParameters;
using GameManagerScripts;
using MEC;
using TigerForge;
using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class JobBoardManager : MonoBehaviour
    {
        [SerializeField] private GameObject jobCanvas;
        [SerializeField] private TextMeshProUGUI[] xps;
        [SerializeField] private TextMeshProUGUI[] coins;
        [SerializeField] private IconHolderLinks[] iconLinks;
        [SerializeField] private GameObject[] checkMarks;
        [SerializeField] private GameObject[] xMarks;
        [SerializeField] private GameObject[] xpCoinHolder;
        [SerializeField] private TextMeshProUGUI[] amountAvailableVsRequested;
        [SerializeField] private TextMeshProUGUI nameOfOrderer;
        [SerializeField] private GameObject[] checkMarksInGame;
        public static JobBoardManager inst;

        private int jobBoardStateIndex;
        private Dictionary<int, string> ordererNamesBySlot = new Dictionary<int, string>();
        private Dictionary<int, List<string>> itemsForEachSlot = new Dictionary<int, List<string>>();
        private Dictionary<int, List<int>> amountsForEachSlot = new Dictionary<int, List<int>>();
        private Dictionary<int, int> xpForEachSlot = new Dictionary<int, int>();
        private bool selectedOrderPossible;
        private bool truckAvailable;
        private int selectedIndexMemorized;

        private void Start()
        {
            inst = this;
            jobBoardStateIndex = ES3.KeyExists("JobBoardStateIndex") ? ES3.Load<int>("JobBoardStateIndex") : -1;
            EventManager.StartListening("JobBoardSlotButton", OnJobBoardSlotButton);
            EventManager.StartListening("Truck", OnTruck);
            EventManager.StartListening("StorageChange", OnStorageChange);
            Timing.RunCoroutine(PrepareJobBoardAndCheckMarks());
        }

        private IEnumerator<float> PrepareJobBoardAndCheckMarks()
        {
            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitForOneFrame;
            PrepareJobBoard();
            OnStorageChange();
        }

        private void OnStorageChange()
        {
            foreach (var kvp in ordererNamesBySlot)
            {
                var itemNames = itemsForEachSlot[kvp.Key];
                int counter = 0;
                for (int i = 0; i < itemNames.Count; i++)
                {
                    var availableAmount = StorageManager.inst.AmountOfItemsInInventoryString(itemNames[i],
                        JobBoardParameters.inst.FindStorageLocationBasedOnString(itemNames[i]));
                    var requestedAmount = amountsForEachSlot[kvp.Key][i];
                    if (availableAmount <= requestedAmount)
                    {
                        counter++;
                    }
                }
                checkMarksInGame[kvp.Key].SetActive(counter == 0);
            }
        }

        public void ChangeJobBoardIndex(int newIndex)
        {
            jobBoardStateIndex = newIndex;
            ES3.Save("JobBoardStateIndex", jobBoardStateIndex);
        }

        private void OnTruck()
        {
            truckAvailable = true;
        }

        private void OnJobBoardSlotButton()
        {
            EventManager.EmitEvent("UIClick");
            var chosenSlot = int.Parse(EventManager.GetString("JobBoardSlotButton"));
            ConvertDictionariesToVisualData(chosenSlot);
        }

        public void OnDeliverButton()
        {
            print(selectedOrderPossible + " " + truckAvailable);
            if (selectedOrderPossible == false && truckAvailable)
            {
                print("ajmo brilijanti ");
                SendMissingMaterialsForDiamondFastFinish();
                EventManager.EmitEvent("FailedAction");
            }
            else
            {
                print("nece ");
                if (truckAvailable == false)
                {
                    print("nema kamiona ");
                    EventManager.EmitEvent("FailedAction");
                    return;
                }
                if (selectedOrderPossible == false)
                {
                    print("nema mogucnosti ");
                    EventManager.EmitEvent("FailedAction");
                    return;
                }
                FinishDelivery();
            }
        }

        public void FinishDelivery()
        {
            truckAvailable = false;
            EventManager.EmitEvent("StartTruckDelivery");
            EventManager.EmitEvent("Confirmation");
            ES3.Save("truckCarryGold", int.Parse(coins[selectedIndexMemorized].text));
            ES3.Save("truckCarryXp", int.Parse(xps[selectedIndexMemorized].text));
            RemoveStorageItemsForMemorizedIndex();
            CloseAllUTabs.inst.CloseEverything();
        }

        private void SendMissingMaterialsForDiamondFastFinish()
        {
            var itemNames = itemsForEachSlot[selectedIndexMemorized];
            var tempNames = new List<string>();
            var tempAmounts = new List<int>();
            for (int i = 0; i < itemNames.Count; i++)
            {
                var availableAmount = StorageManager.inst.AmountOfItemsInInventoryString(itemNames[i],
                    JobBoardParameters.inst.FindStorageLocationBasedOnString(itemNames[i]));
                var requestedAmount = amountsForEachSlot[selectedIndexMemorized][i];
                if (availableAmount < requestedAmount) 
                {
                    tempNames.Add(itemNames[i]);
                    tempAmounts.Add(requestedAmount-availableAmount);
                }
            }
            DiamondManager.inst.OfferDiamondsForFastFinish(tempNames,tempAmounts);
        }

        private void RemoveStorageItemsForMemorizedIndex()
        {
            var removalList = itemsForEachSlot[selectedIndexMemorized];
            for (int i = 0; i < removalList.Count; i++)
            {
                StorageManager.inst.StorageRemovalString(removalList[i],
                    JobBoardParameters.inst.FindStorageLocationBasedOnString(removalList[i]),
                    amountsForEachSlot[selectedIndexMemorized][i]);
            }
            ordererNamesBySlot.Remove(selectedIndexMemorized);
            itemsForEachSlot.Remove(selectedIndexMemorized);
            amountsForEachSlot.Remove(selectedIndexMemorized);
            xpForEachSlot.Remove(selectedIndexMemorized);
            AddSpecificJobSlot(selectedIndexMemorized);
            print(ordererNamesBySlot.Count);
            SaveJobBoard();
        }

        private void AddSpecificJobSlot(int jobSlot)
        {
            var ordererName = JobBoardParameters.inst.ReturnOrdererNames(jobBoardStateIndex);
            ordererNamesBySlot.Add(jobSlot, ordererName);
            var itemsRequired = JobBoardParameters.inst.ReturnRandomItemsRequiredForSingleJob(jobBoardStateIndex);
            itemsForEachSlot.Add(jobSlot, itemsRequired);
            amountsForEachSlot.Add(jobSlot,
                JobBoardParameters.inst.ReturnAmountsRequiredForSingleJob(itemsRequired.Count));
            xpForEachSlot.Add(jobSlot, JobBoardParameters.inst.ReturnXpForSlot());
            SaveJobBoard();
        }

        private void MakeAllNewJobs(int amountOfJobs)
        {
            var difference = amountOfJobs - ordererNamesBySlot.Count;
            var index = ordererNamesBySlot.Count;
            for (int i = 0; i < difference; i++)
            {
                AddSpecificJobSlot(index);
                index++;
            }
        }

        private void ConvertDictionariesToVisualData(int indexChosen)
        {
            if (ordererNamesBySlot.Count == 0) return;
            DeactivateAllCheckAndXMarks();
            for (int i = 0; i < ordererNamesBySlot.Count; i++)
            {
                xps[i].SetText(xpForEachSlot[i].ToString());
                coins[i].SetText(JobBoardParameters.inst
                    .CalculateCoinAwardForJob(itemsForEachSlot[indexChosen], amountsForEachSlot[indexChosen])
                    .ToString());
            }

            ActivateCorrectIcons(itemsForEachSlot[indexChosen]);
            nameOfOrderer.SetText(ordererNamesBySlot[indexChosen]);
            SetAmountsAvailableVsRequested(indexChosen);
        }

        private void DeactivateAllCheckAndXMarks()
        {
            for (int i = 0; i < checkMarks.Length; i++)
            {
                checkMarks[i].gameObject.SetActive(false);
                xMarks[i].gameObject.SetActive(false);
            }
        }

        private void SetAmountsAvailableVsRequested(int indexChosen)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var itemNames = itemsForEachSlot[indexChosen];
            int counter = 0;
            DeleteAllTexts();
            for (int i = 0; i < itemNames.Count; i++)
            {
                var availableAmount = StorageManager.inst.AmountOfItemsInInventoryString(itemNames[i],
                    JobBoardParameters.inst.FindStorageLocationBasedOnString(itemNames[i]));
                var requestedAmount = amountsForEachSlot[indexChosen][i];
                amountAvailableVsRequested[i]
                    .SetText(stringBuilder.Append(availableAmount).Append("/").Append(requestedAmount));
                stringBuilder.Clear();
                if (availableAmount >= requestedAmount) 
                {
                    checkMarks[i].SetActive(true);
                }
                else
                {
                    xMarks[i].SetActive(true);
                    counter++;
                }
            }

            selectedOrderPossible = counter <= 0;
            selectedIndexMemorized = indexChosen;
        }

        private void DeleteAllTexts()
        {
            for (int i = 0; i < amountAvailableVsRequested.Length; i++)
            {
                amountAvailableVsRequested[i].SetText("");
            }
        }


        private void ActivateCorrectIcons(List<string> itemNames)
        {
            for (int i = 0; i < iconLinks.Length; i++) // this loop deactivates all icons
            {
                for (int j = 0; j < iconLinks[i].myIcons.Length; j++)
                {
                    iconLinks[i].myIcons[j].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < itemNames.Count; i++) // this loop activates correct item
            {
                for (int j = 0; j < iconLinks[i].myIcons.Length; j++)
                {
                    if (itemNames[i].Equals(iconLinks[i].myIcons[j].name))
                    {
                        iconLinks[i].myIcons[j].gameObject.SetActive(true);
                    }
                }
            }
        }

        private void DisableNonActiveSlots()
        {
            for (int i = 0; i < xpCoinHolder.Length; i++)
            {
                if (i <= ordererNamesBySlot.Count - 1)
                {
                    xpCoinHolder[i].SetActive(true);
                }
                else
                {
                    xpCoinHolder[i].SetActive(false);
                }
            }
        }

        public void OpenCanvas()
        {
            PrepareJobBoard();
            EventManager.EmitEvent("UIClick");
            CloseAllUTabs.inst.CloseEverything();
            jobCanvas.SetActive(true);
        }

        private void PrepareJobBoard()
        {
            if (ordererNamesBySlot.Count <= 0 && ES3.KeyExists("OrdererNamesBySlot"))
            {
                LoadJobBoard();
            }
            else
            {
                MakeAllNewJobs(JobBoardParameters.inst.ReturnNumberOfJobsBasedOnState(jobBoardStateIndex));
            }

            ConvertDictionariesToVisualData(0);
            DisableNonActiveSlots();
        }

        private void LoadJobBoard()
        {
            ordererNamesBySlot = ES3.Load<Dictionary<int, string>>("OrdererNamesBySlot");
            itemsForEachSlot = ES3.Load<Dictionary<int, List<string>>>("ItemsForEachSlot");
            amountsForEachSlot = ES3.Load<Dictionary<int, List<int>>>("AmountsForEachSlot");
            xpForEachSlot = ES3.Load<Dictionary<int, int>>("XpForEachSlot");
        }

        private void SaveJobBoard()
        {
            ES3.Save("OrdererNamesBySlot",ordererNamesBySlot);
            ES3.Save("ItemsForEachSlot",itemsForEachSlot);
            ES3.Save("AmountsForEachSlot",amountsForEachSlot);
            ES3.Save("XpForEachSlot",xpForEachSlot);
        }
        
    }
}