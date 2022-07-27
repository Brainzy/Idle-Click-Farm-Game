using SpawnManagers;
using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class VisitorAppearance : MonoBehaviour, TutorialManager.ITutorial
    {
        [SerializeField] private SpawnBezierWalker mySpawner;
        [SerializeField] private string eventNameToEndTutorial="VisitorNamedFarm";
        [SerializeField] private bool shouldStartAnotherSpawner;
        [SerializeField] private SpawnBezierWalker anotherSpawner;


        private bool startedSpawner;
    
        public void RunTutorial()
        {
            if (startedSpawner) return;
            startedSpawner = true;
            mySpawner.StartSpawner();
            EventManager.StartListening(eventNameToEndTutorial, OnVisitorNamedFarm);
        }

        private void OnVisitorNamedFarm()
        {
            TutorialManager.inst.TutorialComplete();
            print("gotov tutorial");
            EventManager.StopListening(eventNameToEndTutorial, OnVisitorNamedFarm);
            if (shouldStartAnotherSpawner) anotherSpawner.StartSpawner();
        }

        public void DisableSelf()
        {
            enabled = false;
        }
    }
}
