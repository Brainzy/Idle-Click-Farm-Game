using System;
using System.Collections.Generic;
using System.Linq;
using InteractionScripts;
using PathologicalGames;
using ScriptableObjectMakingScripts;
using UnityEngine;
using WalkerScripts;

namespace BuildingScripts
{
    public partial class BuildingManager : MonoBehaviour
    {
        public Dictionary<Vector3Int, Building> structureDictionary = new Dictionary<Vector3Int, Building>();
        public Dictionary<Vector3Int, CellType> specialDictionary = new Dictionary<Vector3Int, CellType>();
        public Grid placementGrid;
        public static BuildingManager inst;
        public Building[] startingBuildings;
        public List<Vector3> storingLocations;
        public Building startingChickenCoop;

        public BuildingAttributes[] allBuildings;
        public List<Transform> allPrefabs = new List<Transform>(); //temp public for debug
        public List<string> savedBuildingNames = new List<string>();
        public List<Vector3> savedBuildingPositions = new List<Vector3>();
        public List<Building> savedBuildings= new List<Building>();
        public List<DateTime> savedDates= new List<DateTime>();

        private List<Building> tempAnimalList = new List<Building>();
        private List<Building> tempFarmList= new List<Building>();
        private List<Vector3> tempFarmPos= new List<Vector3>();

        private void SaveGame()
        {
            ES3.Save("SavedBuildingNames", savedBuildingNames);
            ES3.Save("SavedBuildingPositions", savedBuildingPositions);
            savedDates.Clear();
            for (int i = 0; i < savedBuildings.Count; i++)
            {
                savedDates.Add(savedBuildings[i].startedDate);
            }
            ES3.Save("SavedDates", savedDates);
        }
        
        //deo zgrada je vec na mapi po defaultu to su starting buildings i one se koriste radi blokiranje nelegalne gradnje
        // structureDictionary se koristi samo kod trazenja 
        //deo tih zgrada ce se menjati

        private void Awake()
        {
            inst = this;
            placementGrid = new Grid(1000, 1000); // ToDo this is hardcoded for now
            structureDictionary = ES3.KeyExists("StructureDictionary")
                ? ES3.Load<Dictionary<Vector3Int, Building>>("StructureDictionary")
                : structureDictionary;
            specialDictionary = ES3.KeyExists("SpecialDictionary")
                ? ES3.Load<Dictionary<Vector3Int, CellType>>("SpecialDictionary")
                : specialDictionary;
            MakeStartingLocationsForStoringAnimations();
            MakePrefabList();
        }

        private void Start()
        {
            for (int i = 0; i < startingBuildings.Length; i++) // adding initial buildings to list
            {
                AddBuilding(startingBuildings[i]);
            }
            LoadSavedBuildings();
        }

        private void MakePrefabList()
        {
            for (int i = 0; i < allBuildings.Length; i++)
            {
                allPrefabs.Add(allBuildings[i].myPrefab);
                for (int j = 0; j < allBuildings[i].myGrowablePrefabs.Count; j++)
                {
                    if (allPrefabs.Contains(allBuildings[i].myGrowablePrefabs[j])) continue;
                    allPrefabs.Add(allBuildings[i].myGrowablePrefabs[j]);
                }
            }
        }

        public void SaveNewlyAddedBuildingToLists(Building building)
        {
           savedBuildingNames.Add(building.buildingAttributes.myPrefab.name);
           savedBuildingPositions.Add(building.transform.position);
           savedBuildings.Add(building);
           SaveGame();
        }

        public void ChangedBuilding(Building newBuilding)
        {
            var savedTransform = newBuilding.transform;
            var pos = savedTransform.position;
            var index = savedBuildingPositions.FindIndex(s => s == pos);
            if (index != -1)
            {
                savedBuildingNames[index] = savedTransform.name.Split('(')[0];
                savedBuildings[index] = newBuilding;
            }
        }
        
        

        private void LoadSavedBuildings()
        {
            savedBuildingNames = ES3.KeyExists("SavedBuildingNames")
                ? ES3.Load<List<string>>("SavedBuildingNames")
                : savedBuildingNames;
            savedBuildingPositions = ES3.KeyExists("SavedBuildingPositions")
                ? ES3.Load<List<Vector3>>("SavedBuildingPositions")
                : savedBuildingPositions;
            savedDates = ES3.KeyExists("SavedDates")
                ? ES3.Load<List<DateTime>>("SavedDates")
                : savedDates;
            print("idem u loadovanje zgrada " + savedBuildingNames.Count+" "+savedBuildingPositions.Count+" "+savedDates.Count);
            for (int i = 0; i < savedBuildingNames.Count; i++)
            {
                //print("pokusavam nesto sa ");
                //print(FindPrefabBasedOnName(savedBuildingNames[i]));
                var myPrefab = FindPrefabBasedOnName(savedBuildingNames[i]);
                var spawned = PoolManager.Pools["Buildings"].Spawn(myPrefab);
                spawned.position = savedBuildingPositions[i];
                var building = spawned.GetComponent<Building>();
                if (building.buildingAttributes.isGrowable)
                {
                    for (int j = 0; j < building.buildingAttributes.myGrowablePrefabs.Count; j++)
                    {
                        if (myPrefab.name.Equals(building.buildingAttributes.myGrowablePrefabs[j].name))
                        {
                            spawned.rotation = building.buildingAttributes.myGrowablePrefabs[j].rotation;
                        }
                    }
                }
                else
                {
                    spawned.rotation = building.buildingAttributes.myPrefab.rotation;
                }

                if (building.buildingAttributes.isAnimal)   tempAnimalList.Add(building);
                if (building.buildingAttributes.makesCellTypes.Length > 0)
                {
                    tempFarmList.Add(building);
                    tempFarmPos.Add(building.transform.position);
                }
                savedBuildings.Add(building);
                tempFarmList.Add(startingChickenCoop);
                tempFarmPos.Add(startingChickenCoop.transform.position);
                building.UpdatedTimerOfflineTimePassed(savedDates[i]);
            }

            AddAnimalsToBuildings();
        }

        private void AddAnimalsToBuildings()
        {
            for (int i = 0; i < tempAnimalList.Count; i++)
            {
                var animalPos = tempAnimalList[i].transform.position;
                float smallest= Single.PositiveInfinity;
                var index = 0;
                for (int j = 0; j < tempFarmPos.Count; j++)
                {
                    var dist = Math.Abs(Vector3.Distance(animalPos, tempFarmPos[j]));
                    if (dist<smallest)
                    {
                        smallest = dist;
                        index = j;
                    }
                }
                tempFarmList[index].GetComponent<ChickenAnimationController>().AddAnimals(tempAnimalList[i].GetComponent<ChickenMovement>());
            }
        }
        
        private Transform FindPrefabBasedOnName(string prefabName)
        {
            //print("trazi se " + prefabName);
            for (int i = 0; i < allPrefabs.Count; i++)
            {
                //print("poredim sa " + allPrefabs[i]);
                if (allPrefabs[i].name.Equals(prefabName))
                {
                    return allPrefabs[i];
                }
            }

            return null;
        }

        private void MakeStartingLocationsForStoringAnimations()
        {
            var storageNames = System.Enum.GetNames(typeof(StorageLocation));

            for (int i = 0; i < startingBuildings.Length; i++)
            {
                for (int j = 0; j < storageNames.Length; j++)
                {
                    if (startingBuildings[i].name.Contains(storageNames[j]))
                    {
                        storingLocations.Add(startingBuildings[i].transform.position);
                    }
                }
            }
        }

        public void AddBuilding(Building building)
        {
            if (building.buildingAttributes.isAnimal) return;
            var pos = Vector3Int.FloorToInt(building.transform.position);
            if (building.buildingAttributes.manualPlacement == false)
            {
                AutoPlacementByRenderSize(building);
            }
            else
            {
                ManualPositioning(building, pos);
            }
        }

        private void ManualPositioning(Building building, Vector3Int pos)
        {
            var topLeftPoint = new Vector3Int(pos.x + building.buildingAttributes.spaceTakenUp, 0,
                pos.z - building.buildingAttributes.spaceTakenRight);
            var bottomRightPoint = new Vector3Int(pos.x - building.buildingAttributes.spaceTakenDown, 0,
                pos.z + building.buildingAttributes.spaceTakenLeft);
            var list = Utility.MakeAListBetweenPositions(topLeftPoint, bottomRightPoint);
            for (int j = 0; j < list.Count; j++)
            {
                AddPositionToDictionary(building, list, j);
            }
        }

        private void AutoPlacementByRenderSize(Building building)
        {
            var bounds = building.rend.bounds;
            var maxPoint = bounds.max;
            var startingPoint = new Vector3Int((int) Mathf.Round(maxPoint.x), 0, (int) Mathf.Round(maxPoint.z));
            var minPoint = bounds.min;
            var endingPoint = new Vector3Int((int) Mathf.Round(minPoint.x), 0, (int) Mathf.Round(minPoint.z));
            var list = Utility.MakeAListBetweenPositions(startingPoint, endingPoint);
            for (int j = 0; j < list.Count; j++)
            {
                AddPositionToDictionary(building, list, j);
            }
        }

        private void AddPositionToDictionary(Building building, List<Vector3Int> list, int j)
        {
            var posFromList = list[j];
            structureDictionary.Add(posFromList, building);
            placementGrid[posFromList.x, posFromList.z] = CellType.Building;
            if (building.buildingAttributes.makesCellTypes.Length > 0)
            {
                specialDictionary.Add(posFromList, building.buildingAttributes.makesCellTypes[0]);
//                print("ubacena pozicija " + posFromList + " na specijalan recnik " + specialDictionary.Count);
            }

            //specialStructureDictionary.Add(posFromList,building.buildingAttributes.sp);
            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); //debugging spawning cube
            //cube.transform.position = posFromList;
        }
    }
}