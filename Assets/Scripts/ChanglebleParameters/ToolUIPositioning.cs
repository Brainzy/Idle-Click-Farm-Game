using UnityEngine;

namespace ChanglebleParameters
{
    public class ToolUIPositioning : MonoBehaviour
    {    
        public static ToolUIPositioning inst;
        public Vector3[] toolPositions;
   
        private void Awake()
        {
            inst = this;
        }
    
    }
}
