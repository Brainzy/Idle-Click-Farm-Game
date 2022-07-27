using System.Collections.Generic;
using BuildingScripts;
using ChanglebleParameters;
using GameManagerScripts;
using TigerForge;
using UnityEngine;

namespace UManagerScripts
{
    public class SpawnToolsForUi : MonoBehaviour
    {
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform[] allTools;
        public static SpawnToolsForUi inst;
    
        public List<string> spawnList= new List<string>();

        private List<Transform> myActiveObjects= new List<Transform>();
        private void Awake()
        {
            inst = this;
        }

        public void SpawnToolsFromSpawnList(Building targetBuilding)
        {
            if (targetBuilding.buildingAttributes.isAnimal)
            {
                if (targetBuilding.animalIsHarvestable == false) return;
            }
            myActiveObjects.Clear();
            canvas.gameObject.SetActive(true);
            for (int i = 0; i < allTools.Length; i++) // first disable all than enable what is needed
            {
                allTools[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < allTools.Length; i++)
            {
                if (spawnList.Contains(allTools[i].name))
                {
                    allTools[i].gameObject.SetActive(true);
                    myActiveObjects.Add(allTools[i]);
                }
                else
                {
                    allTools[i].gameObject.SetActive(false);
                }
            }

            SetLocationsOfUI(targetBuilding);
        }
        
        private void SetLocationsOfUI(Building targetBuilding)
        {
            EventManager.EmitEvent("UIClick");
            // znaci trebalo bi da uzme renderer donju levu poziciju i da nju doda pozicije podesive
            // Vector3 botRight = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
            var bounds = targetBuilding.rend.bounds;
            var targetPosition = new Vector3(bounds.min.x,0,bounds.max.z);
          //  GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); //debugging spawning cube
            //cube.transform.position = targetPosition;
            for (int i = 0; i < myActiveObjects.Count; i++)
            {
                myActiveObjects[i].position = GameManager.inst.cam.WorldToScreenPoint(targetPosition+ToolUIPositioning.inst.toolPositions[i]);
              //  GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube); //debugging spawning cube
               // cube2.transform.position = targetPosition+ToolUIPositioning.inst.toolPositions[i];
            }
            
        }
        
    }
}
