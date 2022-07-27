using System.Text;
using ChanglebleParameters;
using TigerForge;
using TMPro;
using UnityEngine;

namespace GameManagerScripts
{
    public class ExperienceManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI experienceDisplayText;
        [SerializeField] private RectTransform experienceBar; // 176 is max

        private const float maxExpBarValue = 176;
        private int experienceAmount;
        private int playerLevel;
        private int experienceForNextLevel;
        private StringBuilder stringBuilder;
        public static ExperienceManager inst;

        //ToDo implementation of taking leveling event and getting and saving new level
        private void Awake()
        {
            inst = this;
            stringBuilder= new StringBuilder();
            experienceAmount = ES3.KeyExists("ExperienceAmount") ? ES3.Load<int>("ExperienceAmount") : 0; //ToDo switch to cloud solution
        }

        private void Start()
        {
            playerLevel = FindPlayerLevel();
            experienceForNextLevel = ExperienceParameters.inst.experiencePointForLevel.Length > playerLevel ? ExperienceParameters.inst.experiencePointForLevel[playerLevel] : 999999999;
            SetTextAndBar();
        }

        public int ReturnCurrentLevel()
        {
            return playerLevel;
        }

        private void SetTextAndBar()
        {
            experienceDisplayText.SetText(stringBuilder.Append(experienceAmount).Append("/").Append(experienceForNextLevel));
            stringBuilder.Clear();
            var expWidth = maxExpBarValue * experienceAmount / experienceForNextLevel;
            experienceBar.sizeDelta= new Vector2(expWidth,11);
        }

        public void AddExperience(int amount)
        {
            experienceAmount += amount;
            if (experienceAmount >= experienceForNextLevel)
            {
                playerLevel++;
                print("sada sam level " + playerLevel);
                EventManager.EmitEvent("LeveledUp");
                experienceForNextLevel = ExperienceParameters.inst.experiencePointForLevel.Length > playerLevel ? ExperienceParameters.inst.experiencePointForLevel[playerLevel] : 999999999;
            }
            SetTextAndBar();
            ES3.Save("ExperienceAmount",experienceAmount);
        }
        private int FindPlayerLevel()
        {
            for (int i = ExperienceParameters.inst.experiencePointForLevel.Length - 1; i >= 0; i--)
            {
                if (experienceAmount > ExperienceParameters.inst.experiencePointForLevel[i])
                {
                    return i+1;
                }
            }
            return 0;
        }
    }
}
