using System;
using System.Collections.Generic;
using AudioScripts;
using BuildingScripts;
using MEC;
using PaintIn3D;
using PathologicalGames;
using TigerForge;
using UnityEngine;

namespace TutorialScripts
{
    public class RepairHouse : MonoBehaviour, TutorialManager.ITutorial
    {    
        public bool shownDialog;
        [SerializeField] private string dialog =
            "Tap on your barn and paint it by dragging the brush";
        [SerializeField] private P3dPaintable paintableBarn;
        [SerializeField] private Transform barnFixedPrefab;
        [SerializeField] private Transform oldBarn;
        [SerializeField] private GameObject myPaintObject;
        [SerializeField] private P3dChangeCounter counter;
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private float amountToBePaintedToFinishThisTutorial;

        private int cutTreeAmount;
        private bool finishedPainting;
        private float previusRatio=1;
        private DateTime timeOfLastSound;
        private const float paintSoundDuration=2.3f;
        
        
        public void RunTutorial()
        {
            if (shownDialog) return;
            TutorialSharedResource.inst.ActivateDialog();
            TutorialSharedResource.inst.text.SetText(dialog);
            shownDialog = true;
            EventManager.StartListening("MouseDown",OnMouseDown);
        }

        public void DisableSelf()
        {
            enabled = false;
        }

        private void OnMouseDown()
        {
            TutorialSharedResource.inst.DeactivateDialog(oldBarn.gameObject);
            EventManager.StopListening("MouseDown", OnMouseDown);
            EventManager.StartListening("BarnPainted",OnBarnPainted);
            EventManager.StartListening("DraggingTool",OnDraggingTool);
            EventManager.StartListening("ReleasedTool",OnReleasedTool);
            myPaintObject.SetActive(true);
            Timing.RunCoroutine(PaintingListener());
        }
        
        private IEnumerator<float> PaintingListener()
        {
            timeOfLastSound=DateTime.MinValue;
            while (finishedPainting == false)
            {
                var currentRatio = counter.Ratio;
                if (currentRatio <previusRatio && Math.Abs(currentRatio) > 0.1f)
                {
                    print("ofarbano za sada " + currentRatio);
                    previusRatio = currentRatio;
                    if (DateTime.Now.Subtract(timeOfLastSound).Seconds > paintSoundDuration)
                    {
                        timeOfLastSound=DateTime.Now;
                        soundManager.PaintingInProgress();
                    }
                    if (currentRatio <= amountToBePaintedToFinishThisTutorial)
                    {
                        OnBarnPainted();
                        finishedPainting = true;
                    }
                }
                yield return Timing.WaitForOneFrame;
            }
        }

        private void OnReleasedTool()
        {
            paintableBarn.enabled = false;
        }

        private void OnDraggingTool()
        {
            var takenString = EventManager.GetString("DraggingTool");
            if (takenString.Equals(BuildingManager.ToolName.Paint.ToString()))
            {
                paintableBarn.enabled = true; 
            }
        }

        public void DespawnOldBarnSpawnNew()
        {
            oldBarn.gameObject.SetActive(false);
            var spawned = PoolManager.Pools["Buildings"].Spawn(barnFixedPrefab);
            spawned.position = oldBarn.position;
            spawned.rotation = oldBarn.rotation;
        }
        
        private void OnBarnPainted()
        {
            CloseAllUTabs.inst.CloseEverything();
            myPaintObject.SetActive(false);
            TutorialManager.inst.TutorialComplete();
            DespawnOldBarnSpawnNew();
            print("gotov tutorial ");
        }
    
    }
}
