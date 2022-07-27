using DG.Tweening;
using UnityEngine;

namespace ButtonScripts
{
    public class DefaultButtonAnimation : MonoBehaviour
    {
        public float animationDuration = 2f;
        public Vector3 movementVector = new Vector3(2,2,0);
        public int vibrato;
        public float randomness;
        public bool snapping;
        public bool fadeout = true;
        public float scaleAmount=1.3f;
        public float scaleUpDuration=1f;
       // public float scalingAnimationDuration;
        public float scaleDownDuration=1f;
        public Ease scaleEase;
        public void ClickedAnimation()
        {
            print("kliknuto dugme ");
            //transform.DOShakePosition(animationDuration, strength: movementVector, vibrato: vibrato, randomness: randomness, snapping: snapping, fadeout);
            //transform.DOShakeScale(scalingAnimationDuration, strength: movementVector, vibrato: vibrato, randomness: randomness,fadeout);
            transform.DOScale(scaleAmount, scaleUpDuration).SetEase(scaleEase).OnComplete(ScaleBack);
        }

        private void ScaleBack()
        {
           transform.DOScale(1, scaleDownDuration).SetEase(scaleEase);
        }
    }
}
