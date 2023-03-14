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

public class ClearSurroundingFog : MonoBehaviour
{
    private List<GameObject> clearedFogList;
    private List<GameObject> prevClearedFogList;
    [SerializeField] private bool isBuilder;
    private string clearFogType;
    private void Start()
    {
        clearedFogList = new List<GameObject>();
        prevClearedFogList = new List<GameObject>();
        clearFogType = isBuilder ? "builder" : "enemy";
    }

    private void Update()
    {
        Ray2D ray = new Ray2D(transform.position, Vector2.positiveInfinity);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            prevClearedFogList = new List<GameObject>(clearedFogList);
            clearedFogList.Clear();
            hit.transform.parent.gameObject.transform.Find("Tile_fog_" + clearFogType).gameObject.GetComponent<FogTileManager>().SetVisible(false);
            clearedFogList.Add(hit.transform.parent.gameObject);
            string[] digits = Regex.Split(clearedFogList[0].gameObject.name, @"\D+");
            int x = 0, y = 0;
            int.TryParse(digits[1], out x);
            int.TryParse(digits[2], out y);

            for (int i = x-1; i < x+2; i++)
            {
                for (int j = y-1; j < y+2; j++)
                {
                    GameObject groundGrid = GameObject.Find($"Tile {i} {j}");
                    if (groundGrid)
                    {
                        GameObject fog = groundGrid.transform.Find("Tile_fog_" + clearFogType).gameObject;
                        fog.GetComponent<FogTileManager>().SetVisible(false);
                        clearedFogList.Add(groundGrid);
                    }
                }
            }
            
            // for (int i = 1; i < 2; i++)
            // {
            //     GameObject groundGrid = GameObject.Find($"Tile {x+i} {y}");
            //     if (groundGrid)
            //     {
            //         GameObject fog = groundGrid.transform.Find("Tile_fog_" + clearFogType).gameObject;
            //         fog.GetComponent<FogTileManager>().SetVisible(false);
            //         clearedFogList.Add(groundGrid);
            //     }
            //     groundGrid = GameObject.Find($"Tile {x-i} {y}");
            //     if (groundGrid)
            //     {
            //         GameObject fog = groundGrid.transform.Find("Tile_fog_" + clearFogType).gameObject;
            //         fog.GetComponent<FogTileManager>().SetVisible(false);
            //         clearedFogList.Add(groundGrid);
            //     }
            //     groundGrid = GameObject.Find($"Tile {x} {y+i}");
            //     if (groundGrid)
            //     {
            //         GameObject fog = groundGrid.transform.Find("Tile_fog_" + clearFogType).gameObject;
            //         fog.GetComponent<FogTileManager>().SetVisible(false);
            //         clearedFogList.Add(groundGrid);
            //     }
            //     groundGrid = GameObject.Find($"Tile {x} {y-i}");
            //     if (groundGrid)
            //     {
            //         GameObject fog = groundGrid.transform.Find("Tile_fog_" + clearFogType).gameObject;
            //         fog.GetComponent<FogTileManager>().SetVisible(false);
            //         clearedFogList.Add(groundGrid);
            //     }
            // }
            
            
            // the overlay part of the two lists remains cleared
            List<GameObject> RestoreFogList = prevClearedFogList.Except(clearedFogList).ToList();
            foreach (var s in RestoreFogList)
            {
                s.transform.Find("Tile_fog_" + clearFogType).gameObject.GetComponent<FogTileManager>().SetVisible(true);
            }
            prevClearedFogList.Clear();
        }
    }
}