using System;
using System.Collections.Generic;
using BuildingScripts;
using MEC;
using PathCreation;
using PathologicalGames;
using TigerForge;
using UnityEngine;
using WalkerScripts;
using Random = UnityEngine.Random;


namespace SpawnManagers
{
   public class SpawnBezierWalker : MonoBehaviour
   {
      public Transform prefabToSpawn;
      public float spawnIntervalInSeconds;
      public float speed;
      public Vector3 rotationAdjustment;
      public PathCreator myIncommingPath;
      public PathCreator myOutgoingPath;
      public bool oneTimeSpawningOnly;
      public string walkerTagForStoppingSameTypes;

      public bool testSpawn;
      private Transform mySpawnedObject;
      private WalkerOnBezierPath myScript;
      private const float oneSecond = 1f;
      private bool shoulStartSpawningMyWalker;
      private CoroutineHandle spawnerCo;
      

      private void Start()
      {
         EventManager.StartListening("AddWalker",OnAddWalker);
         if (ES3.KeyExists(prefabToSpawn.name))
         {
            var lastTimeSpawned = ES3.Load<DateTime>(transform.name);
            if (DateTime.Now.Subtract(lastTimeSpawned).TotalSeconds > spawnIntervalInSeconds)
            {
               StartSpawner();
            }
         }
      }

      private void OnAddWalker()
      {
         var receivedString = EventManager.GetString("AddWalker");
         if (receivedString != prefabToSpawn.name) return;
         StartSpawner();
      }

      private void Update() //Todo temp func delete later
      {
         if (testSpawn)
         {
            testSpawn = false;
            StartSpawner();
         }
      }

      public void StartSpawner()
      {
         if (spawnerCo.IsRunning) return;
         spawnerCo=Timing.RunCoroutine(RunSpawner());
         Debug.Log("startujem spawn");
         if (oneTimeSpawningOnly) return;
         ES3.Save(transform.name,DateTime.Now);
      }

      private IEnumerator<float> RunSpawner()
      {
         while (true)
         {
            yield return Timing.WaitForSeconds(Random.Range(0f, 2f));
            while (ActiveWalkersTracker.inst.activeTags.Contains(walkerTagForStoppingSameTypes))
            {
               yield return Timing.WaitForOneFrame;
            }
            ActiveWalkersTracker.inst.AddTagToList(walkerTagForStoppingSameTypes);
            yield return Timing.WaitForSeconds(spawnIntervalInSeconds);
            SpawnMyPrefab();
            while (mySpawnedObject.gameObject.activeSelf)
            {
               yield return Timing.WaitForSeconds(oneSecond);
            }

            if (oneTimeSpawningOnly)
            {
               yield break;
            }
         }
      }

      private void SpawnMyPrefab()
      {
         mySpawnedObject = PoolManager.Pools["Buildings"].Spawn(prefabToSpawn);
         myScript = mySpawnedObject.GetComponent<WalkerOnBezierPath>();
         myScript.pathCreator = myIncommingPath;
         myScript.mySpawner = this;
         myScript.walkerTagForStoppingSameTypes = walkerTagForStoppingSameTypes;
         print("spawnujem " + mySpawnedObject.name);
         myScript.returnPath = myOutgoingPath;
         myScript.stopped = false;
         myScript.speed = speed;
         myScript.rotationAdjustment = rotationAdjustment;
         myScript.distanceTravelled = 0;
         myScript.StartMovement();
      }
      

   }
}
