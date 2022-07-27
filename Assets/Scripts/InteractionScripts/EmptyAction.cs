using BuildingScripts;
using ScriptableObjectMakingScripts;
using UnityEngine;

namespace InteractionScripts
{
    public class EmptyAction : MonoBehaviour, BuildingManager.ITakeActionFromTool
    {
        public void RunActionFromTool(NewTool toolData)
        {
            
        }
    }
}
