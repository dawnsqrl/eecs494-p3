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
    [SerializeField] private int range = 1;
    private string clearFogType;
    private Vector3 prevPos;
    private GridManager gridManager; 
    private void Start()
    {
        clearedFogList = new List<GameObject>();
        prevClearedFogList = new List<GameObject>();
        clearFogType = isBuilder ? "builder" : "enemy";
        prevPos = transform.position;
        gridManager = GameObject.FindGameObjectWithTag("GroundTiles").GetComponent<GridManager>();

        if (GetComponent<GroundTileManager>())
        {
            ClearFog_SetLongTerm();
            isBuilder = true;
        }
        if (CompareTag("Builder"))
        {
            ClearFog_builder();
            isBuilder = true;
        }
        else
        {
            ClearFog_enemy();
            isBuilder = false;
        }

        clearFogType = isBuilder ? "builder" : "enemy";
    }

    private void Update()
    {
        if (Math.Truncate(transform.position.x) != Math.Truncate(prevPos.x) || Math.Truncate(transform.position.y) != Math.Truncate(prevPos.y))
        {
            if (CompareTag("Enemy") || CompareTag("BaseCar"))
            {
                ClearFog_enemy();
            }

            else if (CompareTag("Builder"))
            {
                ClearFog_builder();
            }
        }
    }
    
    private void ClearFog_enemy()
    {
        Ray2D ray = new Ray2D(transform.position, Vector2.positiveInfinity);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            prevClearedFogList = new List<GameObject>(clearedFogList);
            clearedFogList.Clear();
            // hit.transform.parent.gameObject.transform.Find("Tile_fog_" + clearFogType).gameObject.GetComponent<FogTileManager>().SetVisible(false);
            hit.transform.gameObject.GetComponent<GroundTileManager>().SetFogVisible(false, clearFogType);
            clearedFogList.Add(hit.transform.parent.gameObject);
            string[] digits = Regex.Split(clearedFogList[0].gameObject.name, @"\D+");
            int x = 0, y = 0;
            int.TryParse(digits[1], out x);
            int.TryParse(digits[2], out y);

            for (int i = x-range; i < x+1+range; i++)
            {
                for (int j = y-range; j < y+1+range; j++)
                {
                    GameObject tileAtPosition = gridManager.GetTileAtPosition(new Vector2(i, j));
                    if (tileAtPosition)
                    {
                        // GameObject fog = groundGrid.transform.Find("Tile_fog_" + clearFogType).gameObject;
                        // fog.GetComponent<FogTileManager>().SetVisible(false);
                        tileAtPosition.GetComponent<TileManager>().SetFogVisible(false, clearFogType);
                        clearedFogList.Add(tileAtPosition);
                    }
                }
            }
            // the overlay part of the two lists remains cleared
            List<GameObject> RestoreFogList = prevClearedFogList.Except(clearedFogList).ToList();
            foreach (var tile in RestoreFogList)
            {
                // s.transform.Find("Tile_fog_" + clearFogType).gameObject.GetComponent<FogTileManager>().SetVisible(true);
                tile.GetComponent<TileManager>().SetFogVisible(true, clearFogType);
            }
            prevClearedFogList.Clear();
        }
    }
    
    private void ClearFog_builder()
    {
        Ray2D ray = new Ray2D(transform.position, Vector2.positiveInfinity);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            prevClearedFogList = new List<GameObject>(clearedFogList);
            clearedFogList.Clear();
            // hit.transform.parent.gameObject.transform.Find("Tile_fog_" + clearFogType).gameObject.GetComponent<FogTileManager>().SetVisible(false);
            if (!hit.transform.gameObject.GetComponent<GroundTileManager>().BuilderFogLongTermDisabled())
            {
                hit.transform.gameObject.GetComponent<GroundTileManager>().SetFogVisible(false, clearFogType);
                clearedFogList.Add(hit.transform.parent.gameObject);
            }
            string[] digits = Regex.Split(hit.transform.parent.gameObject.name, @"\D+");
            int x = 0, y = 0;
            int.TryParse(digits[1], out x);
            int.TryParse(digits[2], out y);

            for (int i = x-range; i < x+1+range; i++)
            {
                for (int j = y-range; j < y+1+range; j++)
                {
                    GameObject tileAtPosition = gridManager.GetTileAtPosition(new Vector2(i, j));
                    if (tileAtPosition && !hit.transform.parent.gameObject.GetComponent<TileManager>().BuilderFogLongTermDisabled())
                    {
                        // GameObject fog = groundGrid.transform.Find("Tile_fog_" + clearFogType).gameObject;
                        // fog.GetComponent<FogTileManager>().SetVisible(false);
                        tileAtPosition.GetComponent<TileManager>().SetFogVisible(false, clearFogType);
                        clearedFogList.Add(tileAtPosition);
                    }
                }
            }
            // the overlay part of the two lists remains cleared
            List<GameObject> RestoreFogList = prevClearedFogList.Except(clearedFogList).ToList();
            foreach (var tile in RestoreFogList)
            {
                // s.transform.Find("Tile_fog_" + clearFogType).gameObject.GetComponent<FogTileManager>().SetVisible(true);
                if (!tile.GetComponent<TileManager>().BuilderFogLongTermDisabled())
                {
                    tile.GetComponent<TileManager>().SetFogVisible(true, clearFogType);
                }
            }
            prevClearedFogList.Clear();
        }
    }
    
    private void ClearFog_SetLongTerm()
    {
        Ray2D ray = new Ray2D(transform.position, Vector2.positiveInfinity);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            // hit.transform.parent.gameObject.transform.Find("Tile_fog_" + clearFogType).gameObject.GetComponent<FogTileManager>().SetVisible(false);
            hit.transform.gameObject.GetComponent<GroundTileManager>().SetFogVisible_LongTerm(false);
            string[] digits = Regex.Split(hit.transform.parent.gameObject.name, @"\D+");
            int x = 0, y = 0;
            int.TryParse(digits[1], out x);
            int.TryParse(digits[2], out y);

            for (int i = x-range; i < x+1+range; i++)
            {
                for (int j = y-range; j < y+1+range; j++)
                {
                    GameObject tileAtPosition = gridManager.GetTileAtPosition(new Vector2(i, j));
                    if (tileAtPosition)
                    {
                        tileAtPosition.GetComponent<TileManager>().SetFogVisible_LongTerm(false);
                    }
                }
            }
        }
    }
}