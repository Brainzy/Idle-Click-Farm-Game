using UnityEngine;

public class CloseAllUTabs : MonoBehaviour
{
    [SerializeField] private GameObject[] uTabs;

    public static CloseAllUTabs inst;

    private void Awake()
    {
        inst = this;
    }

    public void CloseEverything()
    {
        for (int i = 0; i < uTabs.Length; i++)
        {
            uTabs[i].SetActive(false);
        }
    }
}
