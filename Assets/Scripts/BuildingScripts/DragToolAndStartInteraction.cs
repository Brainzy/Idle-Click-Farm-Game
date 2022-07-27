using GameManagerScripts;
using ScriptableObjectMakingScripts;
using TigerForge;
using UnityEngine;

namespace BuildingScripts
{
    public class DragToolAndStartInteraction : MonoBehaviour
    {
        public static DragToolAndStartInteraction inst;
    
        public Transform transToDrag;
        public bool hasDragObject;
        private Transform oldParent;
        private Vector3 oldPosition;
        [SerializeField] private Transform canvas;
        public NewTool toolData;
        
        private Transform lastTransform;
        private BuildingManager.ITakeActionFromTool interactibleObject;
        [SerializeField] private LayerMask interactiveMask;
        
        
        private void Awake()
        {
            inst = this;
        }
    
        private void Update()
        {
            if (hasDragObject == false) return;

            if (Input.GetMouseButtonUp(0))
            {
                EventManager.EmitEvent("ReleasedTool");
                DragStop();
                return;
            }
            transToDrag.position = Input.mousePosition;
            Ray ray = GameManager.inst.cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 200,interactiveMask))
            {
               // print(hit.transform.name);
                for (int i = 0; i < toolData.tagsItInteractsWith.Length; i++)
                {
                    if (hit.transform.CompareTag(toolData.tagsItInteractsWith[i].ToString()))
                    {
                        if (lastTransform != hit.transform)
                        {
                            interactibleObject=hit.transform.GetComponent<BuildingManager.ITakeActionFromTool>();
                            interactibleObject.RunActionFromTool(toolData);
                        }
                        interactibleObject.RunActionFromTool(toolData);
                    }
                }
               
            }
        }
    
        public void StartDrag()
        {
            EventManager.SetData("DraggingTool",toolData.toolName.ToString());
            EventManager.EmitEvent("DraggingTool");
            hasDragObject = true;
            oldParent = transToDrag.parent;
            oldPosition = transToDrag.position;
            transToDrag.SetParent(canvas);
            transToDrag.tag = "IgnoreWhileDragging";
        }

        private void DragStop()
        {
            if (hasDragObject == false) return;
            transToDrag.SetParent(oldParent);
            transToDrag.position = oldPosition;
            transToDrag.tag = "Normal";
            hasDragObject = false;
        }
    
    
    
    }
}
