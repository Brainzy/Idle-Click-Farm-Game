using TigerForge;
using UnityEngine;

public class RoadSideAdditionButton : MonoBehaviour
{
    public void AdditionButtonClicked()
    {
        EventManager.SetData("AdditionButton",name);
        EventManager.EmitEvent("AdditionButton");
    }
}
