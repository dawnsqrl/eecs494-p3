using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class GroundTileManager : MonoBehaviour
{
    [SerializeField] private bool growthed = false;

    private bool blank_tile = false;
    private int blank_rate = 10;
    //private int blank_regrowth_rate = 10;
    // Start is called before the first frame update
    void Start()
    {
        set_blank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>().setAim(GetComponent<Tile>().GetSelfCoordinate(0, 0));
        //print("aim");
    }

    private void set_blank()
    {
        if (UnityEngine.Random.Range(0, 101) < blank_rate)
        {
            blank_tile = true;
        }
    }

    public void RemoveFog()
    {
        if (!blank_tile)
        {
            GameObject fog = transform.parent.gameObject.transform.Find("Tile_fog").gameObject;
            if (fog != null)
            {
                fog.SetActive(false);
            }
        }     
    }

    public void SetGrowthed()
    {
        if (!blank_tile)
            growthed = true;
        
        // Initiate a prefab representing player's Hyphae.
        var playerHyphae =
            PrefabUtility.InstantiatePrefab(
                    Resources.Load<GameObject>("Prefabs/Ground/Hyphae/Tile_Player")
                    ) as GameObject;
        playerHyphae.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        playerHyphae.transform.rotation = Quaternion.identity;
        playerHyphae.name = $"Player Tile {playerHyphae.transform.position}";
        playerHyphae.GetComponent<Tile>().Init(UnityEngine.Random.Range(0, 2) == 1);
    }

    public bool CheckGrowthed()
    {
        return growthed;
    }
}
