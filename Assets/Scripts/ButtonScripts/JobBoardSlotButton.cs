using TigerForge;
using UnityEngine;

namespace ButtonScripts
{
    public class JobBoardSlotButton : MonoBehaviour
    {
        public void OnClickedSlotButton()
        {
            EventManager.SetData("JobBoardSlotButton",name);
//            print("kliknuto na " + name);
            EventManager.EmitEvent("JobBoardSlotButton");
        }
    }
}
