using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundTileManager : MonoBehaviour
{
    [SerializeField] private bool growthed = false;

    private bool blank_tile = false;
    private int blank_rate = 2;

    private GameObject fog_builder;
    private GameObject fog_enemy;
    //private int blank_regrowth_rate = 10;
    // Start is called before the first frame update
    void Start()
    {
        set_blank();
        fog_builder = transform.parent.gameObject.transform.Find("Tile_fog_builder").gameObject;
        fog_enemy = transform.parent.gameObject.transform.Find("Tile_fog_enemy").gameObject;
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

    public void SetFogVisible(bool visible, string fog_type)
    {
        if (!blank_tile)
        {
            GameObject fog = fog_type == "builder" ? fog_builder : fog_enemy;
            if (fog)
            {
                fog.SetActive(visible);
            }
        }     
    }
    
    public void SetFogVisible_LongTerm(bool visible)
    {
        if (!blank_tile)
        {
            if (fog_builder)
            {
                fog_builder.SetActive(visible);
                transform.parent.GetComponent<TileManager>().builderFogLongTermDisabled = true;
            }
        }     
    }

    public void SetGrowthed()
    {
        // if (!growthed)
        // {
            // if (!blank_tile)
            // {
            //     // Initiate a prefab representing player's Hyphae.
            //     GameObject playerHyphae = Instantiate(Resources.Load<GameObject>("Prefabs/Ground/Hyphae/Tile_Player"), new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), Quaternion.identity);
            //     playerHyphae.name = $"Player Tile {playerHyphae.transform.position}";
            //     playerHyphae.GetComponent<Tile>().Init(UnityEngine.Random.Range(0, 2) == 1);
            // }
        // }
        gameObject.GetComponent<ClearSurroundingFog>().enabled = true;
        gameObject.GetComponent<Tile>().SetBaseColor(new Color32(0x10, 0xAB, 0x00, 0xFF));
        gameObject.GetComponent<Tile>().SetOffsetColor(new Color32(0x5C, 0xCA, 0x59, 0xFF));
        gameObject.GetComponent<Tile>().SetColor(UnityEngine.Random.Range(0, 2) == 1);
        growthed = true;
    }

    public bool CheckGrowthed()
    {
        return growthed;
    }
    public bool BuilderFogLongTermDisabled()
    {
        return transform.parent.GetComponent<TileManager>().builderFogLongTermDisabled;
    }
}
