using System.Collections.Generic;
using MEC;
using UnityEngine;
using WalkerScripts;

namespace InteractionScripts
{
    public class ChickenAnimationController : MonoBehaviour
    {
        public int animalsInside;
        public int currentActiveSector;
        [SerializeField] private Animator myAnimator;
        [SerializeField] private float[] waitTimes;
        [SerializeField] private string[] animationNames;
        [SerializeField] private List<Transform> firstPosList= new List<Transform>();
        [SerializeField] private List<Transform> secondPosList=new List<Transform>();
        [SerializeField] private List<Transform> thirdPosList=new List<Transform>();
        [SerializeField] private List<Transform> fourthPositionList=new List<Transform>();

        private List<ChickenMovement> myAnimalList = new List<ChickenMovement>();
        private List<List<Transform>> allPosList;

        private void OnEnable()
        {
            allPosList = new List<List<Transform>> {firstPosList, secondPosList,thirdPosList,fourthPositionList};
        }

        public void AddAnimals(ChickenMovement animal)
        {
            if (animalsInside == 0) Timing.RunCoroutine(RunAnimation().CancelWith(gameObject));
            myAnimalList.Add(animal);
            animal.transform.position= ReturnRandomPositionInCurrentActiveSector();
            animalsInside++;
        }

        private IEnumerator<float> RunAnimation()
        {
            while (true)
            {
                for (int i = 0; i < animationNames.Length; i++)
                {
                    yield return Timing.WaitForSeconds(waitTimes[i]);
                    myAnimator.Play(animationNames[i]);
                    currentActiveSector = i;
                    for (int j = 0; j < myAnimalList.Count; j++)
                    {
                        myAnimalList[j].FlyToLocation(ReturnRandomPositionInCurrentActiveSector());
                    }
                }
            }
        }

        private Vector3 ReturnRandomPositionInCurrentActiveSector()
        {
            return allPosList[currentActiveSector][Random.Range(0, allPosList[currentActiveSector].Count - 1)].position;
        }
        
    }
}