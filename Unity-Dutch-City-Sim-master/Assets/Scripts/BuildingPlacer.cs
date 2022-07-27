using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
	private bool currentlyPlacing;
	private BuildingPreset curBuildingPreset;

	private float placementIndicatorUpdateRate = 0.05f;
	private float lastUpdateTime;
	private Vector3 curPlacementPos;
	private Vector3 startPosition;
	private Vector3 endPosition;

	public GameObject placementIndicator;
	public static BuildingPlacer inst;

	private bool firstClick = true;

	void Awake()
	{
		inst = this;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			CancelBuildingPlacement();

		if (Time.time - lastUpdateTime > placementIndicatorUpdateRate && currentlyPlacing)
		{
			lastUpdateTime = Time.time;

			curPlacementPos = Selector.inst.GetCurTilePosition();
			placementIndicator.transform.position = curPlacementPos;
		}

		if (currentlyPlacing && Input.GetMouseButtonDown(0))
		{
			if (!curBuildingPreset.allowMultiple)
			{
				PlaceBuilding();
			}
			else
			{
				//CancelBuildingPlacement();
				if (firstClick)
				{
					// Record starting position
					startPosition = curPlacementPos;
					firstClick = false;
				}
				else
				{
					endPosition = curPlacementPos;
					//PlacementManager.GetPath();

					//CancelBuildingPlacement(startPosition, endPosition);
					//PlaceBuilding();
				}

				// Record end position
				//Debug.Log(curPlacementPos);
			}

		}
	}

	public void BeginNewBuildingPlacement(BuildingPreset buildingPreset)
	{
		if (City.inst.money < buildingPreset.cost)
			return;

		currentlyPlacing = true;
		curBuildingPreset = buildingPreset;



		placementIndicator.SetActive(true);
	}

	public void CancelBuildingPlacement()
	{
		currentlyPlacing = false;
		placementIndicator.SetActive(false);
	}

	void PlaceBuilding()
	{
		GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curPlacementPos, Quaternion.identity);
		City.inst.OnPlaceBuilding(curBuildingPreset);

		CancelBuildingPlacement();
	}
}