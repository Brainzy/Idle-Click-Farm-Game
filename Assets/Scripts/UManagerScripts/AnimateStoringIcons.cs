using System.Collections.Generic;
using ChanglebleParameters;
using DG.Tweening;
using GameManagerScripts;
using MEC;
using PathologicalGames;
using UnityEngine;

namespace UManagerScripts
{
    public class AnimateStoringIcons : MonoBehaviour
    {
        [SerializeField] private GameObject myCanvas; // activate bar for animation
        [SerializeField] private Transform mainUICanvas;
        [SerializeField] private GameObject[] barIcons; // icon indicating which bar is being animated
        public Transform[] iconPrefabs; // prefabs to spawn to move to barIcon and despawn
        [SerializeField] private Animator[] iconAnimators;
        [SerializeField] private RectTransform barScale;
     

        private List<CoroutineHandle> coRoutines = new List<CoroutineHandle>();

        public void AnimateIcon(int indexOfIcon, string prefabName, Vector3 pos, float percentageOfFill)
        {
            myCanvas.SetActive(true);
            
            for (int i = 0; i < barIcons.Length; i++)
            {
                if (i == indexOfIcon)
                {
                    barIcons[i].SetActive(true);
                }
                else
                {
                    barIcons[i].SetActive(false);
                }
            }
            barScale.localScale= new Vector3(percentageOfFill, 1, 1);
//            print("spawnovacu " + iconPrefabs[FindIcon(prefabName)].name);
            var spawned = PoolManager.Pools["Buildings"].Spawn(iconPrefabs[FindIcon(prefabName)]);
            Vector3 topRight = GameManager.inst.cam.WorldToScreenPoint(new Vector3(pos.x,2,pos.z));
            Vector3 botLeft =  GameManager.inst.cam.WorldToScreenPoint(new Vector3(pos.x,0,pos.z));
            spawned.position = topRight;
            spawned.SetParent(mainUICanvas);
            spawned.DOMove(botLeft, AnimationParameters.inst.storingFallSpeedToGround).OnComplete((() =>
            {
                spawned.DOMove(barIcons[indexOfIcon].transform.position, AnimationParameters.inst.storingFlightSpeedTowardsBar).OnComplete((
                    delegate
                    {
                        iconAnimators[indexOfIcon].Play("Pressed");
                    }));
            }));
            var coRo = Timing.RunCoroutine(DespawnSpawned(spawned));
            coRoutines.Add(coRo);
        }

        public int FindIcon(string prefabName)
        {
            for (int i = 0; i < iconPrefabs.Length; i++) 
            {  
                if (prefabName.Equals(iconPrefabs[i].name))
                {
                    return i;
                }
            }
            return 0;
        }

        private IEnumerator<float> DespawnSpawned(Transform spawned)
        {
            var waitTime = AnimationParameters.inst.storingFallSpeedToGround + AnimationParameters.inst.storingFlightSpeedTowardsBar;
            yield return Timing.WaitForSeconds(waitTime);
            PoolManager.Pools["Buildings"].Despawn(spawned);
            coRoutines.RemoveAt(0);
            if (coRoutines.Count == 0) myCanvas.SetActive(false);
        }
    }
}