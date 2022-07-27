using TigerForge;
using UnityEngine;
using UnityEngine.UI;

namespace UManagerScripts
{
    public class FriendBookManager : MonoBehaviour
    {    
        [SerializeField] private GameObject[] normalTabs;
        [SerializeField] private GameObject[] pressedTabs;
        [SerializeField] private GameObject[] textHolders;
        [SerializeField] private Button[] normalButtons;
      

        [SerializeField] private GameObject friendbookCanvas;
        
        public void OpenCloseFriendbook()
        {
            EventManager.EmitEvent("UIClick");
            if (friendbookCanvas.activeSelf)
            {
                friendbookCanvas.SetActive(false);
            }
            else
            {
                friendbookCanvas.SetActive(true);
            }
        }    
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
                }
                else
                {
                    pressedTabs[j].SetActive(false);
                }
            }
            for (int j = 0; j < textHolders.Length; j++)
            {
                if (j == i)
                {
                    textHolders[j].SetActive(true);
                }
                else
                {
                    textHolders[j].SetActive(false);
                }
            }
            
        }
    
    }
}
