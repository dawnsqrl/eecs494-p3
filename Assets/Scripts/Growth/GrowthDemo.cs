using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthDemo : MonoBehaviour
{
    [SerializeField] private int growth_speed = 1; // 0 -> 0, 5 -> 80
    [SerializeField] private int timeGap = 3;

    [SerializeField] private GridManager _gridManager;

    // [SerializeField] private GameObject ResourceController;
    [SerializeField] private int mapSize_x, mapSize_y;

    // [SerializeField] private float range_factor = 0.8f;
    [SerializeField] private GrassManager grassManager;

    //private bool aim_is_set = false, aim_is_reach = false;
    //private Vector2 growthAim;

    //private float avg_aim_dis = 0.0f;
    private List<Vector2> growthed = new List<Vector2>();
    private bool isSimulationPaused = false;

    private bool first_loop = true;
    private List<Vector2> edge_list = new List<Vector2>();

    private int init_x, init_y;
    private int vitality;
    [SerializeField] private BuildingController buildingController;
    [SerializeField] private GameObject mushuroom;

    public Vector2 getInitPos()
    {
        return new Vector2(init_x, init_y);
    }

    private void Awake()
    {
        EventBus.Subscribe<ModifyPauseEvent>(e => isSimulationPaused = e.status);
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }

    private void Start()
    {
        while (true)
        {
            GameObject.Find("Mushroom").transform.position = new Vector3(Random.Range(5, 45), Random.Range(5, 45), 0);
            init_x = (int)GameObject.Find("Mushroom").transform.position.x;
            init_y = (int)GameObject.Find("Mushroom").transform.position.y + 1;

            GameObject.Find("Mushroom").transform.position = new Vector3(
                GameObject.Find("Mushroom").transform.position.x + 0.5f,
                GameObject.Find("Mushroom").transform.position.y, GameObject.Find("Mushroom").transform.position.z);

            if (!grassManager.CheckRange(new Vector2(init_x, init_y), 10))
                break;
        }

        init_x = (int)GameObject.Find("Mushroom").transform.position.x;
        init_y = (int)GameObject.Find("Mushroom").transform.position.y + 1;

        //buildingController = GameObject.Find("Canvas").transform.Find("BuildingCanvas").gameObject;
        //buildingController.register_building(new Vector2(init_x, init_y), mushuroom);
        //print(init_x);
        //print(init_y);
        StartCoroutine(AutoGrowth(timeGap));
        EventBus.Publish(new AssignInitGrowthPositionEvent(new Vector2(init_x, init_y)));
        edge_list.Add(new Vector2(init_x, init_y));
        edge_list.Add(new Vector2(init_x, init_y - 2));
        edge_list.Add(new Vector2(init_x, init_y - 1));
        edge_list.Add(new Vector2(init_x + 1, init_y));
        edge_list.Add(new Vector2(init_x + 1, init_y - 1));
        edge_list.Add(new Vector2(init_x + 1, init_y - 2));
        edge_list.Add(new Vector2(init_x - 1, init_y));
        edge_list.Add(new Vector2(init_x - 1, init_y - 1));
        edge_list.Add(new Vector2(init_x - 1, init_y - 2));
    }

    //public void setAim(int x, int y)
    //{
    //    if (x <= mapSize_x && y <= mapSize_y && x >= 0 && y >= 0)
    //    {
    //        aim_is_set = true;
    //        growthAim = new Vector2(x, y);
    //    }
    //    else
    //    {
    //        aim_is_set = false;
    //    }
    //if (!aim_is_set)
    //{
    //    growthAim = aimCoord;
    //    aim_is_set = true;
    //}
    //}

    IEnumerator AutoGrowth(int timeGap)
    {
        
        yield return new WaitForSeconds(3.0f);
        int base_rate = 16 * growth_speed;
        //float new_avg_dis = 0.0f;
        //float dis_to_aim = 0.0f;

        float count;

        // print("startDemo");
        //
        // for (int i = 0; i < 50; i++)
        //     for (int j = 0; j < 50; j++)
        //         Position2GroundManager(i, j).SetGrowthed();

        // RemoveFogFromTile(init_x, init_y, 1);
        Position2GroundManager(init_x, init_y).SetGrowthed();
        Position2GroundManager(init_x + 1, init_y).SetGrowthed();
        Position2GroundManager(init_x + 1, init_y - 1).SetGrowthed();
        Position2GroundManager(init_x, init_y - 1).SetGrowthed();
        //Position2GroundManager(init_x, init_y).SetGrowthed();
        //Position2GroundManager(init_x, init_y).SetGrowthed();

        buildingController.register_building(new Vector2(init_x, init_y), mushuroom);

        while (true)
        {
            yield return new WaitForSeconds(timeGap / SimulationSpeedControl.GetSimulationSpeed());
            yield return null;

            count = 0.0f;
            foreach (Vector2 pos in edge_list)
            {
                while (isSimulationPaused || !GameProgressControl.isGameActive)
                    yield return null;

                //if (!Position2Growthed(growthAim))
                //    aim_is_reach = false;
                //else
                //    aim_is_reach = true;

                Vector2[] temp =
                {
                    new Vector2(pos.x + 1, pos.y), new Vector2(pos.x, pos.y + 1), new Vector2(pos.x - 1, pos.y),
                    new Vector2(pos.x, pos.y - 1)
                };
                foreach (Vector2 adj_pos in temp)
                {
                    if (Mathf.FloorToInt(adj_pos.x) < 0 || Mathf.FloorToInt(adj_pos.x) >= mapSize_x ||
                        Mathf.FloorToInt(adj_pos.y) < 0 || Mathf.FloorToInt(adj_pos.y) >= mapSize_y)
                        continue;
                    if (Position2Growthed(adj_pos) || Position2Mucused(adj_pos))
                        continue;
                    if (growthed.Contains(adj_pos))
                        continue;

                    int adj_factor = count_growthed_adjacent(Mathf.FloorToInt(adj_pos.x), Mathf.FloorToInt(adj_pos.y));
                    float growth_possibility = Mathf.Log(adj_factor + 1, 5) * base_rate; // log base 5, 4 -> 1;
                    //if (aim_is_set && adj_factor != 4)
                    //{
                    //    dis_to_aim = GetDistance(Position2Tile(adj_pos), Position2Tile(growthAim));
                    //    new_avg_dis += dis_to_aim;
                    //    count += 1.0f;
                    //}
                    //if (aim_is_set && dis_to_aim < avg_aim_dis * range_factor && !aim_is_reach && adj_factor != 4)
                    //{
                    //    growth_possibility += 30;//25 / avg_aim_dis * (avg_aim_dis - dis_to_aim) + 10;
                    //}
                    //if (aim_is_set && dis_to_aim > avg_aim_dis && !aim_is_reach && adj_factor != 4)
                    //{
                    //    //growth_possibility -= 40;
                    //    growth_possibility = 10;
                    //}

                    if (first_loop)
                        growth_possibility = 80;

                    if (vitality < 300)
                        growth_possibility = 0;

                    if (vitality < 500)
                        growth_possibility = growth_possibility / 2;

                    // Random.Range(int minInclusive, int maxExclusive);
                    //Random.seed = System.DateTime.Now.Millisecond;
                    if (UnityEngine.Random.Range(0, 101) < growth_possibility)
                    {
                        // RemoveFogFromTile(x, y, 1);
                        growthed.Add(adj_pos);
                        Position2GroundManager(adj_pos).SetGrowthed();
                        //ResourceController.GetComponent<Resource>().change_growth_amount(ResourceController.GetComponent<Resource>().get_growth_amount() - 0.5f);
                        //ResourceController.GetComponent<Resource>().change_resource(ResourceController.GetComponent<Resource>().get_resource() - 1.0f);
                    }
                }
            }

            //avg_aim_dis = new_avg_dis / count;
            //new_avg_dis = 0.0f;

            foreach (Vector2 item in growthed)
            {
                //if (item == growthAim)
                //    aim_is_reach = true;
                //    Position2GroundManager(item).SetGrowthed();
                edge_list.Add(item);
            }

            growthed.Clear();

            edge_list.RemoveAll(
                item => count_growthed_adjacent(Mathf.FloorToInt(item.x), Mathf.FloorToInt(item.y)) == 4);

            if (first_loop)
                first_loop = false;
        }
    }

    public int count_growthed_adjacent(int x, int y)
    {
        //if (Position2Growthed(x, y))
        //    return 0;

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
        Tile2GroundManager(tile).SetFogVisible(false, "builder");
    }

    public void RemoveFogFromTile(int x, int y, int radius)
    {
        RemoveFogFromTile(Position2Tile(x, y));
        for (int i = 1; i < radius + 1; i++)
        {
            if (FogTilePositionSanityCheck(x + i, y))
            {
                Position2GroundManager(x + i, y).SetFogVisible(false, "builder");
            }

            if (FogTilePositionSanityCheck(x - i, y))
            {
                Position2GroundManager(x - i, y).SetFogVisible(false, "builder");
            }

            if (FogTilePositionSanityCheck(x, y + i))
            {
                Position2GroundManager(x, y + i).SetFogVisible(false, "builder");
            }

            if (FogTilePositionSanityCheck(x, y - i))
            {
                Position2GroundManager(x, y - i).SetFogVisible(false, "builder");
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
        return _gridManager.GetTileAtPosition(new Vector2(x, y));
    }

    public GameObject Position2Tile(Vector2 vec)
    {
        return _gridManager.GetTileAtPosition(vec);
    }

    public bool Position2Growthed(int x, int y)
    {
        return Tile2GroundManager(Position2Tile(x, y)).CheckGrowthed();
    }

    public bool Position2Growthed(Vector2 vec)
    {
        return Tile2GroundManager(Position2Tile(vec)).CheckGrowthed();
    }

    public bool Position2Mucused(Vector2 vec)
    {
        return Tile2GroundManager(Position2Tile(vec)).CheckMucused();
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
        return _gridManager.GetTileGroundAtPosition(tile).GetComponent<Tile>().GetSelfCoordinate(0, 0);
    }

    public GroundTileManager Tile2GroundManager(GameObject tile)
    {
        return _gridManager.GetTileGroundAtPosition(tile).gameObject.GetComponent<GroundTileManager>();
    }

    public bool FakeGrowthed(Vector2 pos)
    {
        foreach (Vector2 item in growthed)
        {
            if (item == pos)
                return true;
        }

        return false;
    }

    public void AddToEdge(Vector2 pos)
    {
        edge_list.Add(pos);
    }
}