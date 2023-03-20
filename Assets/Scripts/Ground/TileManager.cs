using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private GameObject fog_builder;
    private GameObject fog_enemy;
    //private int blank_regrowth_rate = 10;
    // Start is called before the first frame update
    void Start()
    {
        fog_builder = transform.Find("Tile_fog_builder").gameObject;
        fog_enemy = transform.Find("Tile_fog_enemy").gameObject;
    }

    public void SetFogVisible(bool visible, string fog_type)
    {
        GameObject fog = fog_type == "builder" ? fog_builder : fog_enemy;
        if (fog)
        {
            fog.SetActive(visible);
        }
    }
}
