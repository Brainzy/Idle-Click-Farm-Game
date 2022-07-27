using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UManagerScripts
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private GameObject[] normalTabs;
        [SerializeField] private GameObject[] pressedTabs;
        [SerializeField] private Button[] normalButtons;
        [SerializeField] private GameObject[] scrollViews;
        [SerializeField] private string[] headersByTab;

        private void Start()
        {
            for (int i = 0; i < normalButtons.Length; i++)
            {
                var i1 = i;
                normalButtons[i].onClick.AddListener(delegate{OpenTabButton(i1);});
            }
        }

        public void OpenTabButton(int i)
        {
            EventManager.EmitEvent("UIClick");
            for (int j = 0; j < normalTabs.Length; j++)
            {
                if (j != i)
                {
                    normalTabs[j].SetActive(true);
                }
                else
                {
                    normalTabs[j].SetActive(false);
                }
            }
            for (int j = 0; j < pressedTabs.Length; j++)
            {
                if (j == i)
                {
                    pressedTabs[j].SetActive(true);
                    scrollViews[j].SetActive(true);
                    headerText.SetText(headersByTab[j]);
                }
                else
                {
                    pressedTabs[j].SetActive(false);
                    scrollViews[j].SetActive(false);
                }
            }
        }
        
    }
}
