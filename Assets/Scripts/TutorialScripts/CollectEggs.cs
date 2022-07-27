using TigerForge;
using TutorialScripts;
using UnityEngine;

public class CollectEggs : MonoBehaviour, TutorialManager.ITutorial
{
    public bool shownDialog;
    [SerializeField] private string dialog =
        "Collect 3 eggs from chickens";
    [SerializeField] private GameObject chickenCoop;
    [SerializeField] private int amountOfEggsToCollect = 3;

    private int collectedEggs;
    
    public void DisableSelf()
    {
        enabled = false;
    }
    
    public void RunTutorial()
    {
        if (shownDialog) return;
        TutorialSharedResource.inst.ActivateDialog();
        TutorialSharedResource.inst.text.SetText(dialog);
        shownDialog = true;
        EventManager.StartListening("MouseDown",OnMouseDown);
    }
    
    private void OnMouseDown()
    {
        TutorialSharedResource.inst.DeactivateDialog(chickenCoop);
        EventManager.StopListening("MouseDown", OnMouseDown);
        EventManager.StartListening("Chicken",OnChicken);
    }

    private void OnChicken()
    {
        var amount = EventManager.GetInt("Chicken");
        collectedEggs += amount;
        if (collectedEggs >= amountOfEggsToCollect)
        {
            CloseAllUTabs.inst.CloseEverything();
            TutorialManager.inst.TutorialComplete();
            print("gotov tutorial ");
            EventManager.StopListening("Chicken",OnChicken);
        }
    }
}
