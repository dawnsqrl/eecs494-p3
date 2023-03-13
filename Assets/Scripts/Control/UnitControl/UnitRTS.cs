using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;
using GameObject = UnityEngine.GameObject;

public class UnitRTS : MonoBehaviour
{
    private GameObject selectedGameObject;
    private IMovePosition movePosition;
    private List<GameObject> clearedFogList;
    private List<GameObject> prevClearedFogList;
    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<IMovePosition>();
        clearedFogList = new List<GameObject>();
        prevClearedFogList = new List<GameObject>();
        SetSelectedActive(false);
    }

    public void SetSelectedActive(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        movePosition.SetMovePosition(targetPosition);
    }
    
    private void Update()
    {
        Ray2D ray = new Ray2D(transform.position, Vector2.positiveInfinity);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask.GetMask("Fog"));
        if (hit)
        {
            prevClearedFogList = new List<GameObject>(clearedFogList);
            hit.transform.gameObject.GetComponent<FogTileManager>().SetVisible(false);
            clearedFogList.Clear();
            clearedFogList.Add(hit.transform.parent.gameObject);
            
            // the overlay part of the two lists remains cleared
            List<GameObject> RestoreFogList = prevClearedFogList.Except(clearedFogList).ToList();
            int a = 0;
            foreach (var s in RestoreFogList)
            {
                s.transform.Find("Tile_fog").gameObject.GetComponent<FogTileManager>().SetVisible(true);
            }
            prevClearedFogList.Clear();
        }
    }
}