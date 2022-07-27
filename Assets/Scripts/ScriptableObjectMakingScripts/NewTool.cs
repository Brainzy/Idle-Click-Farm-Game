using BuildingScripts;
using UnityEngine;

namespace ScriptableObjectMakingScripts
{
    [CreateAssetMenu(fileName = "Change name of tool", menuName = "New Tool")]
    public class NewTool : ScriptableObject
    {
        public BuildingManager.ToolName toolName;
        public Transform userinterfacePrefab;
        public BuildingManager.BuildingTag[] tagsItInteractsWith;
        public int[] gainedAmountItemsFromInteraction;
        // animation it does for each tag
        public BuildingManager.StorageLocation[] storageLocations;
        public int[] experienceGains;
    }
}