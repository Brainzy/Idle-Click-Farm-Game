using System.Collections.Generic;
using System.Linq;
using BuildingScripts;
using ButtonScripts;
using ChanglebleParameters;
using TigerForge;
using TMPro;
using UManagerScripts;
using UnityEngine;

namespace GameManagerScripts
{
    public class DiamondManager : MonoBehaviour
    {
        [SerializeField] private GameObject myCanvas;
        [SerializeField] private int initialDiamondGrant;
        [SerializeField] private TextMeshProUGUI diamondText;
        [SerializeField] private GameObject[] slots;
        [SerializeField] private IconHolderLinks[] links;
        [SerializeField] private TextMeshProUGUI[] amountTexts;
        [SerializeField] private TextMeshProUGUI useXDiamonds;

        private int diamondAmountToUse;
        private int diamondAmount;
        private List<string> memorizedItems = new List<string>();
        private List<int> memorizedAmounts= new List<int>();
        public static DiamondManager inst;

        private void Awake()
        {
            inst = this;
        }

        private void Start()
        {
            if (ES3.KeyExists("diamondAmount"))
            {
                diamondAmount = ES3.Load<int>("diamondAmount");
            }
            else
            {
                diamondAmount = initialDiamondGrant;
                ES3.Save("diamondAmount", initialDiamondGrant);
            }

            diamondText.SetText(diamondAmount.ToString());
        }

        public void AddDiamonds(int amount)
        {
            diamondAmount += amount;
            ES3.Save("diamondAmount", diamondAmount);
            diamondText.SetText(diamondAmount.ToString());
        }

        public void OfferDiamondsForFastFinish(List<string> items, List<int> amounts)
        {
            CloseAllUTabs.inst.CloseEverything();
            memorizedAmounts= new List<int>();
            memorizedItems= new List<string>();
            myCanvas.SetActive(true);
            for (int i = 0; i < items.Count; i++)
            {
                slots[i].SetActive(true);
                amountTexts[i].SetText(amounts[i].ToString());
                for (int k = 0; k < links[i].myIcons.Length; k++)
                {
                    if (links[i].myIcons[k].name.Equals(items[i]))
                    {
                        links[i].myIcons[k].gameObject.SetActive(true);
                    }
                }
                memorizedItems.Add(items[i]);
                memorizedAmounts.Add(amounts[i]);
            }
            diamondAmountToUse = amounts.Sum();
            useXDiamonds.SetText("Use "+diamondAmountToUse);
        }

        public void UseDiamondButton()
        {
            AddDiamonds(-diamondAmountToUse);
            CloseAllUTabs.inst.CloseEverything();
            EventManager.EmitEvent("SpentCoins");
            JobBoardManager.inst.FinishDelivery();
            for (int i = 0; i < memorizedItems.Count; i++)
            {
                var storageLocation = JobBoardParameters.inst.FindStorageLocationBasedOnString(memorizedItems[i]);
                StorageManager.inst.StorageAddition(storageLocation,memorizedItems[i],memorizedAmounts[i],new Vector3(0,0,0));
            }
        }
        
    }
}