using System.Collections.Generic;
using UnityEngine;

namespace ChanglebleParameters
{
    public class AchievmentParameters : MonoBehaviour
    {
        public int levelToShowAchievementsAt = 6;
        public List<Vector2Int> BeepBeep;
        public List<Vector2Int> Patronage;
        public List<Vector2Int> GotMilk;
        public List<Vector2Int> HyperHarvester;
        public List<List<Vector2Int>> achievmentList;

        public static AchievmentParameters inst;

        private void Awake()
        {
            inst = this;
            achievmentList = new List<List<Vector2Int>> {BeepBeep, Patronage, GotMilk,HyperHarvester };
        }
    }
}