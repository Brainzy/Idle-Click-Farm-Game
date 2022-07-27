using TigerForge;
using TMPro;
using UnityEngine;

namespace GameManagerScripts
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;
        public static CoinManager inst;
        [SerializeField] private int initialMoneyGrant=5000;
        private int coinAmount;

        private void Awake()
        {
            inst = this;
            if (ES3.KeyExists("CoinAmount"))
            {
                coinAmount=ES3.Load<int>("CoinAmount");
            }
            else
            {
                coinAmount = initialMoneyGrant;
                ES3.Save("CoinAmount",initialMoneyGrant);
            }
            coinText.SetText(coinAmount.ToString());
            EventManager.StartListening("CoinChange", OnCoinChange);
        }

        public int ReturnCoinAmount()
        {
            return coinAmount;
        }

        public void AddCoins(int amount)
        {
            coinAmount+=amount;
            ES3.Save("CoinAmount",coinAmount);
            coinText.SetText(coinAmount.ToString());
        }

        private void OnCoinChange()
        {
            var change = EventManager.GetInt("CoinChange");
            if (change<0) EventManager.EmitEvent("SpentCoins");
            coinAmount += change;
            ES3.Save("CoinAmount",coinAmount);
            coinText.SetText(coinAmount.ToString());
        }
        
    
    }
}
