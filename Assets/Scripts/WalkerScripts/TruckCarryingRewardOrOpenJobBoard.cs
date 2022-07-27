using BuildingScripts;
using GameManagerScripts;
using TigerForge;
using UManagerScripts;
using UnityEngine;

namespace WalkerScripts
{
    public class TruckCarryingRewardOrOpenJobBoard : MonoBehaviour, BuildingManager.IClickOnBuilding
    {
        public int truckCarryGold;
        public int truckCarryXp;
        public GameObject rewardObject;
        private void OnEnable()
        {
            truckCarryGold = ES3.KeyExists("truckCarryGold") ? ES3.Load<int>("truckCarryGold") : 0; 
            truckCarryXp = ES3.KeyExists("truckCarryXp") ? ES3.Load<int>("truckCarryXp") : 0;
            if (truckCarryXp > 0)
            {
                rewardObject.SetActive(true);
            }
            else
            {
                rewardObject.SetActive(false);
            }
        }

        public void ClickedOnBuilding()
        {
            if (truckCarryXp == 0)
            {
                JobBoardManager.inst.OpenCanvas();
            }
            else
            {
                EventManager.EmitEvent("Confirmation");
                rewardObject.SetActive(false);
                ExperienceManager.inst.AddExperience(truckCarryXp);
                CoinManager.inst.AddCoins(truckCarryGold);
                truckCarryGold = 0;
                truckCarryXp = 0;
                ES3.Save("truckCarryGold",0);
                ES3.Save("truckCarryXp",0);
                EventManager.EmitEvent("TruckRewardPickedUp");
            }
        }
        
        
    }
}
