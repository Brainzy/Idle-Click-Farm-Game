using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingScripts;
using ChanglebleParameters;
using ScriptableObjectMakingScripts;
using TigerForge;
using TMPro;
using UManagerScripts;
using UnityEngine;

namespace GameManagerScripts
{
    public class StorageManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] barnTextAmounts;
        [SerializeField] private TextMeshProUGUI[] siloTextAmounts;
        [SerializeField] private TextMeshProUGUI[] compostTextAmounts;
        [SerializeField] private GameObject[] storageInventoryCanvases;
        [SerializeField] private TextMeshProUGUI headerBarnText;
        [SerializeField] private TextMeshProUGUI headerSiloText;
        [SerializeField] private TextMeshProUGUI headerCompostText;
        [SerializeField] private TextMeshProUGUI headerRoadSideBarnText;
        [SerializeField] private TextMeshProUGUI headerRoadSideSiloText;
        [SerializeField] private TextMeshProUGUI headerRoadSideCompostText;
        [SerializeField] private NewTool[] tools;
        [SerializeField] private AnimateStoringIcons animateStoringIcons;
        [SerializeField] private RectTransform[] barScales;
        [SerializeField] private TextMeshProUGUI[] barnTextRoadSide;
        [SerializeField] private TextMeshProUGUI[] siloTextRoadSide;
        [SerializeField] private TextMeshProUGUI[] compostTextRoadSide;
        
        private Dictionary<string, int> barnAmounts = new Dictionary<string, int>();
        private Dictionary<string, int> siloAmounts= new Dictionary<string, int>();
        private Dictionary<string, int> compostAmounts= new Dictionary<string, int>();
        private Dictionary<string, TextMeshProUGUI> barnTextDict= new Dictionary<string, TextMeshProUGUI>();
        private Dictionary<string,TextMeshProUGUI> siloTextDict = new Dictionary<string, TextMeshProUGUI>();
        private Dictionary<string,TextMeshProUGUI> compostTextDict = new Dictionary<string, TextMeshProUGUI>();
        private Dictionary<string, TextMeshProUGUI> barnTextDictRoadSide= new Dictionary<string, TextMeshProUGUI>();
        private Dictionary<string,TextMeshProUGUI> siloTextDictRoadSide = new Dictionary<string, TextMeshProUGUI>();
        private Dictionary<string,TextMeshProUGUI> compostTextDictRoadSide = new Dictionary<string, TextMeshProUGUI>();
        private List<Dictionary<string, int>> storageIntList;
        private List<Dictionary<string, TextMeshProUGUI>> storageTextList;
        private List<Dictionary<string, TextMeshProUGUI>> storageTextRoadSide;
        private List<int> storageMaxAmounts;
        private List<TextMeshProUGUI> storageHeaders= new List<TextMeshProUGUI>();
        private List<TextMeshProUGUI> storageHeadersRoadSide= new List<TextMeshProUGUI>();
        private StringBuilder stringBuilder= new StringBuilder();
        public static StorageManager inst;
        
        private const string fullConstText = " is full, please sell some products before harvesting!";

        private void Awake()
        {
            inst = this;
            for (int i = 0; i < barnTextAmounts.Length; i++)
            {
                barnTextDict.Add(barnTextAmounts[i].name,barnTextAmounts[i]);
                barnTextDictRoadSide.Add(barnTextRoadSide[i].name,barnTextRoadSide[i]);
            }
            for (int i = 0; i < siloTextAmounts.Length; i++)
            {
                siloTextDict.Add(siloTextAmounts[i].name,siloTextAmounts[i]);
                siloTextDictRoadSide.Add(siloTextRoadSide[i].name,siloTextRoadSide[i]);
            }
            for (int i = 0; i < compostTextAmounts.Length; i++)
            {
                compostTextDict.Add(compostTextAmounts[i].name,compostTextAmounts[i]);
                compostTextDictRoadSide.Add(compostTextRoadSide[i].name,compostTextRoadSide[i]);
            }
            storageTextList = new List<Dictionary<string, TextMeshProUGUI>> {barnTextDict, siloTextDict, compostTextDict};
            storageTextRoadSide = new List<Dictionary<string, TextMeshProUGUI>> {barnTextDictRoadSide, siloTextDictRoadSide, compostTextDictRoadSide};
            storageHeaders = new List<TextMeshProUGUI> {headerBarnText,headerSiloText, headerCompostText};
            storageHeadersRoadSide = new List<TextMeshProUGUI> {headerRoadSideBarnText,headerRoadSideSiloText, headerRoadSideCompostText};
        }

        private void Start()
        {
            barnAmounts = ES3.KeyExists("BarnAmounts") ? ES3.Load<Dictionary<string,int>>("BarnAmounts") : barnAmounts;
            siloAmounts = ES3.KeyExists("SiloAmounts") ? ES3.Load<Dictionary<string,int>>("SiloAmounts") : siloAmounts;
            compostAmounts = ES3.KeyExists("CompostAmounts") ? ES3.Load<Dictionary<string,int>>("CompostAmounts") : compostAmounts;
            storageMaxAmounts = new List<int> {Utility.ReturnCurrentLevelValue(StorageParameters.inst.barnStoragePerLevels),
                Utility.ReturnCurrentLevelValue(StorageParameters.inst.siloStoragePerLevels),  Utility.ReturnCurrentLevelValue(StorageParameters.inst.compostStoragePerLevels)};
            if (barnAmounts.Count == 0) InitializeDictionaryBasedOnToolTags(barnAmounts,BuildingManager.StorageLocation.Barn);
            if (siloAmounts.Count == 0) InitializeDictionaryBasedOnToolTags(siloAmounts, BuildingManager.StorageLocation.Silo);
            if (compostAmounts.Count == 0) InitializeDictionaryBasedOnToolTags(compostAmounts, BuildingManager.StorageLocation.Compost);
            storageIntList = new List<Dictionary<string, int>> {barnAmounts, siloAmounts, compostAmounts};
        }

        private void InitializeDictionaryBasedOnToolTags(Dictionary<string, int> dict, BuildingManager.StorageLocation storageLocation)
        {
            for (int i = 0; i < tools.Length; i++)
            {
                for (int j = 0; j < tools[i].storageLocations.Length; j++)
                {
                    if (!tools[i].storageLocations[j].Equals(storageLocation)) continue;
                    var itemName = tools[i].tagsItInteractsWith[j].ToString();
                    dict.Add(itemName,0);
                }
            }
        }
        public void StorageAddition(BuildingManager.StorageLocation storageLocation, string itemName, int amount, Vector3 position)
        {
            EventManager.EmitEvent("StorageChange");
            EventManager.SetData(itemName,amount);
            EventManager.EmitEvent(itemName);
            var index = IndexBasedOnStorageLocationEnum(storageLocation);
            print("dodajem " + storageLocation + " " + itemName);
            storageIntList[index][itemName] += amount;
            if (index!=2)  animateStoringIcons.AnimateIcon(index,itemName,position, (float) storageIntList[index].
                Sum((x => x.Value))/(float) storageMaxAmounts[index]);
            ES3.Save("BarnAmounts",barnAmounts);
            ES3.Save("SiloAmounts", siloAmounts);
            ES3.Save("CompostAmounts",compostAmounts);
        }

        public void StorageRemoval(BuildingAttributes building, int amount)
        {
            EventManager.EmitEvent("StorageChange");
            var itemName= building.displayName;
            var storageLocation= FindStorageLocationFromTool(building.interactivitieLinks[0], itemName);
            var index = IndexBasedOnStorageLocationEnum(storageLocation);
            storageIntList[index][itemName] -= amount;
            ES3.Save("BarnAmounts",barnAmounts);
            ES3.Save("SiloAmounts",siloAmounts);
            ES3.Save("CompostAmounts",compostAmounts);
        }

        public void StorageRemovalString(string itemName,BuildingManager.StorageLocation storageLocation, int amount)
        {
            EventManager.EmitEvent("StorageChange");
            storageIntList[IndexBasedOnStorageLocationEnum(storageLocation)][itemName] -= amount;
            ES3.Save("BarnAmounts",barnAmounts);
            ES3.Save("SiloAmounts",siloAmounts);
            ES3.Save("CompostAmounts",compostAmounts);
            SetTextFromDict(0);
            SetTextFromDict(1);
            SetTextFromDict(2);
        }

        public bool IsThereRoom(BuildingManager.StorageLocation storageLocation,string itemName,int amount)
        {
            var index = IndexBasedOnStorageLocationEnum(storageLocation);
            if (storageIntList[index].Sum((x => x.Value)) + amount <= storageMaxAmounts[index])
            {
                return true;
            }
            NoRoomTextActivation.inst.ActivateText(stringBuilder.Append(storageLocation).Append(fullConstText).ToString());
            stringBuilder.Clear();
            EventManager.EmitEvent("FailedAction");
            return false;
        }
        
        public void OpenInventory(BuildingManager.StorageLocation storageLocation)
        {
            CloseAllUTabs.inst.CloseEverything();
            var index = IndexBasedOnStorageLocationEnum(storageLocation);
            storageInventoryCanvases[index].SetActive(true);
            SetTextFromDict(index);
            var percentageOfFill=(float) storageIntList[index].Sum((x => x.Value))/(float) storageMaxAmounts[index];
            barScales[index].localScale= new Vector3(percentageOfFill, 1, 1);
            
        }
        
        public void SetTextFromDict(int index)
        {
            int currentTotal = 0;
            foreach (var kvp in storageIntList[index])
            {
                storageTextList[index][kvp.Key].SetText(kvp.Value.ToString());
                storageTextRoadSide[index][kvp.Key].SetText(kvp.Value.ToString());
                currentTotal += kvp.Value;
            }
            storageHeaders[index].SetText(stringBuilder.Append(currentTotal).Append("/").Append(storageMaxAmounts[index]));
            storageHeadersRoadSide[index].SetText(stringBuilder.Append(currentTotal).Append("/").Append(storageMaxAmounts[index]));
            stringBuilder.Clear();
        }

        private int IndexBasedOnStorageLocationEnum(BuildingManager.StorageLocation storageLocation)
        {
            switch (storageLocation)
            {
                case BuildingManager.StorageLocation.Barn:
                    return 0;
                case BuildingManager.StorageLocation.Silo:
                    return 1;
                case BuildingManager.StorageLocation.Compost:
                    return 2;
            }
            return -1;
        }

        public int AmountOfItemsInInventory(BuildingAttributes building)
        {
            var itemName = building.displayName;
            var storageLocation =
                FindStorageLocationFromTool(building.interactivitieLinks[0], itemName);
            var indexOfStorage = IndexBasedOnStorageLocationEnum(storageLocation);
            return storageIntList[indexOfStorage][itemName];
        }

        public int AmountOfItemsInInventoryString(string itemName, BuildingManager.StorageLocation storageLocation)
        {
            var indexOfStorage = IndexBasedOnStorageLocationEnum(storageLocation);
            return storageIntList[indexOfStorage][itemName];
        }

        private BuildingManager.StorageLocation FindStorageLocationFromTool(NewTool tool,string interactsWithName)
        {
            for (int i = 0; i < tool.tagsItInteractsWith.Length; i++)
            {
                if (tool.tagsItInteractsWith[i].ToString().Equals(interactsWithName))
                {
                    return tool.storageLocations[i];
                }
                
            }
            return BuildingManager.StorageLocation.Barn;
        }

    }
}

