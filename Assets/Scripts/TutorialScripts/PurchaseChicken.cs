using TigerForge;
using TutorialScripts;
using UnityEngine;

public class PurchaseChicken : MonoBehaviour,TutorialManager.ITutorial
{
    public bool shownDialog;
    [SerializeField] private string dialog =
        "Open asset menu and purchase 3 Chicken, by dragging them into Chicken Coop";
    [SerializeField] private GameObject chickenCoop;
    [SerializeField] private int amountOfChickensToBuyForThisTutorial = 3;

    private int boughtChickens;
    
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
        EventManager.StartListening("BuildingPlaced",OnBuildingPlaced);
    }

    private void OnBuildingPlaced()
    {
        var buildingName = EventManager.GetString("BuildingPlaced");
        if (buildingName != "Chicken") return;
        boughtChickens++;
        if (boughtChickens >= amountOfChickensToBuyForThisTutorial)
        {
            CloseAllUTabs.inst.CloseEverything();
            TutorialManager.inst.TutorialComplete();
            print("gotov tutorial ");
        }
    }

   
}
