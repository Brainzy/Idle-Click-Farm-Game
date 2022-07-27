using UnityEngine;

namespace ChanglebleParameters
{
    public class StorageParameters : MonoBehaviour
    {
        public static StorageParameters inst;
        public Vector2[] barnStoragePerLevels;
        public Vector2[] siloStoragePerLevels;
        public Vector2[] compostStoragePerLevels;
   
        private void Awake()
        {
            inst = this;
        }
    }
}

