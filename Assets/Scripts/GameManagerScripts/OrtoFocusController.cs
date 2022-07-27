using DG.Tweening;
using UnityEngine;

namespace GameManagerScripts
{
    public class OrtoFocusController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Camera _camera2;
        [SerializeField] private Vector3 correctionFactor= new Vector3(-34f,0,25.5f);

        [SerializeField] private float defaultCamHeight;

        public static OrtoFocusController inst;
    
        private void Start()
        {
            inst = this;
            defaultCamHeight = _camera.transform.position.y;
        }

        public void FocusCameraOnTarget(GameObject target)
        {
            if (target != null)
            {
              
                CamManager.inst.DisableCamDrag();
                if (_camera.TryGetFocusTransforms(defaultCamHeight, correctionFactor, target, out var targetPosition))
                {
                    _camera.transform.DOMove(targetPosition, 0.5f);
                    _camera2.transform.DOMove(targetPosition, 0.5f);
                }
            }
        }
   
    }
}