using BuildingScripts;
using ScriptableObjectMakingScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ButtonScripts
{
    public class ToolImageDrag : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private NewTool myInteractivity;
        public void OnPointerDown(PointerEventData eventData)
        {
            DragToolAndStartInteraction.inst.transToDrag = transform;
            DragToolAndStartInteraction.inst.toolData = myInteractivity;
            DragToolAndStartInteraction.inst.StartDrag();
        }
    }
}
