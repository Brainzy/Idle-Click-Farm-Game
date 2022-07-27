using System.Collections.Generic;
using SpawnManagers;
using UnityEngine;

namespace BuildingScripts
{
   public class ActiveWalkersTracker : MonoBehaviour
   {
      public List<SpawnBezierWalker> allSpawnersList;
   
      private List<string> activeSpawnList = new List<string>();
      public List<string> activeTags= new List<string>();
   
      public static ActiveWalkersTracker inst;

      private void Awake()
      {
         inst = this;
      }

      private void Start()
      {
         if (ES3.KeyExists("activeSpawnList"))
         {
            activeSpawnList = ES3.Load<List<string>>("activeSpawnList");
         }
         for (int i = 0; i < activeSpawnList.Count; i++)
         {
            print("trazi se " + activeSpawnList[i]);
            FindSpawner(activeSpawnList[i]).StartSpawner();
         }

         print(activeSpawnList.Count);
      }

      private SpawnBezierWalker FindSpawner(string spawnerName)
      {
         for (int i = 0; i < allSpawnersList.Count; i++)
         {
            if (spawnerName.Equals(allSpawnersList[i].name))
            {
               return allSpawnersList[i];
            }
         }

         return null;
      }

      public void AddToList(SpawnBezierWalker spawnBezierWalker)
      {
         print("dodajem " + spawnBezierWalker);
         if (activeSpawnList.Contains(spawnBezierWalker.name)) return;
         activeSpawnList.Add(spawnBezierWalker.name);
         ES3.Save("activeSpawnList",activeSpawnList);
      }
   
      public void RemoveFromList(SpawnBezierWalker spawnBezierWalker)
      {
         print("oduzimam " + spawnBezierWalker);
         if (!activeSpawnList.Contains(spawnBezierWalker.name)) return;
         activeSpawnList.Remove(spawnBezierWalker.name);
         ES3.Save("activeSpawnList",activeSpawnList);
      }
      
      public void AddTagToList(string tagString)
      {
         print("dodajem tag " + tagString + " " + activeTags.Contains(tagString));
         if (activeTags.Contains(tagString)) return;
         activeTags.Add(tagString);
      }
   
      public void RemoveTagFromList(string tagString)
      {
         if (!activeTags.Contains(tagString)) return;
         activeTags.Remove(tagString);
      }
   
   
   
   }
}
