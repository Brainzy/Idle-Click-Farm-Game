using UnityEngine;

public class ButtonParameters : MonoBehaviour
{
   public static ButtonParameters inst;
   public Vector3 shopCanvasOffScreenPosition;
   public Vector3 shopCanvasActivePosition;
   public float shopCanvasAnimationDuration;
   
   private void Awake()
   {
      inst = this;
   }
}
