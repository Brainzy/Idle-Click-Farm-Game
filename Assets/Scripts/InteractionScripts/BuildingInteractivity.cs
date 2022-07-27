using BuildingScripts;
using PathologicalGames;
using UManagerScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class BuildingInteractivity : MonoBehaviour, BuildingManager.IClickOnBuilding
    {
         public Building building;

        protected void DespawnAndSpawnYoungBuilding()
        {
           var spawned= PoolManager.Pools["Buildings"].Spawn(building.buildingAttributes.myGrowablePrefabs[0]);
           var transform1 = transform;
           spawned.position = transform1.position;
           spawned.rotation = building.buildingAttributes.myPrefab.rotation;
           BuildingManager.inst.ChangedBuilding(spawned.GetComponent<Building>());
           PoolManager.Pools["Buildings"].Despawn(transform1);
        }
        
        public void ClickedOnBuilding()
        {
            CheckIfShouldShowTimer();
            CheckIfShouldShowAnimalTimer();
            CheckIfShouldShowInteractibles();
        }

        private void CheckIfShouldShowInteractibles()
        {
            if (building.myPrefabIndex < building.buildingAttributes.myGrowablePrefabs.Count - 1)
                return; //Todo Current implementation only lets interactibles on final growable level
            var interactivities = building.buildingAttributes.interactivitieLinks;
            SpawnToolsForUi.inst.spawnList.Clear();
            for (int i = 0; i < interactivities.Length; i++)
            {
                SpawnToolsForUi.inst.spawnList.Add(interactivities[i].userinterfacePrefab.name);
            }
            SpawnToolsForUi.inst.SpawnToolsFromSpawnList(building);
        }

        private void CheckIfShouldShowAnimalTimer()
        {
            if (!building.buildingAttributes.isAnimal) return;
            if (building.animalIsHarvestable) return;
            ShowUTimer.inst.ShowGrowTimer(building);
        }

        private void CheckIfShouldShowTimer()
        {
            if (!building.buildingAttributes.isGrowable) return;
            if (building.myPrefabIndex > building.buildingAttributes.myGrowablePrefabs.Count - 1) return;
            ShowUTimer.inst.ShowGrowTimer(building);
        }

       
    }
}