using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthDemo : MonoBehaviour
{
    [SerializeField] private int init_x, init_y;
    [SerializeField] private int growth_speed = 1; // 0 -> 0, 5 -> 80
    [SerializeField] private int timeGap = 3;
    [SerializeField] private GameObject GridManagerGameobject, ResourceController;

    [SerializeField] private int mapSize_x, mapSize_y;
    [SerializeField] private float range_factor = 0.8f;

    private bool aim_is_set = false, aim_is_reach = false;
    private Vector2 growthAim;

    private float avg_aim_dis = 0.0f;
    private List<Vector2> growthed = new List<Vector2>();
    private bool isSimulationPaused = false;

    private void Awake()
    {
        EventBus.Subscribe<ModifyPauseEvent>(e => isSimulationPaused = e.status);
    }

    private void Start()
    {
        StartCoroutine(AutoGrowth(timeGap));
        EventBus.Publish(new AssignInitGrowthPositionEvent(new Vector2(init_x, init_y)));
    }

    public void setAim(int x, int y)
    {
        if (x <= mapSize_x && y <= mapSize_y && x >= 0 && y >= 0)
        {
            aim_is_set = true;
            growthAim = new Vector2(x, y);
        }
        else
        {
            aim_is_set = false;
        }
        //if (!aim_is_set)
        //{
        //    growthAim = aimCoord;
        //    aim_is_set = true;
        //}
    }

    IEnumerator AutoGrowth(int timeGap)
    {
        yield return new WaitForSeconds(5.0f);
        int base_rate = 16 * growth_speed;
        float new_avg_dis = 0.0f;
        float dis_to_aim = 0.0f;

        float count;

        print("startDemo");
        
        RemoveFogFromTile(init_x, init_y, 1);
        Position2GroundManager(init_x, init_y).SetGrowthed();

        while (true)
        {
            yield return new WaitForSeconds(timeGap / SimulationSpeedControl.GetSimulationSpeed());
            yield return null;

            count = 0.0f;
            for (int x = 0; x < mapSize_x; x++)
            {
                for (int y = 0; y < mapSize_y; y++)
                {
                    while (isSimulationPaused || GameProgressControl.isGameEnded)
                        yield return null;

                    if (!Position2Growthed(growthAim))
                        aim_is_reach = false;
                    else
                        aim_is_reach = true;

                    int adj_factor = count_growthed_adjacent(x, y);
                    //if (adj_factor == 1)
                    //    adj_factor = 4;
                    //if (adj_factor != 1 && adj_factor != 0)
                    //    adj_factor = 1;

                    float growth_possibility = Mathf.Log(adj_factor + 1, 5) * base_rate; // log base 5, 4 -> 1;
                    if (aim_is_set && adj_factor != 0)
                    {
                        dis_to_aim = GetDistance(Position2Tile(x, y), Position2Tile(growthAim));
                        new_avg_dis += dis_to_aim;
                        count += 1.0f;
                    }

                    if (aim_is_set && dis_to_aim < avg_aim_dis * range_factor && !aim_is_reach && adj_factor != 0)
                    {
                        growth_possibility += 30;//25 / avg_aim_dis * (avg_aim_dis - dis_to_aim) + 10;
                    }
                    if (aim_is_set && dis_to_aim > avg_aim_dis && !aim_is_reach && adj_factor != 0)
                    {
                        //growth_possibility -= 40;
                        growth_possibility = 10;
                    }

                    //growth_possibility = growth_possibility * ResourceController.GetComponent<Resource>().get_growth_amount() * 1.0f / 1000.0f;
                    //if (growth_possibility != 0)
                    //{
                    //    print("****************");
                    //    print(growth_possibility);
                    //    print(Mathf.Log(adj_factor + 1, 5));
                    //    print(base_rate);
                    //    print("****************");
                    //}


                    // Random.Range(int minInclusive, int maxExclusive);
                    if (UnityEngine.Random.Range(0, 101) < growth_possibility)
                    {
                        RemoveFogFromTile(x, y, 1);
                        growthed.Add(new Vector2(x, y));
                        //Position2GroundManager(x, y).SetGrowthed();

                        ResourceController.GetComponent<Resource>().change_growth_amount(ResourceController.GetComponent<Resource>().get_growth_amount() - 0.5f);
                        ResourceController.GetComponent<Resource>().change_resource(ResourceController.GetComponent<Resource>().get_resource() - 1.0f);
                    }
                }
            }
            avg_aim_dis = new_avg_dis / count;
            new_avg_dis = 0.0f;

            foreach (Vector2 item in growthed)
            {
                if (item == growthAim)
                    aim_is_reach = true;
                Position2GroundManager(item).SetGrowthed();
            }

        }
        
    }

    public int count_growthed_adjacent(int x, int y)
    {
        if (Position2Growthed(x, y))
            return 0;

        int res = 0;
        if (x + 1 < mapSize_x)
            res += Position2Growthed(x + 1, y) ? 1 : 0;
        if (x - 1 > 0)
            res += Position2Growthed(x - 1, y) ? 1 : 0;
        if (y + 1 < mapSize_y)
            res += Position2Growthed(x, y + 1) ? 1 : 0;
        if (y - 1 > 0)
            res += Position2Growthed(x, y - 1) ? 1 : 0;

        return res;
    }

    public float GetDistance(GameObject tile, GameObject aim)
    {
        float dis = Vector2.Distance(Tile2Position(tile), Tile2Position(aim));
        //if (dis <= 1)
            //aim_is_reach = true;

        return dis;
    }

    public void RemoveFogFromTile(GameObject tile)
    {
        Tile2GroundManager(tile).SetFogVisible(false);
    }
    
    public void RemoveFogFromTile(int x, int y, int radius)
    {
        RemoveFogFromTile(Position2Tile(x, y));
        for (int i = 1; i < radius + 1; i++)
        {
            if (FogTilePositionSanityCheck(x+i, y))
            {
                Position2GroundManager(x+i, y).SetFogVisible(false);
            }
            if (FogTilePositionSanityCheck(x-i, y))
            {
                Position2GroundManager(x - i, y).SetFogVisible(false);
            }
            if (FogTilePositionSanityCheck(x, y+i))
            {
                Position2GroundManager(x, y+i).SetFogVisible(false);
            }
            if (FogTilePositionSanityCheck(x, y-i))
            {
                Position2GroundManager(x, y -i).SetFogVisible(false);
            }
        }
    }

    private bool FogTilePositionSanityCheck(int x, int y)
    {
        if (!(x < mapSize_x && x >= 0 && y < mapSize_y && y >= 0))
        {
            return false;
        }

        if (!Position2Tile(x, y))
        {
            return false;
        }
        return true;
    }

    public GameObject Position2Tile(int x, int y)
    {
        return GridManagerGameobject.GetComponent<GridManager>().GetTileAtPosition(new Vector2(x, y));
    }
    public GameObject Position2Tile(Vector2 vec)
    {
        return GridManagerGameobject.GetComponent<GridManager>().GetTileAtPosition(vec);
    }

    public bool Position2Growthed(int x, int y)
    {
        return Tile2GroundManager(Position2Tile(x, y)).CheckGrowthed();
    }
    public bool Position2Growthed(Vector2 vec)
    {
        return Tile2GroundManager(Position2Tile(vec)).CheckGrowthed();
    }

    public GroundTileManager Position2GroundManager(int x, int y)
    {
        return Tile2GroundManager(Position2Tile(x, y));
    }
    public GroundTileManager Position2GroundManager(Vector2 vec)
    {
        return Tile2GroundManager(Position2Tile(vec));
    }

    public Vector2 Tile2Position(GameObject tile)
    {
        return tile.transform.Find("Tile_ground").gameObject.GetComponent<Tile>().GetSelfCoordinate(0, 0);
    }
    public GroundTileManager Tile2GroundManager(GameObject tile)
    {
        return tile.transform.Find("Tile_ground").gameObject.GetComponent<GroundTileManager>();
    }

}