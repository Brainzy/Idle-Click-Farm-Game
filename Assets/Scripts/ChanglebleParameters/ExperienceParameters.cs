using UnityEngine;

namespace ChanglebleParameters
{
   public class ExperienceParameters : MonoBehaviour
   {
      public static ExperienceParameters inst;
      public int[] experiencePointForLevel;
   
      private void Awake()
      {
         inst = this;
      }
   }
}
