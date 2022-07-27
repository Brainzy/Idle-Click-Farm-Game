using System.Collections.Generic;
using BuildingScripts;
using DG.Tweening;
using InteractionScripts;
using MEC;
using TigerForge;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WalkerScripts
{
    public class ChickenMovement : BuildingInteractivity, BuildingManager.IFinishedToolActionAnimal
    {
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private Rigidbody rgdBody;
        [SerializeField] private float minGrazingRandom;
        [SerializeField] private float maxGrazingRandom;
        [SerializeField] private float flyLenght = 1;
        [SerializeField] private Animator myAnimator;
        [SerializeField] private string moveAnim= "Armature|ChickenWalk";
        [SerializeField] private string almostHarvestableAnim= "Armature|ChickenEggsLaid-Start";
        [SerializeField] private string harvestableAnim= "Armature|ChickenEggsLaid-Loop";
        [SerializeField] private string grazingAnim= "Armature|ChickenGrazing";
        [SerializeField] private string eventNameForHarvestable;
        
        private float chickenGrazingDefaultDuration;
        private float chickenLayingEggAnimationDefaultDuration;
        private float flyingX;
        private float flyingY;
        private float flyingZ;
        private float grazingBeginTimer;
        private float grazingDurationTimer;
        private float fallingDownLaidEgg;
        private Vector3 direction;
        private ChickenState myPreviuousState;
        private float flyTimer;
        private float soundTimer;

        private enum TypeOfAnimal
        {
            Chicken,
            Cow
        }

        [SerializeField] private TypeOfAnimal typeOfAnimal;
   
       
        private enum ChickenState
        {
            Grazing,
            Moving,
            LayingEgg,
            EggLayed,
            Flying
        }
        private ChickenState myState;

        private void OnEnable()
        {
            transform.Rotate(direction);
            direction = (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized;
            transform.Rotate(direction);
            UpdateMyState(ChickenState.Moving);
            grazingBeginTimer = Random.Range(minGrazingRandom, maxGrazingRandom);
            building.growTimer = building.buildingAttributes.myGrowableTimers[0];
            myAnimator.Play(moveAnim);
            UpdateAnimClipTimes();
        }
        private void UpdateMyState(ChickenState newState)
        {
            myPreviuousState = myState;
            myState = newState;
        }
        
        private void UpdateAnimClipTimes()
        {
            AnimationClip[] clips = myAnimator.runtimeAnimatorController.animationClips;
            foreach(AnimationClip clip in clips)
            {
                switch(clip.name)
                {
                    case "Armature|ChickenGrazing":
                        chickenGrazingDefaultDuration = clip.length;
                        grazingDurationTimer = chickenGrazingDefaultDuration;
                        break;
                    case "Armature|ChickenEggsLaid-Start":
                        chickenLayingEggAnimationDefaultDuration = clip.length;
                        fallingDownLaidEgg = chickenLayingEggAnimationDefaultDuration;
                        break;
                    case "CowArmature|CowGrazing":
                        chickenGrazingDefaultDuration = clip.length;
                        grazingDurationTimer = chickenGrazingDefaultDuration;
                        break;
                    case "CowArmature|CowNeedMilking_Start_Anim":
                        chickenLayingEggAnimationDefaultDuration = clip.length;
                        fallingDownLaidEgg = chickenLayingEggAnimationDefaultDuration;
                        break;
                }
            }
        }

        private void Update()
        {
            grazingBeginTimer -= Time.deltaTime;
            soundTimer -= Time.deltaTime;
            if (soundTimer <= 0)
            {
                soundTimer = building.buildingAttributes.producesSoundAfterSeconds;
                EventManager.EmitEvent(building.buildingAttributes.displayName);
            }
            if (myState==ChickenState.Moving|| myState==ChickenState.Grazing) building.growTimer -= Time.deltaTime;
            if (building.growTimer <= 0)
            {
                UpdateMyState( ChickenState.LayingEgg);
                myAnimator.Play(almostHarvestableAnim);
                building.growTimer = building.buildingAttributes.myGrowableTimers[0];
            }
            if (grazingBeginTimer <= 0 && myState==ChickenState.Moving)
            {
                UpdateMyState( ChickenState.Grazing);
                myAnimator.Play(grazingAnim);
                grazingBeginTimer = Random.Range(minGrazingRandom, maxGrazingRandom);
            }
            switch (myState)
            {
                case ChickenState.Moving:
                {
                    Vector3 newPos = transform.position + direction * (speed * Time.deltaTime);
                    rgdBody.MovePosition(newPos);
                    break;
                }
                case ChickenState.Grazing:
                    grazingDurationTimer -= Time.deltaTime;
                    if (grazingDurationTimer <= 0)
                    {
                        UpdateMyState(ChickenState.Moving);
                        myAnimator.Play(moveAnim);
                       // print("pustam move anim ");
                        grazingDurationTimer = chickenGrazingDefaultDuration;
                    }
                    break;
                case ChickenState.LayingEgg:
                    fallingDownLaidEgg -= Time.deltaTime;
                    if (fallingDownLaidEgg <= 0)
                    {
                        UpdateMyState(ChickenState.EggLayed);
                        myAnimator.Play(harvestableAnim);
                        building.animalIsHarvestable = true;
                        fallingDownLaidEgg = chickenLayingEggAnimationDefaultDuration;
                        EventManager.EmitEvent(eventNameForHarvestable,1f);
                    }
                    break;
                case ChickenState.EggLayed:
                    break;
                case ChickenState.Flying:
                    flyTimer -= Time.deltaTime;
                    rgdBody.MovePosition(new Vector3(flyingX,flyingY,flyingZ));
                    if (flyTimer <= 0)
                    {
                        UpdateMyState(myPreviuousState != ChickenState.Flying ? myPreviuousState : ChickenState.Moving);
                    }
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //print("usao u sudar ");
            if (myState != ChickenState.Moving)  return;
            if (other.CompareTag(gameObject.tag))
            {
                direction = new Vector3(Random.Range(-1f, -0.2f) * direction.x, 0,
                    Random.Range(-1f, -0.2f) * direction.z).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                direction = new Vector3(-direction.x, 0, -direction.z).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        public void FinishedAction()
        {
            building.animalIsHarvestable = false;
            if (myState!=ChickenState.Flying)
            {
                EggCollected();
            }
            else
            {
                Timing.RunCoroutine(WaitForFlightEnd());
            }
           
        }

        private IEnumerator<float> WaitForFlightEnd()
        {
            while (myState == ChickenState.Flying)
            {
                yield return Timing.WaitForOneFrame;
            }
            EggCollected();
        }

        private void EggCollected()
        {
            UpdateMyState(ChickenState.Moving);
            myAnimator.Play(moveAnim);
        }

        public void FlyToLocation(Vector3 pos)
        {
            UpdateMyState(ChickenState.Flying);
            var curPos = transform.position;
            flyingX = curPos.x;
            flyingY = 0;
            flyingZ = curPos.z;
            if (typeOfAnimal == TypeOfAnimal.Chicken)
            {
                DOTween.To(() => flyingY, x => flyingY = x, 3, flyLenght/2)
                    .OnComplete((() =>  DOTween.To(() => flyingY, x => flyingY = x, 0, flyLenght/2))); 
                DOTween.To(() => flyingX, x => flyingX = x, pos.x, flyLenght);
                DOTween.To(() => flyingZ, x => flyingZ = x, pos.z, flyLenght);
            }
            flyTimer = flyLenght;
        }
        
    }
}