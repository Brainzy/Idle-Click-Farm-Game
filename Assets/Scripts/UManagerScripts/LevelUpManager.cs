using System.Collections.Generic;
using System.Text;
using ButtonScripts;
using GameManagerScripts;
using ScriptableObjectMakingScripts;
using TigerForge;
using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class LevelUpManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelUpCanvas;
        [SerializeField] private BuildingAttributes[] allBuildings;
        [SerializeField] private TextMeshProUGUI levelUpTextHeader;
        [SerializeField] private Transform contentParentScrollView;
        [SerializeField] private Transform prefabNewItemForScroll;
        [SerializeField] private AnimateStoringIcons animateIconToTakePrefabFrom;
        [SerializeField] private GameObject[] levelUpUIIcons;

        private const string firstPart = "You are now a level ";
        private const string secondPart = " farmer!";
        private StringBuilder stringBuilder= new StringBuilder();
        private List<GameObject> activationList= new List<GameObject>();
        private List<int> activationValueList= new List<int>();
        
        private void Start()
        {
            EventManager.StartListening("LeveledUp",OnLeveledUp);
        }

        private void OnLeveledUp()
        {
            levelUpCanvas.SetActive(true);
            stringBuilder.Clear();
            levelUpTextHeader.SetText(stringBuilder.Append(firstPart).Append(ExperienceManager.inst.ReturnCurrentLevel()).Append(secondPart));
            for (int i = 0; i < allBuildings.Length; i++)
            {
                var currentValue = Utility.ReturnCurrentLevelValue(allBuildings[i].piecesByLevel);
                var previousValue = Utility.ReturnPreviousLevelValue(allBuildings[i].piecesByLevel);
                if (currentValue > previousValue)
                {
                    AddNewIconToActivationList(currentValue - previousValue,allBuildings[i].displayName);
                }
            }
            ActivateNewIcons();
        }

        private void ActivateNewIcons()
        {
            for (int j = 0; j < activationList.Count; j++)
            {
                activationList[j].SetActive(true);
                print("aktivira se " + activationList[j].name);
                activationList[j].GetComponent<LinkForLevelUpScrollView>().myText.SetText(activationValueList[j].ToString());
            }
            activationList.Clear();
            activationValueList.Clear();
        }

        private void AddNewIconToActivationList(int textAmount,string prefabName)
        {
            for (int i = 0; i < levelUpUIIcons.Length; i++)
            {
                levelUpUIIcons[i].SetActive(false);
                if (levelUpUIIcons[i].name.Equals(prefabName))
                {
                    activationList.Add(levelUpUIIcons[i]);
                    activationValueList.Add(textAmount);
                }
            }
           
        }
        
        
    
    }
}
