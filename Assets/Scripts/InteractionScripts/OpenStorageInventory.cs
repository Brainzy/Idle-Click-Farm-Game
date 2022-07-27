using BuildingScripts;
using GameManagerScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class OpenStorageInventory : MonoBehaviour,BuildingManager.IClickOnBuilding
    {
        [SerializeField] private BuildingManager.StorageLocation myStorageLocation;
        public void ClickedOnBuilding()
        {
           StorageManager.inst.OpenInventory(myStorageLocation);
        }
    }
}
