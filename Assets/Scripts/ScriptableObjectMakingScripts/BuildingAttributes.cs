using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectMakingScripts
{
	[CreateAssetMenu(fileName = "Change name of building", menuName = "New Building")]
	public class BuildingAttributes : ScriptableObject
	{
		public string displayName;
		public int cost;
		[Tooltip(("If you tick this, spaceTaken will be by bellow values instead of auto"))] public bool manualPlacement;
		public int spaceTakenUp;
		public int spaceTakenDown;
		public int spaceTakenLeft;
		public int spaceTakenRight;
		public float constructingTimeSeconds; //Not implemented
		[Tooltip(("X is level, Y is pieces on this level, start from lowest, end with highest level"))] public Vector2[] piecesByLevel;
		public Transform myPrefab;
		public float producesSoundAfterSeconds;
		public bool isAnimal;
		public bool isGrowable;
		public List<Transform> myGrowablePrefabs;
		public List<float> myGrowableTimers;
		public Transform cutTree;
		public Transform treeLeaf;
		public CellType[] makesCellTypes;
		[Tooltip(("Leave empty if no special requirements for droping the building, otherwise it is a list of all allowed types of land"))] public CellType[] allowedCellTypes;
		public NewTool[] interactivitieLinks;
		public string interactionResultName;
		public int interactionResultSellPrice;
	}
}