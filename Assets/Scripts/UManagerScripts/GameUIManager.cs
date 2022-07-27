using System.Collections.Generic;
using DG.Tweening;
using GameManagerScripts;
using TigerForge;
using UnityEngine;
using UnityEngine.UI;

namespace UManagerScripts
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Button shopButton, settingsButton, socialButton;
        [SerializeField] private GameObject shopCanvas;
    
        private Tween shopTween;
        private List<GameObject> allMenuList= new List<GameObject>();

        private void Start()
        {
            shopButton.onClick.AddListener(OpenShopButton);
            allMenuList.Add(shopCanvas);
        }

        public void OpenShopButton()
        {
            EventManager.EmitEvent("UIClick");
            if (shopCanvas.activeInHierarchy)
            {
                CamManager.inst.EnableCamDrag();
                shopTween.Kill();
                shopTween = shopCanvas.transform.DOLocalMove(ButtonParameters.inst.shopCanvasOffScreenPosition,
                    ButtonParameters.inst.shopCanvasAnimationDuration).OnComplete(DeactivateShop);
            }
            else
            {
                CamManager.inst.DisableCamDrag();
                shopTween.Kill();
                shopCanvas.SetActive(true);
                shopTween = shopCanvas.transform.DOLocalMove(ButtonParameters.inst.shopCanvasActivePosition,
                    ButtonParameters.inst.shopCanvasAnimationDuration);
            }
        }

        private void DeactivateShop()
        {
            EventManager.EmitEvent("UIClick");
            shopCanvas.SetActive(false);
        }
    
    }
}