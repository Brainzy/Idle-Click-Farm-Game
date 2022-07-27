using System.Collections.Generic;
using MEC;
using TigerForge;
using UnityEngine;

namespace WalkerScripts
{
    public class RiverboatSelfReturn : MonoBehaviour
    {
        [SerializeField] private float waitTimeInHarbor = 5f;

        private void Start()
        {
            EventManager.StartListening("RiverBoat",OnRiverBoat);
        }

        private void OnRiverBoat()
        {
            Timing.RunCoroutine(WaitASecondForVillagerPurchaseEvent());
        }
        
        private IEnumerator<float> WaitASecondForVillagerPurchaseEvent()
        {
            yield return Timing.WaitForSeconds(waitTimeInHarbor);
            EventManager.EmitEvent("BoatDepart");
        }
        
        
    }
}
