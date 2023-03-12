using System;
using System.Collections;
using System.Collections.Generic;
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
        GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>().setAim(GetComponent<Tile>().GetSelfCoordinate(0, 0));
        print("aim");
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
    }

    public bool CheckGrowthed()
    {
        return growthed;
    }
}
