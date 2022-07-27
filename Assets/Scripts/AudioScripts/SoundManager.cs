using System.Collections.Generic;
using DarkTonic.MasterAudio;
using ScriptableObjectMakingScripts;
using TigerForge;
using UnityEngine;
using WalkerScripts;

namespace AudioScripts
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private BuildingAttributes[] trees;
        private List<string> treeNames= new List<string>();
    
        private void PlaySound(string clipName)
        {
            if (AudioStatus.soundOn == false) return;
            MasterAudio.PlaySoundAndForget(clipName);
        }
        private List<string> MakeStringList(BuildingAttributes[] buildingAttributes)
        {
            List<string> tempList= new List<string>();
            for (int i = 0; i < buildingAttributes.Length; i++)
            {
                tempList.Add(buildingAttributes[i].displayName);
            }
            return tempList;
        }
    
        private void Start()
        {
            treeNames = MakeStringList(trees);
            EventManager.StartListening("Chicken",OnChicken);
            EventManager.StartListening("Cow",OnCow);
            EventManager.StartListening("StartedDraggingImage",OnStartedDraggingImage);
            EventManager.StartListening("StoppedDraggingImage",OnStoppedDraggingImage);
            EventManager.StartListening("SpentCoins",OnSpentCoins);
            EventManager.StartListening("ActivatedDialog",OnActivatedDialog);
            EventManager.StartListening("BuildingPlaced", OnBuildingPlaced);
            EventManager.StartListening("ToolUsed", OnToolUsed);
            EventManager.StartListening("LeveledUp", OnLeveledUp);
            EventManager.StartListening("UIClick", OnUIClick);
            EventManager.StartListening("FailedAction", OnFailedAction);
            EventManager.StartListening("VillagerClicked", OnVillagerClicked);
            EventManager.StartListening("VisitorClicked", OnVisitorClicked);
            EventManager.StartListening("Confirmation", OnConfirmation);
        }
        private void OnVisitorClicked()
        {
            PlaySound("greeting_female");
        }

        private void OnVillagerClicked()
        {
            PlaySound("greeting_male");
        }
        
        private void OnConfirmation()
        {
            PlaySound("confirmation");
        }

        public void PaintingInProgress()
        {
            PlaySound("Paint");
        }

        private void OnFailedAction()
        {
            PlaySound("negative");
        }

        private void OnUIClick()
        {
            PlaySound("general_UI_click");
        }

        private void OnLeveledUp()
        {
            PlaySound("level_up");
        }

        private void OnToolUsed()
        {
            var toolName = EventManager.GetString("ToolUsed");
            PlaySound(toolName);
        }

        private void OnBuildingPlaced()
        {
            var buildingName = EventManager.GetString("BuildingPlaced");
            if (treeNames.Contains(buildingName)) PlaySound("tree_planting");
        }

        private void OnActivatedDialog()
        {
            PlaySound("window_pop_up");
        }

        private void OnSpentCoins()
        {
            PlaySound("coin_purchase");
        }

        private void OnStoppedDraggingImage()
        {
            PlaySound("drop");
        }

        private void OnStartedDraggingImage()
        {
            PlaySound("tap_drag");
        }

        private void OnChicken()
        {
            PlaySound("chicken_cluck");
        }
    
        private void OnCow()
        {
            PlaySound("cow_moo");
        }


   
    }
}
