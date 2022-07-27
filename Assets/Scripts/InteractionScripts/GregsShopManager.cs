using System;
using System.Collections.Generic;
using BuildingScripts;
using ChanglebleParameters;
using GameManagerScripts;
using TigerForge;
using TMPro;
using UnityEngine;

public class GregsShopManager : MonoBehaviour, BuildingManager.IClickOnBuilding
{
    [SerializeField] private float[] gregSecondCoolDowns;
    [SerializeField] private GameObject gregsShopUI;
    [SerializeField] private GameObject[] slotHolders;
    [SerializeField] private TextMeshProUGUI[] amounts;
    [SerializeField] private TextMeshProUGUI[] prices;
    [SerializeField] private Transform[] itemNames;

    private List<DateTime> gregTimers = new List<DateTime>();
    private List<bool> gregSlotStatus = new List<bool>();
    private int chosenSlotForPurchase;

    private void Start()
    {
        if (!ES3.KeyExists("gregTimers"))
        {
            ActivateAllSlots();
            gregTimers = new List<DateTime> {DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now};
            gregSlotStatus= new List<bool>{true,true,true,true};
            SaveGregShop();
        }
        else
        {
            LoadGregShop();
        }

        EventManager.StartListening("GregSlot", OnGregSlot);
    }

    private void LoadGregShop()
    {
        gregTimers = ES3.Load<List<DateTime>>("gregTimers");
        gregSlotStatus= ES3.Load<List<bool>>("gregSlotStatus");
    }

    private void SaveGregShop()
    {
        ES3.Save("gregTimers", gregTimers);
        ES3.Save("gregSlotStatus", gregSlotStatus);
    }
    private void ActivateAllSlots()
    {
        for (int i = 0; i < slotHolders.Length; i++)
        {
            slotHolders[i].SetActive(true);
        }
    }

    private void OnGregSlot()
    {
        chosenSlotForPurchase = int.Parse(EventManager.GetString("GregSlot"));
        if (slotHolders[chosenSlotForPurchase].activeSelf == false) return;
        var item = itemNames[chosenSlotForPurchase].name;
        var storageLocation = JobBoardParameters.inst.FindStorageLocationBasedOnString(item);
        var amountBought = int.Parse(amounts[chosenSlotForPurchase].text.Replace("x",""));
        if (StorageManager.inst.IsThereRoom(storageLocation, item, amountBought) == false)
        {
            EventManager.EmitEvent("FailedAction");
            return;
        }
        var price = int.Parse(prices[chosenSlotForPurchase].text);
        if (price > CoinManager.inst.ReturnCoinAmount())
        {
            EventManager.EmitEvent("FailedAction");
            return;
        }

        EventManager.EmitEvent("SpentCoins");
        CoinManager.inst.AddCoins(-price);
        StorageManager.inst.StorageAddition(storageLocation, item, amountBought,
            itemNames[chosenSlotForPurchase].position);
        slotHolders[chosenSlotForPurchase].SetActive(false);
        gregSlotStatus[chosenSlotForPurchase] = false;
        EventManager.EmitEvent("BoughtGregShopItem");
        gregTimers[chosenSlotForPurchase]=DateTime.Now;
        SaveGregShop();
    }

    private void CheckIfShoulOpenGregSlot()
    {
        for (int i = 0; i < gregSlotStatus.Count; i++)
        {
            slotHolders[i].SetActive(gregSlotStatus[i]);
        }
        var now = DateTime.Now;
        for (int i = 0; i < gregTimers.Count; i++)
        {
            if (now.Subtract(gregTimers[i]).TotalSeconds >= gregSecondCoolDowns[i])
            {
                if (slotHolders[i].activeSelf == false)
                {
                    slotHolders[i].SetActive(true);
                }
            }
        }
    }
    public void ClickedOnBuilding()
    {
        CheckIfShoulOpenGregSlot();
        gregsShopUI.SetActive(true);
    }
}