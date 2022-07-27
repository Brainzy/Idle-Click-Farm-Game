using BuildingScripts;
using UManagerScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class FarmHouseAchievments : MonoBehaviour, BuildingManager.IClickOnBuilding
    {
       
        public void ClickedOnBuilding()
        {
           AchievmentManager.inst.OpenAchievments();
        }
    }
}
