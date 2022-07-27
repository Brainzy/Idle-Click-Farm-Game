using TigerForge;
using UnityEngine;

public class DefaultButtonScript : MonoBehaviour
{
    public void DefaultButtonClicked()
    {
        EventManager.EmitEvent(name);
    }
}
