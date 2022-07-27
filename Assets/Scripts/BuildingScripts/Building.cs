using System;
using System.Diagnostics.CodeAnalysis;
using GameManagerScripts;
using HighlightPlus;
using InteractionScripts;
using PathologicalGames;
using ScriptableObjectMakingScripts;
using SpawnManagers;
using TigerForge;
using UnityEngine;

namespace BuildingScripts
{
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public class Building : MonoBehaviour
    {   
        public bool dragBuilding;
        public BuildingAttributes buildingAttributes;
        public HighlightEffect highlightEffect;
        public Renderer rend;
        public int myPrefabIndex;
        public DateTime startedDate;
        public float growTimer;
        public bool animalIsHarvestable;
        

        private void OnEnable()
        {
            if (buildingAttributes.isGrowable)
            {
                myPrefabIndex = Utility.FindMyIndex(buildingAttributes, name);
                if (myPrefabIndex > buildingAttributes.myGrowableTimers.Count - 1) return;
                growTimer = buildingAttributes.myGrowableTimers[myPrefabIndex];
                startedDate = DateTime.Now;
            }
        }

        public void UpdatedTimerOfflineTimePassed(DateTime memorizedTime)
        {
            growTimer -= DateTime.Now.Subtract(memorizedTime).Seconds;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (dragBuilding == false) return;
                if (IsBuildingInValidSpot())
                {
                    BuildingPlaced();
                }
                else
                {
                    PoolManager.Pools["Buildings"].Despawn(transform);
                }
            }
            if (dragBuilding)
            {
                var newPos=GameManager.inst.GetCurTilePosition();
                var myPos = transform.position;
                if (newPos == myPos) return;
                transform.position = newPos;
                if (IsBuildingInValidSpot() == false)
                {
                    highlightEffect.highlighted = true;
                }
                else
                {
                    highlightEffect.highlighted = false;
                }
            }

            if (dragBuilding==false && buildingAttributes.isGrowable)
            {
                if (myPrefabIndex > buildingAttributes.myGrowablePrefabs.Count - 1) return;
                growTimer -= Time.deltaTime;
                if (growTimer > 0) return;
                SpawnNextGrowableBuilding();
            }
        }

        private void SpawnNextGrowableBuilding()
        {
            myPrefabIndex++;
            if (myPrefabIndex > buildingAttributes.myGrowablePrefabs.Count - 1) return;
            var spawnedObject = PoolManager.Pools["Buildings"].Spawn(buildingAttributes.myGrowablePrefabs[myPrefabIndex]);
            spawnedObject.rotation = buildingAttributes.myGrowablePrefabs[myPrefabIndex].rotation;
            spawnedObject.GetComponent<Building>().myPrefabIndex = myPrefabIndex + 1;
            spawnedObject.position = transform.position;
            BuildingManager.inst.ChangedBuilding(spawnedObject.GetComponent<Building>());
            PoolManager.Pools["Buildings"].Despawn(transform);
        }

        private void BuildingPlaced()
        {
            dragBuilding = false;
            UReferences.inst.ReturnUScript(buildingAttributes.displayName).amountOfBuilding.amountPlacedInScene++;
            if (CompareTag("Chicken")|| CompareTag("Cow"))
            {
                var animal = GetComponent<WalkerScripts.ChickenMovement>();
                animal.enabled = true;
                var pos = Vector3Int.FloorToInt(transform.position);
                var chickenCoop = BuildingManager.inst.structureDictionary[pos];
                var script = chickenCoop.GetComponent<ChickenAnimationController>();
                script.AddAnimals(animal);
            }
            BuildingManager.inst.SaveNewlyAddedBuildingToLists(this);
            BuildingManager.inst.AddBuilding(this);
            EventManager.SetData("CoinChange", -buildingAttributes.cost);
            EventManager.EmitEvent("CoinChange");
            EventManager.SetData("BuildingPlaced",buildingAttributes.displayName);
            EventManager.EmitEvent("BuildingPlaced");
        }

        private bool IsBuildingInValidSpot()
        {
            if (buildingAttributes.manualPlacement == false)
            {
                var bounds = rend.bounds;
                var maxPoint = bounds.max;
                var startingPoint= new Vector3Int((int) Mathf.Round(maxPoint.x),0,(int) Mathf.Round(maxPoint.z));
                var minPoint = bounds.min;
                var endingPoint= new Vector3Int((int) Mathf.Round(minPoint.x),0,(int) Mathf.Round(minPoint.z));
                var list=Utility.MakeAListBetweenPositions(startingPoint, endingPoint);
                return Utility.IsListClearOfBuildings(list, buildingAttributes.allowedCellTypes);
            }
            else
            {
                var pos = Vector3Int.FloorToInt(transform.position);
                var topLeftPoint = new Vector3Int(pos.x + buildingAttributes.spaceTakenUp, 0,
                    pos.z- buildingAttributes.spaceTakenLeft );
                var bottomRightPoint = new Vector3Int(pos.x - buildingAttributes.spaceTakenDown, 0,
                    pos.z + buildingAttributes.spaceTakenRight);
                var list = Utility.MakeAListBetweenPositions(topLeftPoint, bottomRightPoint);
                return Utility.IsListClearOfBuildings(list, buildingAttributes.allowedCellTypes);
            }
        }

     
        
    }
}
