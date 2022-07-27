using GameManagerScripts;
using TigerForge;
using UnityEngine;

namespace UManagerScripts
{
    public class IAPManager : MonoBehaviour
    {
        [SerializeField] private GameObject iapCanvas;
        
        public void OpenCloseShop()
        {
            EventManager.EmitEvent("UIClick");
            if (iapCanvas.activeSelf)
            {
                CamManager.inst.EnableCamDrag();
                iapCanvas.SetActive(false);
                
            }
            else
            {
                CamManager.inst.DisableCamDrag();
                iapCanvas.SetActive(true);
            }
        }
    
    }
}
