using BuildingScripts;
using TigerForge;
using UnityEngine;

namespace GameManagerScripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask interactiveMask;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.EmitEvent("MouseDown");
                if (!Utility.IsMouseOverUIWithIgnores())
                {
                    RaycastHit hit;
                    Ray ray = GameManager.inst.cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 200,interactiveMask))
                    {
                        CamManager.inst.DisableCamDrag();
                        hit.collider.GetComponent<BuildingManager.IClickOnBuilding>().ClickedOnBuilding();
                    }
                    else
                    {
                        CamManager.inst.EnableCamDrag();
                        CloseAllUTabs.inst.CloseEverything();
                    }
                }
            }
        }
    }
}
