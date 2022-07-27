using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MEC;
using UnityEngine;

public class SlideControllor : MonoBehaviour
{
   [SerializeField] private ProCamera2DTransitionsFX transitionsFx;
   [SerializeField] private SlideSceneParameters slideSceneParameters;


   private void Start()
   {
      Timing.RunCoroutine(RunSlideShow());
   }

   private IEnumerator<float> RunSlideShow()
   {
      for (int i = 0; i < slideSceneParameters.slideObjects.Length; i++)
      {
         yield return Timing.WaitForSeconds(slideSceneParameters.durationOfSlides[i]);
         transitionsFx.TransitionEnter();
         ActivateObject(slideSceneParameters.slideObjects[i]);
      }
   }

   private void ActivateObject(GameObject obj)
   {
      for (int i = 0; i < slideSceneParameters.slideObjects.Length; i++)
      {
         var objectFromList = slideSceneParameters.slideObjects[i];
         if (obj.Equals(objectFromList))
         {
            objectFromList.SetActive(true);
         }
         else
         {
            objectFromList.SetActive(false);
         }
      }

      
      
      
      
      
      
      
      
   }
}
