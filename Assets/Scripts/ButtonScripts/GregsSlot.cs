using TigerForge;
using UnityEngine;

namespace ButtonScripts
{
    public class GregsSlot : MonoBehaviour
    {
        public void OnClickedSlotButton()
        {
            EventManager.SetData("GregSlot", name);
            EventManager.EmitEvent("GregSlot");
        }
    }
}