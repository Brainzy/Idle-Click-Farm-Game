using System.Collections.Generic;
using System.Linq;
using BuildingScripts;
using DG.Tweening;
using GameManagerScripts;
using MEC;
using PathologicalGames;
using ScriptableObjectMakingScripts;
using TigerForge;
using UnityEngine;

namespace InteractionScripts
{
    public class TreeCut : BuildingInteractivity,BuildingManager.ITakeActionFromTool
    {
        [SerializeField] private Vector3[] startPositionModification;
        [SerializeField] private Vector3[] endRotation;
        [SerializeField] private Vector3[] endPosition;
        [SerializeField] private float[] fallDuration;
        [SerializeField] private BuildingManager.BuildingTag myTag;
        
        private List<Transform> despawnList= new List<Transform>();
        private CoroutineHandle action;
        private int myToolIndex;

        public void RunActionFromTool(NewTool toolData)
        {
            if (action.IsRunning == false)
            {
                action= Timing.RunCoroutine(ActionCoRoutine(toolData));
            }
        }

        private IEnumerator<float> ActionCoRoutine(NewTool toolData)
        {
            if (StorageManager.inst.IsThereRoom(toolData.storageLocations[myToolIndex],myTag.ToString(), 
                toolData.gainedAmountItemsFromInteraction[myToolIndex]) == false) yield break;
            EventManager.SetData("ToolUsed",toolData.toolName.ToString());
            EventManager.EmitEvent("ToolUsed");
            myToolIndex = FindMyToolIndex(toolData);
            ExperienceManager.inst.AddExperience(toolData.experienceGains[myToolIndex]);
            AnimateFallingTreeAndLeaves();
            AnimateExperienceAndIcon(toolData);
            EventManager.EmitEvent("TreeCut");
            DespawnAndSpawnYoungBuilding();
            yield return Timing.WaitForSeconds(fallDuration.Max());
            for (int i = 0; i < despawnList.Count; i++)
            {
                PoolManager.Pools["Buildings"].Despawn(despawnList[i]);
            }
            despawnList.Clear();
        }
        
        private void AnimateFallingTreeAndLeaves()
        {
            for (int i = 0; i < startPositionModification.Length; i++)
            {
                var startPos = transform.position + startPositionModification[i];
                var endingPos = startPos + endPosition[i];
                var spawnedCut = PoolManager.Pools["Buildings"]
                    .Spawn(i == 0 ? building.buildingAttributes.cutTree : building.buildingAttributes.treeLeaf);
                spawnedCut.position = startPos;
                spawnedCut.DOMove(endingPos, fallDuration[i]);
//                print("pozicija " + spawnedCut.name + " ce biti " + endRotation[i]);
                spawnedCut.DORotate(endRotation[i], fallDuration[i]);
                despawnList.Add(spawnedCut);
            }
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
