using System.Collections.Generic;
using System.Text;
using BuildingScripts;
using GameManagerScripts;
using MEC;
using ScriptableObjectMakingScripts;
using TigerForge;
using TMPro;
using UManagerScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class AnimateAndStoreToolResultingAction : BuildingInteractivity,BuildingManager.ITakeActionFromTool
    {    
        [SerializeField] private BuildingManager.BuildingTag myTag;
     
        private CoroutineHandle action;
        private int myToolIndex;
        private const int waitTime = 1;
      

        public void RunActionFromTool(NewTool toolData)
        {
            if (action.IsRunning == false)
            {
                if (building.animalIsHarvestable==false) return;
                action= Timing.RunCoroutine(ActionCoRoutine(toolData));
            }
        }
        
        private IEnumerator<float> ActionCoRoutine(NewTool toolData)
        {
            if (StorageManager.inst.IsThereRoom(toolData.storageLocations[myToolIndex], myTag.ToString(),
                toolData.gainedAmountItemsFromInteraction[myToolIndex]) == false)
            {
                yield break;
            }
            EventManager.SetData("ToolUsed",toolData.toolName.ToString());
            EventManager.EmitEvent("ToolUsed");
            myToolIndex = FindMyToolIndex(toolData);
            ExperienceManager.inst.AddExperience(toolData.experienceGains[myToolIndex]);
            AnimateExperienceAndIcon(toolData);
            GetComponent<BuildingManager.IFinishedToolActionAnimal>().FinishedAction();
            yield return Timing.WaitForSeconds(waitTime);
        }
        
        private void AnimateExperienceAndIcon(NewTool toolData)
        {
            StorageManager.inst.StorageAddition(toolData.storageLocations[myToolIndex],myTag.ToString(), 
                toolData.gainedAmountItemsFromInteraction[myToolIndex], transform.position);
        }

        private int FindMyToolIndex(NewTool toolData)
        {
            for (int i = 0; i < toolData.tagsItInteractsWith.Length; i++)
            {
                if (myTag.Equals(toolData.tagsItInteractsWith[i]))
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
