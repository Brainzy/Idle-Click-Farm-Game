using BuildingScripts;
using UManagerScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class OpenJobBoard : MonoBehaviour, BuildingManager.IClickOnBuilding
    {
        public void ClickedOnBuilding()
        {
            JobBoardManager.inst.OpenCanvas();
        }
    }
}
