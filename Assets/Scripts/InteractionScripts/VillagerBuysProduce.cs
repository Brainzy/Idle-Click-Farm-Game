using BuildingScripts;
using TigerForge;
using UManagerScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class VillagerBuysProduce : MonoBehaviour, BuildingManager.IClickOnBuilding
    {
        [SerializeField] private string myText;
        [SerializeField] private int myIndex;
        [SerializeField] private int myBuyingAmount;
        public void ClickedOnBuilding()
        {
            EventManager.EmitEvent("VillagerClicked");
            print(myText+"  "+myIndex+" "+ myBuyingAmount);
            VillagerBuysManager.inst.OpenDialog(myText, myIndex, myBuyingAmount);
        }
    }
}
