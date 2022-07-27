using GameManagerScripts;
using TigerForge;
using TMPro;
using UnityEngine;

namespace TutorialScripts
{
    public class TutorialSharedResource : MonoBehaviour
    {
        public static TutorialSharedResource inst;
        
        [SerializeField] private GameObject visitorTalkUI;
        public TextMeshProUGUI text;

        public void ActivateDialog()
        {
            CloseAllUTabs.inst.CloseEverything();
            visitorTalkUI.SetActive(true);
            EventManager.EmitEvent("ActivatedDialog");
        }

        public void DeactivateDialog(GameObject target)
        {
            OrtoFocusController.inst.FocusCameraOnTarget(target);
            CloseAllUTabs.inst.CloseEverything();
            visitorTalkUI.SetActive(false); 
        }

        private void Awake()
        {
            inst = this;
        }
    }
}
