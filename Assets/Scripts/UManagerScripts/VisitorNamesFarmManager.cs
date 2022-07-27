using TigerForge;
using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class VisitorNamesFarmManager : MonoBehaviour
    {
        [SerializeField] private string namingDialog =
            "Bom dia! I am Antonia and I represent the Cooperative for this area. What's the name of this lovely farm of yours?";

        [SerializeField] private GameObject namingUIHolder;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TMP_InputField inputField;
        
        public static VisitorNamesFarmManager inst;
        private void Awake()
        {
            inst = this;
        }

        public void OpenDialog()
        {
            namingUIHolder.SetActive(true);
            text.SetText(namingDialog);
        }

        public void FinishedTypingOfName()
        {
            ES3.Save("FarmName",inputField.text);
            CloseAllUTabs.inst.CloseEverything();
            EventManager.EmitEvent("VisitorNamedFarm");
        }
        
    }
}
