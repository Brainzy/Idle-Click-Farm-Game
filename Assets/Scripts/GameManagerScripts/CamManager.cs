using BitBenderGames;
using UnityEngine;

namespace GameManagerScripts
{
    public class CamManager : MonoBehaviour
    {
        [SerializeField] private MobileTouchCamera touchCamera;
        [SerializeField] private MobileTouchCamera touchCamera2;

        public bool draggingTool {
            get { return _draggingTool; } 
            set
            {
                touchCamera.enabled = !value;
                touchCamera2.enabled =! value;
                _draggingTool = value;
            } 
        }

        private bool _draggingTool;
        
        public static CamManager inst;

        private void Awake()
        {
            inst = this;
        }

        private void Update()
        {
            var pos = touchCamera.transform.position;
            if (pos.x > 800 || pos.x<-800) 
            {
                FixCameraBug();
            }

            if (pos.z > 800 || pos.z < -800)
            {
                FixCameraBug();
            }
            
        }

        private void FixCameraBug()
        {
            touchCamera.enabled = false;
            touchCamera2.enabled = false;
            touchCamera.transform.position = touchCamera2.transform.position;
            touchCamera.transform.rotation = touchCamera2.transform.rotation;
            touchCamera.enabled = true;
            touchCamera2.enabled = true;
            print("radi li fix buga ");
        }

        public void DisableCamDrag()
        {
            //Debug.Log("disablovana drag ");
            touchCamera.enabled = false;
            touchCamera2.enabled = false;
        }

        public void EnableCamDrag()
        {
           // Debug.Log("enablovan drag ");
            touchCamera.enabled = true;
            touchCamera2.enabled = true;
        }
        
        
    }
}
