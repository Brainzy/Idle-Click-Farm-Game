using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class NoRoomTextActivation : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryFullUIHolder;
        [SerializeField] private TextMeshProUGUI inventoryFullText;

        public static NoRoomTextActivation inst;
        private void Awake()
        {
            inst = this;
        }

        public void ActivateText(string text)
        {
            inventoryFullUIHolder.SetActive(true);
            inventoryFullText.SetText(text);
        }
        
    }
}
