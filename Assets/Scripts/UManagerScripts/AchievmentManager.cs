using System.Collections.Generic;
using System.Text;
using ChanglebleParameters;
using GameManagerScripts;
using TigerForge;
using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class AchievmentManager : MonoBehaviour
    {
        [SerializeField] private GameObject achievmentCanvas;
        [SerializeField] private List<TextMeshProUGUI> textStatus;

        private List<int> achievmentCounter = new List<int> {0, 0, 0, 0};
        private StringBuilder stringBuilder = new StringBuilder();
        private List<int> tiersAchieved = new List<int>();
        public static AchievmentManager inst;

        private void Awake()
        {
            inst = this;
        }

        private void Start()
        {
            EventManager.StartListening("StartTruckDelivery", OnStartTruckDelivery);
            EventManager.StartListening("VillagerPurchaseSuccess", OnVillagerPurchaseSuccess);
            EventManager.StartListening("Cow", OnCow);
            EventManager.StartListening("Palmito", OnPalmito);
        }

        private void OnStartTruckDelivery()
        {
            achievmentCounter[0] += 1;
            ES3.Save("achievmentCounter", achievmentCounter);
        }

        private void OnVillagerPurchaseSuccess()
        {
            achievmentCounter[1] += 1;
            ES3.Save("achievmentCounter", achievmentCounter);
        }

        private void OnCow()
        {
            achievmentCounter[2] += 1;
            ES3.Save("achievmentCounter", achievmentCounter);
        }

        private void OnPalmito()
        {
            achievmentCounter[3] += 1;
            ES3.Save("achievmentCounter", achievmentCounter);
        }

        private void UpdateTexts()
        {
            CalculateAchievedTier();
            for (int i = 0; i < textStatus.Count; i++)
            {
                print("petlja stigla do " + i + " " + tiersAchieved.Count + " " + textStatus.Count + " " +
                      AchievmentParameters.inst.achievmentList[i].Count);
                stringBuilder.Append(tiersAchieved[i]).Append("/")
                    .Append(AchievmentParameters.inst.achievmentList[i].Count);
                textStatus[i].SetText(stringBuilder);
                stringBuilder.Clear();
            }
        }

        private void CalculateAchievedTier()
        {
            tiersAchieved.Clear();
            for (int i = 0; i < achievmentCounter.Count; i++)
            {
                for (int j = AchievmentParameters.inst.achievmentList[i].Count - 1; j >= 0; j--)
                {
                    if (achievmentCounter[i] >= AchievmentParameters.inst.achievmentList[i][j].x)
                    {
                        tiersAchieved.Add(AchievmentParameters.inst.achievmentList[i][j].y);
                        break;
                    }
                    if (j==0) tiersAchieved.Add(0);
                }
            }
        }

        public void OpenAchievments()
        {
            var currentLevel = ExperienceManager.inst.ReturnCurrentLevel();
            if (currentLevel < 2) return;
            if (currentLevel < AchievmentParameters.inst.levelToShowAchievementsAt)
            {
                EventManager.EmitEvent("FailedAction");
            }
            else
            {
                achievmentCanvas.SetActive(true);
                EventManager.EmitEvent("UIClick");
                UpdateTexts();
            }
        }
    }
}