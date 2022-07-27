using SpawnManagers;
using TigerForge;
using TutorialScripts;
using UnityEngine;

public class Visitor1End : MonoBehaviour, TutorialManager.ITutorial
{
    [SerializeField] private SpawnBezierWalker visitor1EndDialogue;
    [SerializeField] private SpawnBezierWalker villager1BuysRandomFromPlayer;
    [SerializeField] private SpawnBezierWalker spawnRiverboat;
    [SerializeField] private GameObject endDialogueHolder;
    public static Visitor1End inst;
    private bool startedSpawner;
    
    public void RunTutorial()
    {
        if (startedSpawner) return;
        startedSpawner = true;
        visitor1EndDialogue.StartSpawner();
        TutorialManager.inst.TutorialComplete();
    }

    public void DisableSelf()
    {
        enabled = false;
    }

    private void Awake()
    {
        inst = this;
    }

    public void OpenEndDialogue()
    {
        endDialogueHolder.SetActive(true);
        EventManager.EmitEvent("VisitorTalkedEnd");
        EventManager.StartListening("MouseDown",OnMouseDown);
    }

    private void OnMouseDown()
    {
        EventManager.EmitEvent("VisitorNamedFarm");
        villager1BuysRandomFromPlayer.StartSpawner();
        spawnRiverboat.StartSpawner();
        EventManager.StopListening("MouseDown",OnMouseDown);
        endDialogueHolder.SetActive(false);
    }
}
