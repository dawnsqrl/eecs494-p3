using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            prevClearedFogList = new List<GameObject>(clearedFogList);
            clearedFogList.Clear();
            hit.transform.parent.gameObject.transform.Find("Tile_fog").gameObject.GetComponent<FogTileManager>().SetVisible(false);
            clearedFogList.Add(hit.transform.parent.gameObject);
            string[] digits = Regex.Split(clearedFogList[0].gameObject.name, @"\D+");
            int x = 0, y = 0;
            int.TryParse(digits[1], out x);
            int.TryParse(digits[2], out y);
            for (int i = 1; i < 2; i++)
            {
                GameObject groundGrid = GameObject.Find($"Tile {x+i} {y}");
                if (groundGrid)
                {
                    GameObject fog = groundGrid.transform.Find("Tile_fog").gameObject;
                    fog.GetComponent<FogTileManager>().SetVisible(false);
                    clearedFogList.Add(groundGrid);
                }
                groundGrid = GameObject.Find($"Tile {x-i} {y}");
                if (groundGrid)
                {
                    GameObject fog = groundGrid.transform.Find("Tile_fog").gameObject;
                    fog.GetComponent<FogTileManager>().SetVisible(false);
                    clearedFogList.Add(groundGrid);
                }
                groundGrid = GameObject.Find($"Tile {x} {y+i}");
                if (groundGrid)
                {
                    GameObject fog = groundGrid.transform.Find("Tile_fog").gameObject;
                    fog.GetComponent<FogTileManager>().SetVisible(false);
                    clearedFogList.Add(groundGrid);
                }
                groundGrid = GameObject.Find($"Tile {x} {y-i}");
                if (groundGrid)
                {
                    GameObject fog = groundGrid.transform.Find("Tile_fog").gameObject;
                    fog.GetComponent<FogTileManager>().SetVisible(false);
                    clearedFogList.Add(groundGrid);
                }
            }
            
            
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