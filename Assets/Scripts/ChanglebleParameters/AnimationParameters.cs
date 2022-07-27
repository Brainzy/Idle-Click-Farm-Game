using UnityEngine;

namespace ChanglebleParameters
{
    public class AnimationParameters : MonoBehaviour
    {    
        public static AnimationParameters inst;
        public float storingFlightSpeedTowardsBar = 1f;
        public float storingFallSpeedToGround = 1f;
   
        private void Awake()
        {
            inst = this;
        }
    
    }
}
