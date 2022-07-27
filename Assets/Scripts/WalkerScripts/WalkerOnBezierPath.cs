using System.Collections.Generic;
using BuildingScripts;
using MEC;
using PathCreation;
using PathologicalGames;
using SpawnManagers;
using TigerForge;
using UnityEngine;

namespace WalkerScripts
{
    public class WalkerOnBezierPath : MonoBehaviour
    {
        [SerializeField] private Animator[] myAnimators;
        [SerializeField] private string eventNameToWalkToDespawn;
        [HideInInspector] public float distanceTravelled;
        public string walkerTagForStoppingSameTypes;
        public SpawnBezierWalker mySpawner;
        public float speed;
        public PathCreator pathCreator;
        public bool stopped;
        [HideInInspector] public Vector3 rotationAdjustment;
        public PathCreator returnPath;
        public bool deactivateAnimatorsOnMoveEnd;
        public VisitorAnimationController visitorAnimationController;
        
        private void OnEnable()
        {
            gameObject.layer = 0;
            EventManager.StartListening(eventNameToWalkToDespawn,FinishedInteraction);
            Timing.RunCoroutine(WaitFrameBeforeAddition());
        }

        private IEnumerator<float> WaitFrameBeforeAddition()
        {
            yield return Timing.WaitForOneFrame;
            ActiveWalkersTracker.inst.AddToList(mySpawner);
        }
        private void Update()
        {
            if (stopped) return;
            distanceTravelled += speed * Time.deltaTime;
           // print("moja pozicija je " + transform.position);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            var pathRotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop)
                .eulerAngles;
            pathRotation = pathRotation + rotationAdjustment;
            transform.rotation = Quaternion.Euler(pathRotation);

            if (transform.position == pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1))
            {
               // print("kraj mog puta ");
                if (pathCreator == returnPath)
                {
                    DespawnSelf();
                }
                else
                {
                    ActivateInteractivity();
                    EventManager.EmitEvent(tag);
                    if (deactivateAnimatorsOnMoveEnd) ChangeAnimatorStatus(false);
                    else visitorAnimationController.PlayIdleAnimation();
                    stopped = true;
                }

                pathCreator = returnPath;
                distanceTravelled = 0;
            }
        }
        public void StartMovement()
        {
            if (deactivateAnimatorsOnMoveEnd)  ChangeAnimatorStatus(true);
            else visitorAnimationController.PlayWalkAnimation();
        }
        
        private void FinishedInteraction()
        {
            print("completed interaction");
            gameObject.layer = 0;
            if (deactivateAnimatorsOnMoveEnd)  ChangeAnimatorStatus(true);
            else visitorAnimationController.PlayCelebrateAnimation();
            if (deactivateAnimatorsOnMoveEnd) stopped = false;
        }

        public void FinishedCelebrateAnimation()
        {
            stopped = false;
        }

        private void DespawnSelf()
        {
            pathCreator = null;
            returnPath = null;
            EventManager.StopListening(eventNameToWalkToDespawn,FinishedInteraction);
            ActiveWalkersTracker.inst.RemoveFromList(mySpawner);
            ActiveWalkersTracker.inst.RemoveTagFromList(walkerTagForStoppingSameTypes);
            PoolManager.Pools["Buildings"].Despawn(transform);
        }

        private void ActivateInteractivity()
        {
            gameObject.layer = 9;
        }

        private void ChangeAnimatorStatus(bool activate)
        {
            for (int i = 0; i < myAnimators.Length; i++)
            {
                myAnimators[i].enabled = activate;
            }
        }
    }
}