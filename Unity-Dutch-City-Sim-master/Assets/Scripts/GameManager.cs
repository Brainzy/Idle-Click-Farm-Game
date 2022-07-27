using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source https://github.com/SunnyValleyStudio/SimpleCityBuilder
public class GameManager : MonoBehaviour
{
	public CameraMovement cameraMovement;
	public RoadManager roadManager;
	public InputManager inputManager;
	public PlacementManager placementManager;

	public UIController uiController;

	public StructureManager structureManager;

	private float lastUpdateTime;
	private void Start()
	{
		uiController.OnRoadPlacement += RoadPlacementHandler;
		uiController.OnHousePlacement += HousePlacementHandler;
		uiController.OnSpecialPlacement += SpecialPlacementHandler;

	}

	private void SpecialPlacementHandler()
	{
		ClearInputActions();
		inputManager.OnMouseClick += structureManager.PlaceSpecial;
	}

	private void HousePlacementHandler()
	{
		ClearInputActions();
		inputManager.OnMouseClick += structureManager.PlaceHouse;
	}

	private void RoadPlacementHandler()
	{
		ClearInputActions();

		inputManager.OnMouseClick += roadManager.PlaceRoad;
		inputManager.OnMouseHold += roadManager.PlaceRoad;
		inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
	}

	private void ClearInputActions()
	{
		inputManager.OnMouseClick = null;
		inputManager.OnMouseHold = null;
		inputManager.OnMouseUp = null;

		placementManager.StartPlacement();
	}

	private void Update()
	{
		cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
	}

}
