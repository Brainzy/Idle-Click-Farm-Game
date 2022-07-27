using System;
using System.Collections.Generic;
using System.Linq;
using BuildingScripts;
using GameManagerScripts;
using ScriptableObjectMakingScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

public static class Utility
{
    public static List<Vector3Int> MakeAListBetweenPositions(Vector3Int start, Vector3Int end)
    {
        var temporaryPlacementPositions = new List<Vector3Int>();
        var xDifference = start.x - end.x;
        var xAbsolute = Mathf.Abs(xDifference);
        var zDifference = start.z - end.z;
        var zAbsolute = Math.Abs(zDifference);
        var xFactor = 1;
        if (xDifference < 0) xFactor = -1;
        var zFactor = 1;
        if (zDifference < 0) zFactor = -1;

        for (int i = 0; i < xAbsolute + 1; i++)
        {
            for (int j = 0; j < zAbsolute + 1; j++)
            {
                temporaryPlacementPositions.Add(new Vector3Int(start.x - i * xFactor, 0, start.z - j * zFactor));
            }
        }

        return temporaryPlacementPositions;
    }

    public static bool IsListClearOfBuildings(List<Vector3Int> list, CellType[] specialPlots)
    {
        for (int i = 0; i < list.Count; i++)
        {
//            MonoBehaviour.print("usao u print"+specialPlots.Length+BuildingManager.inst.placementGrid[list[i].x, list[i].z]);
            if (BuildingManager.inst.placementGrid[list[i].x, list[i].z] == CellType.Building &&
                specialPlots.Length == 0)
            {
                //MonoBehaviour.print("zauzeto mesto ");
                return false;
            }

            if (specialPlots.Length > 0
            ) //!specialPlots.Contains(BuildingManager.inst.placementGrid[list[i].x, list[i].z])
            {
                if (!BuildingManager.inst.specialDictionary.ContainsKey(list[i])) return false;
                // MonoBehaviour.print(BuildingManager.inst.specialDictionary[list[i]]+" vs "+specialPlots[0]);
                if (!specialPlots.Contains(BuildingManager.inst.specialDictionary[list[i]])) return false;
            }
        }

        return true;
    }

    public static int FindMyIndex(BuildingAttributes building, string name)
    {
        for (int i = 0; i < building.myGrowablePrefabs.Count; i++)
        {
            if (name.Contains(building.myGrowablePrefabs[i].name))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool IsMouseOverUIWithIgnores()
    {
        var pointerEventData = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
        var raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            // print(raycastResultsList[i].gameObject.name);
            if (raycastResultsList[i].gameObject.CompareTag("IgnoreWhileDragging"))
            {
                raycastResultsList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultsList.Count > 0;
    }

    public static bool TryGetBounds(this GameObject obj, out Bounds bounds)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        return renderers.TryGetBounds(out bounds);
    }
    public static bool TryGetBounds(this Renderer[] renderers, out Bounds bounds)
    {
        bounds = default;

        if (renderers.Length == 0)
        {
            return false;
        }

        bounds = renderers[0].bounds;

        for (var i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return true;
    }

    // Facto how far away the camera should be 
    private const float cameraDistance = 8f;

    public static bool TryGetFocusTransforms(this Camera camera, float defaultDistance, Vector3 correction, GameObject targetGameObject,
        out Vector3 targetPosition)
    {
        targetPosition = default;

        if (!targetGameObject.TryGetBounds(out var bounds))
        {
            return false;
        }

        var objectSizes = bounds.max - bounds.min;
        targetPosition = bounds.center - defaultDistance * camera.transform.forward;
        targetPosition= new Vector3(targetPosition.x,defaultDistance,targetPosition.z);
        targetPosition += correction;
        return true;
    }

    public static int ReturnCurrentLevelValue(Vector2[] vects)
    {
        var currentLevel = ExperienceManager.inst.ReturnCurrentLevel();

        for (int i = vects.Length - 1; i >= 0; i--)
        {
            if (currentLevel >= vects[i].x)
            {
                return (int) vects[i].y;
            }
        }

        return 0;
    }

    public static int ReturnPreviousLevelValue(Vector2[] vects)
    {
        var currentLevel = ExperienceManager.inst.ReturnCurrentLevel() - 1;

        for (int i = vects.Length - 1; i >= 0; i--)
        {
            if (currentLevel >= vects[i].x)
            {
                return (int) vects[i].y;
            }
        }

        return 0;
    }
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rnd = new Random();
        for(var i=list.Count; i > 0; i--)
            list.Swap(0, rnd.Next(0, i));
    }

    private static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}