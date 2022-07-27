using System.Collections.Generic;
using ButtonScripts;
using GameManagerScripts;
using TigerForge;
using UManagerScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuildingScripts
{
    public class DragImageOfBuilding : MonoBehaviour
    {
        public static DragImageOfBuilding inst;
        [SerializeField] private Transform canvas;
        [SerializeField] private GameUIManager gameUiManager;
        public Transform transToDrag;
        public UImageBuilding scriptToDrag;
        public bool hasDragObject;
        private Transform oldParent;
        private Vector3 oldPosition;

        private void Awake()
        {
            inst = this;
        }

        public void StartDrag()
        {
            hasDragObject = true;
            CamManager.inst.draggingTool = true;
            EventManager.EmitEvent("StartedDraggingImage");
            oldParent = transToDrag.parent;
            oldPosition = transToDrag.position;
            transToDrag.SetParent(canvas);
            transToDrag.tag = "IgnoreWhileDragging";
        }

        private void DragStop()
        {
            if (hasDragObject == false) return;
            CamManager.inst.draggingTool = true;
            transToDrag.SetParent(oldParent);
            transToDrag.position = oldPosition;
            transToDrag.tag = "Normal";
            hasDragObject = false;
            EventManager.EmitEvent("StoppedDraggingImage");
        }

        private void SuccesfullDragoutImage()
        {
            CloseAllUTabs.inst.CloseEverything();
            scriptToDrag.SuccesfullDragoutImage();
        }

        private void Update()
        {
            if (hasDragObject == false) return;
            if (Utility.IsMouseOverUIWithIgnores() == false)
            {
                DragStop();
                SuccesfullDragoutImage();
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                DragStop();
                return;
            }
            transToDrag.position = Input.mousePosition;
        }

       
    }
}