using System.Text;
using BuildingScripts;
using GameManagerScripts;
using PathologicalGames;
using ScriptableObjectMakingScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ButtonScripts
{
    public class UImageBuilding : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI myTextName;
        [SerializeField] private TextMeshProUGUI myTextAvailable;
        [SerializeField] private TextMeshProUGUI myTextCost;
        [SerializeField] private TextMeshProUGUI piecesOnThisLevel;
        public AmountOfBuilding amountOfBuilding;
        [SerializeField] private BuildingAttributes myBuildingAttributes;
        private bool amDragged;
        private const string availableString = "Available at level ";
        private bool canAffordMe;
        private bool placedLessThanMaxBuildings;
        private  StringBuilder stringBuilder = new StringBuilder();
        private bool loggedInBefore;

        private void Start()
        {
            myTextName.SetText(myBuildingAttributes.displayName);
            myTextAvailable.SetText(stringBuilder.Append(availableString).Append(myBuildingAttributes.piecesByLevel[0].x));
            stringBuilder.Clear();
            myTextCost.SetText(myBuildingAttributes.cost.ToString());
            SetAvailableOnThisLevelText();
            CheckIfCanAffordBuilding();
        }

        private void OnEnable()
        {
            if (loggedInBefore == false) // first time singletons won't be initialized so its done in start method
            {
                loggedInBefore = true;
                return;
            }
            SetAvailableOnThisLevelText();
            CheckIfCanAffordBuilding();
        }

        private void SetAvailableOnThisLevelText()
        {
            var max = Utility.ReturnCurrentLevelValue(myBuildingAttributes.piecesByLevel);
            stringBuilder.Append(amountOfBuilding.amountPlacedInScene).Append("/").Append(max);
            piecesOnThisLevel.SetText(stringBuilder);
            stringBuilder.Clear();
            if (amountOfBuilding.amountPlacedInScene >= max)
            {
                placedLessThanMaxBuildings = false;
            }
            else
            {
                placedLessThanMaxBuildings = true;
            }
        }

        private void ChangeBuildingColor()
        {
            if (canAffordMe && placedLessThanMaxBuildings)
            {
                image.color = new Color32(250, 250, 250, 250);
            }

            else
            {
                image.color = new Color32(25, 25, 25, 250);
            }
        }
        
        private void CheckIfCanAffordBuilding()
        {
            if (myBuildingAttributes.cost > CoinManager.inst.ReturnCoinAmount())
            {
                canAffordMe = false;
            }
            else
            {
                canAffordMe = true;
            }

            ChangeBuildingColor();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (canAffordMe == false || placedLessThanMaxBuildings==false) return;
            DragImageOfBuilding.inst.transToDrag = transform;
            DragImageOfBuilding.inst.scriptToDrag = this;
            DragImageOfBuilding.inst.StartDrag();
        }
        
        public void SuccesfullDragoutImage()
        {
            var spawnedBuilding = PoolManager.Pools["Buildings"].Spawn(myBuildingAttributes.myPrefab);
//            print(spawnedBuilding.name + " je spawnovan");
            spawnedBuilding.rotation = myBuildingAttributes.myPrefab.rotation;
            spawnedBuilding.GetComponent<Building>().dragBuilding = true;
            spawnedBuilding.position = GameManager.inst.cam.ScreenToWorldPoint(Input.mousePosition);
           // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); //debugging spawning cube
           // cube.transform.position = GameManager.inst.cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
