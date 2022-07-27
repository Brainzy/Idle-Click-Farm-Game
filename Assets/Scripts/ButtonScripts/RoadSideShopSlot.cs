using TigerForge;
using UnityEngine;

namespace ButtonScripts
{
    public class RoadSideShopSlot : MonoBehaviour
    {
        public void OnClickedSlotButton()
        {
            EventManager.SetData("ClickedSlotButton",name);
            EventManager.EmitEvent("ClickedSlotButton");
        }
    }
}
