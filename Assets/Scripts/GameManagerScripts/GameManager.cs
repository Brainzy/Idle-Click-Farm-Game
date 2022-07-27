using UnityEngine;

namespace GameManagerScripts
{
    public class GameManager : MonoBehaviour
    {
       
        public Camera cam;
        public static GameManager inst;
        [SerializeField] private Plane plane = new Plane(Vector3.up, Vector3.zero);

        private void Awake()
        {
            inst = this;
        #if (UNITY_ANDROID)
         Application.targetFrameRate = 60;
            #endif
        }

        public Vector3 GetCurTilePosition()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float rayOut = 0.0f;
            if (plane.Raycast(cam.ScreenPointToRay(Input.mousePosition), out rayOut))
            {
                Vector3 newPos = ray.GetPoint(rayOut) - new Vector3(0.5f, 0.0f, 0.5f);
                return new Vector3(Mathf.CeilToInt(newPos.x), 0, Mathf.CeilToInt(newPos.z));
            }

            return new Vector3(0, -99, 0);
        }
    }
}