using UnityEngine;
using UnityEngine.UI;

namespace ButtonScripts
{
    public class ScrollWithSliderTest: MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        private Vector3 startPosition= new Vector3(69.39f,57.9f,0);
        private Vector3 endPosition =new Vector3(416.7f,57.9f,0);

        private float lenghtOfBar;
        void Start()
        {
            scrollRect.onValueChanged.AddListener(ListenerMethod);
            lenghtOfBar = endPosition.x - startPosition.x;
        }
 
        public void ListenerMethod(Vector2 value)
        {
            var clampedValue= Mathf.Clamp(value.x, 0f, 1f);
            var distanceFromStart = clampedValue * lenghtOfBar;
            transform.GetComponent<RectTransform>().anchoredPosition= new Vector3(startPosition.x+distanceFromStart, startPosition.y,0);
        }
        
     
    }
}