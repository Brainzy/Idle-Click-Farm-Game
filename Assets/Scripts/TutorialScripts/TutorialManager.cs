using System.Collections.Generic;
using GameManagerScripts;
using MEC;
using UnityEngine;

namespace TutorialScripts
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private WelcomeDialog welcomeDialog;
        [SerializeField] private FirstHarvest firstHarvest;
        [SerializeField] private FirstPlanting firstPlanting;
        [SerializeField] private HarvestLogicExplanation harvestExplanation;
        [SerializeField] private RepairHouse repairHouse;
        [SerializeField] private RepairHouse repairBarn;
        [SerializeField] private RepairHouse repairSilo;
        [SerializeField] private RepairHouse repairCompostShed;
        [SerializeField] private PurchaseChicken purchaseChicken;
        [SerializeField] private CollectEggs collectEggs;
        [SerializeField] private VisitorAppearance firstVisitorNamesFarm;
        [SerializeField] private RoadSideShop roadSideShop;
        [SerializeField] private VisitorAppearance firstProducePurchaseByVillager;
        [SerializeField] private WelcomeDialog keepProducing;
        [SerializeField] private JobBoardTutorial jobBoardTutorial;
        [SerializeField] private PurchaseCowFarm purchaseCowFarm;
        [SerializeField] private FriendBarAndFriendRoadSideShop friendBarIntroduction;
        [SerializeField] private Visitor1End visitor1End;

        [SerializeField] private List<int> tutorialExperienceAmounts;

        public static TutorialManager inst;
        private List<ITutorial> tutorialScripts;
        private int tutorialIndex;

        private void Awake()
        {
            inst = this;
        }

        public interface ITutorial
        {
            void RunTutorial();
            void DisableSelf();
        }

        private void Start()
        {
            tutorialScripts = new List<ITutorial>
            {
                welcomeDialog, firstHarvest, firstPlanting, harvestExplanation, repairHouse, repairBarn, repairSilo,
                repairCompostShed,
                purchaseChicken, collectEggs, firstVisitorNamesFarm, roadSideShop, firstProducePurchaseByVillager,
                keepProducing, jobBoardTutorial,
                purchaseCowFarm, friendBarIntroduction, visitor1End
            };
            tutorialIndex =
                ES3.KeyExists("TutorialIndex") ? ES3.Load<int>("TutorialIndex") : 0; //ToDo switch to cloud solution
            for (int i = 0; i < tutorialScripts.Count; i++)
            {
                if (ReferenceEquals(tutorialScripts[i], repairHouse))
                {
                    if (tutorialIndex > i)
                    {
                        repairHouse.DespawnOldBarnSpawnNew();
                    }
                }

                if (ReferenceEquals(tutorialScripts[i], repairBarn))
                {
                    if (tutorialIndex > i)
                    {
                        repairBarn.DespawnOldBarnSpawnNew();
                    }
                }

                if (ReferenceEquals(tutorialScripts[i], repairSilo))
                {
                    if (tutorialIndex > i)
                    {
                        repairSilo.DespawnOldBarnSpawnNew();
                    }
                }

                if (ReferenceEquals(tutorialScripts[i], repairCompostShed))
                {
                    if (tutorialIndex > i)
                    {
                        repairCompostShed.DespawnOldBarnSpawnNew();
                    }
                }
            }

            for (int i = 0; i < tutorialScripts.Count; i++)
            {
                if (i <= tutorialIndex)
                {
                    tutorialScripts[i].DisableSelf();
                }
            }

            GoToNextTutorial();
        }

        private void GoToNextTutorial()
        {
            if (tutorialIndex >= tutorialScripts.Count) return;
            Timing.RunCoroutine(RunTutorialAfterSeconds());
        }
        
        private IEnumerator<float> RunTutorialAfterSeconds()
        {
            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitForOneFrame;
            tutorialScripts[tutorialIndex].RunTutorial();
            print("pokrecem tutorial " + tutorialIndex+tutorialScripts[tutorialIndex].ToString());
        }
        

        public void TutorialComplete()
        {
            print(tutorialIndex + " je pre ");
            if (tutorialIndex < tutorialScripts.Count)
            {
                tutorialScripts[tutorialIndex].DisableSelf();
            }
            ExperienceManager.inst.AddExperience(tutorialExperienceAmounts[tutorialIndex]);
            tutorialIndex++;
            print(tutorialIndex + " je posle ");
            ES3.Save("TutorialIndex",tutorialIndex);
            GoToNextTutorial();
        }
    
    }
}
