using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBuilderTutorialController : MonoBehaviour
{
    [SerializeField] private Camera BuilderCamera;
    [SerializeField] private GameObject vitalityBar, Mushroom, snail, VitalityBar, maxBuilding, grass, cave;
    [SerializeField] private GameObject arrow, arrow2;
    [SerializeField] private GridManager _gridManager;

    [SerializeField]
    private GameObject building1, fog1, building2, fog2, building3, fog3, building4, fog4, building0, fog0, destroyBuilding, fogDestroy;

    [SerializeField] private SpellCooldown cool1, cool2, cool3, cool4, cool0, coolDestroy;
    [SerializeField] private ViewDragging vd;

    bool finishGrowth = false, readyToDetectClick = true;

    private int STEP_NUM = 0;
    // step 0 -> mushroom and snail 
    // 2 -> mycelium and mucus
    // 3 -> vitality
    // 3.5 -> view drag and minimap
    // 4 -> building
    // 5 -> max building
    // 6 -> enemy

    bool maxzoom = false, clicked = false, lockClick = false;
    bool firstcall = false, win0 = false, destroyActivate = false;
    private int vitality;
    int buildingCount = 0, temp_count = 0;

    private void Awake()
    {
        EventBus.Subscribe<TBaseCarDestroy>(_ => win0 = true);
        EventBus.Subscribe<ZoomMaxEvent>(_ => maxzoom = true);
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);

        EventBus.Subscribe<BuildingEndDragEvent>(_ => BuildingEndDrag());
        EventBus.Subscribe<BuildingDestoryEvent>(_ => destroyActivate = true);
    }

    private void BuildingEndDrag()
    {
        buildingCount += 1;
    }

    IEnumerator ClickCoolDown()
    {
        readyToDetectClick = false;
        yield return new WaitForSeconds(2.0f);
        readyToDetectClick = true;
    }

    private void Start()
    {
        //vd.enabled = false;
        EventBus.Publish(new StartBuilderTutorialEvent());
        Mushroom.transform.position = new Vector3(25.0f, 15.0f, -2.0f);
        BuilderCamera.transform.position = new Vector3(25.0f, 15.0f, -10.0f);
    }

    private void Update()
    {
        //print(GameProgressControl.isGameActive);
        if (STEP_NUM == 0)
        {
            EventBus.Publish(new OpenZoomEvent());
            EventBus.Publish(new DisplayHintEvent(0,
                "Scroll <b>mouse wheel</b> to zoom the screen."
            ));
            if (maxzoom)
            {
                EventBus.Publish(new CloseZoomEvent());
                STEP_NUM = 1;
            }
        }

        if (STEP_NUM == 1)
        {
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(0,
                    "You found the snail. <b>[Lclick]</b>"
                ));
                firstcall = true;
                EventBus.Publish(new CloseZoomEvent());
                vd.setSize(9.0f);
            }

            if (clicked)
            {
                StartCoroutine(ClickCoolDown());
                clicked = false;
                STEP_NUM = 2;
                firstcall = false;
            }
        }

        if (STEP_NUM == 2)
        {
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(0,
                    "Mushroom's mycelium expands periodically.\n" +
                    "Snail movement leaves behind a trail of mucus. <b>[Lclick]</b>"
                ));
                firstcall = true;
            }

            for (int i = 23; i < 27; i++)
            for (int j = 14; j < 17; j++)
                Position2GroundManager(i, j).SetGrowthed();

            for (int i = 27; i < 29; i++)
            for (int j = 14; j < 16; j++)
                Position2GroundManager(i, j).SetGrowthed();

            Position2GroundManager(28, 14).SetGrowthed();
            Position2GroundManager(28, 15).SetGrowthed();
            Position2GroundManager(28, 16).SetGrowthed();
            Position2GroundManager(28, 17).SetGrowthed();

            Position2GroundManager(27, 13).SetGrowthed();
            Position2GroundManager(23, 17).SetGrowthed();
            Position2GroundManager(22, 16).SetGrowthed();
            Position2GroundManager(22, 14).SetGrowthed();

            for (int j = 11; j < 20; j++)
                Position2GroundManager(32, j).SetMucus();

            for (int i = 26; i < 32; i++)
                Position2GroundManager(i, 11).SetMucus();

            if (clicked)
            {
                StartCoroutine(ClickCoolDown());
                clicked = false;
                STEP_NUM = 3;
                firstcall = false;
            }
        }

        if (STEP_NUM == 3)
        {
            lockClick = true;
            snail.transform.position = new Vector3(10.0f, 25.0f, 0.0f);
            EventBus.Publish(new OpenDragEvent());
            //miniMap.SetActive(true);
            if (!firstcall)
            {
                //vd.enabled = true;
                EventBus.Publish(new UpdateHintEvent(0,
                    "You can <b>move mouse</b> to the edge of the screen to move the view. " +
                    "Try to find the snail!", 575
                ));
                firstcall = true;
            }

            if (BuilderCamera.transform.position is { x: < 17.0f, y: > 15.0f })
            {
                STEP_NUM = 4;
                firstcall = false;
                //vd.enabled = false;
                //miniMap.SetActive(false);
            }
        }

        if (STEP_NUM == 4)
        {
            EventBus.Publish(new CloseDragEvent());
            //miniMap.SetActive(false);
            VitalityBar.SetActive(true);
            if (!firstcall)
            {
                StartCoroutine(vitality_change());
                EventBus.Publish(new UpdateHintEvent(0,
                    "At the top of the screen is the <b>vitality bar</b>. " +
                    "Vitality controls how fast the mycelium can grow. <b>[Lclick]</b>"
                ));
                firstcall = true;
                lockClick = false;
            }

            if (clicked)
            {
                StartCoroutine(ClickCoolDown());
                clicked = false;
                STEP_NUM = 5;
                firstcall = false;
            }
        }

        if (STEP_NUM == 5)
        {
            EventBus.Publish(new StartBuilderTutorialEvent());
            if (!firstcall)
            {
                building1.SetActive(true);
                cool1.enabled = false;
                fog1.SetActive(false);
                building2.SetActive(true);
                cool2.enabled = false;
                fog2.SetActive(false);
                building3.SetActive(true);
                cool3.enabled = false;
                fog3.SetActive(false);
                coolDestroy.enabled = false;
                fogDestroy.SetActive(false);

                BuilderCamera.transform.position = new Vector3(10.0f, 30.0f, -10.0f);
                EventBus.Publish(new UpdateHintEvent(0,
                    "<b>Drag and drop</b> buildings onto your mycelium.\n" +
                    "Hover mouse over the building icons for more information.\n" +
                    "Build buildings will cost vitality!", 575
                ));
                firstcall = true;
                Position2GroundManager(5, 31).SetGrowthed();
                Position2GroundManager(5, 30).SetGrowthed();
                Position2GroundManager(6, 31).SetGrowthed();
                Position2GroundManager(6, 30).SetGrowthed();

                Position2GroundManager(10, 31).SetGrowthed();
                Position2GroundManager(10, 30).SetGrowthed();
                Position2GroundManager(11, 31).SetGrowthed();
                Position2GroundManager(11, 30).SetGrowthed();

                Position2GroundManager(15, 31).SetGrowthed();
                Position2GroundManager(15, 30).SetGrowthed();
                Position2GroundManager(16, 31).SetGrowthed();
                Position2GroundManager(16, 30).SetGrowthed();
            }

            if (temp_count < 2)
            {
                BuilderCamera.transform.position = new Vector3(10.0f, 30.0f, -10.0f);
                temp_count += 1;
            }


            EventBus.Publish(new CloseDragEvent());
            EventBus.Publish(new StartBuilderTutorialEvent());
            if (buildingCount == 3)
            {
                STEP_NUM = 6;
                firstcall = false;
                EventBus.Publish(new StartBuilderTutorialEvent());
            }
        }

        if (STEP_NUM == 6)
        {
            EventBus.Publish(new StartBuilderTutorialEvent());
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(0,
                    "<b>[LDrag]</b> to select mushroom soldiers. " +
                    "<b>[RClick]</b> to lead them to a point.\n" +
                    "Guide your soldiers to attack the snail!", 575
                ));
                firstcall = true;
                Instantiate(Resources.Load<GameObject>("Prefabs/BuilderTutorial/TCitizen"),
                    new Vector3(10.0f, 28.0f), Quaternion.identity);
                Instantiate(Resources.Load<GameObject>("Prefabs/BuilderTutorial/TCitizen"),
                    new Vector3(11.0f, 28.0f), Quaternion.identity);
            }

            if (win0)
            {
                STEP_NUM = 65;
                firstcall = false;
                EventBus.Publish(new StartBuilderTutorialEvent());
            }
        }

        if (STEP_NUM == 65)
        {
            destroyBuilding.SetActive(true);
            EventBus.Publish(new StartBuilderTutorialEvent());
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(0,
                    "<b>Drag and drop</b> destroy icon onto your buildings.\n" +
                    "Retrieve <b>half</b> of vitality cost.", 575
                ));
                firstcall = true;
            }

            if (destroyActivate)
            {
                STEP_NUM = 7;
                firstcall = false;
                EventBus.Publish(new StartBuilderTutorialEvent());
            }
        }

        if (STEP_NUM == 7)
        {
            EventBus.Publish(new StartBuilderTutorialEvent());
            if (!firstcall)
            {
                building0.SetActive(true);
                cool0.enabled = false;
                fog0.SetActive(false);

                building1.SetActive(false);
                building2.SetActive(false);
                building3.SetActive(false);
                destroyBuilding.SetActive(false);

                EventBus.Publish(new StartBuilderTutorialEvent());

                EventBus.Publish(new UpdateHintEvent(0,
                    "Unlike all other buildings, the <b>growth source</b> should be placed off the mycelium, " +
                    "which will become a new origin of mycelium spread. Drag and drop a new source somewhere.", 575
                ));
                firstcall = true;
            }

            if (buildingCount == 4)
            {
                STEP_NUM = 8;
                firstcall = false;
                EventBus.Publish(new StartBuilderTutorialEvent());
            }
        }

        if (STEP_NUM == 8)
        {
            EventBus.Publish(new StartBuilderTutorialEvent());
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(0,
                    "This new source of mycelium takes some time to mature.", 575
                ));
                firstcall = true;
                StartCoroutine(waitForGrowth());
            }

            if (finishGrowth)
            {
                STEP_NUM = 9;
                firstcall = false;
                EventBus.Publish(new StartBuilderTutorialEvent());
            }
        }

        if (STEP_NUM == 9)
        {
            if (!firstcall)
            {
                building4.SetActive(true);
                building1.SetActive(false);
                cool4.enabled = false;
                fog4.SetActive(false);

                arrow2.SetActive(true);

                EventBus.Publish(new StartBuilderTutorialEvent());

                BuilderCamera.transform.position = new Vector3(30.0f, 27.5f, -10.0f);

                EventBus.Publish(new UpdateHintEvent(0,
                    "If mucus surrounds your mycelium, you can spend some vitality to destroy them. " +
                    "Try to decay the mucus in the middle of your view."
                ));
                firstcall = true;

                Position2GroundManager(30, 27).SetMucus();
                Position2GroundManager(30, 26).SetMucus();
                Position2GroundManager(31, 27).SetMucus();
                Position2GroundManager(31, 26).SetMucus();
                Position2GroundManager(32, 26).SetGrowthed();
            }

            if (!(Position2GroundManager(30, 27).CheckMucused()
                  && Position2GroundManager(30, 26).CheckMucused()
                  && Position2GroundManager(31, 27).CheckMucused()
                  && Position2GroundManager(31, 26).CheckMucused()))
            {
                clicked = false;
                STEP_NUM = 10;
                firstcall = false;
            }
        }

        if (STEP_NUM == 10)
        {
            if (!firstcall)
            {
                arrow2.SetActive(false);
                maxBuilding.SetActive(true);
                arrow.SetActive(true);

                building0.SetActive(true);
                building1.SetActive(true);
                building2.SetActive(true);
                building3.SetActive(true);
                building4.SetActive(true);
                destroyBuilding.SetActive(true);
                fog0.SetActive(true);
                fog1.SetActive(true);
                fog2.SetActive(true);
                fog3.SetActive(true);
                fog4.SetActive(true);
                fogDestroy.SetActive(true);

                //miniMap.SetActive(true);

                EventBus.Publish(new StartBuilderTutorialEvent());
                EventBus.Publish(new UpdateHintEvent(0,
                    "You have an <b>upper limit</b> of how many buildings you can place. " +
                    "You can increase the limit by adding more spread source. <b>[Lclick]</b>"
                ));
                firstcall = true;
            }

            if (clicked)
            {
                StartCoroutine(ClickCoolDown());
                clicked = false;
                STEP_NUM = 11;
                firstcall = false;
            }
        }

        if (STEP_NUM == 11)
        {
            if (!firstcall)
            {
                maxBuilding.SetActive(true);
                arrow.SetActive(false);

                grass.SetActive(true);
                cave.SetActive(true);

                EventBus.Publish(new StartBuilderTutorialEvent());
                EventBus.Publish(new UpdateHintEvent(0,
                    "The snail may hide in the grass, and small snails will emerge from the cave to follow the big snail. " +
                    "You can destroy both by growing mycelium over them. <b>[Lclick]</b>"
                ));
                firstcall = true;
            }

            if (clicked)
            {
                StartCoroutine(ClickCoolDown());
                clicked = false;
                STEP_NUM = 12;
                firstcall = false;
            }
        }

        if (STEP_NUM == 12)
        {
            if (!firstcall)
            {
                EventBus.Publish(new StartBuilderTutorialEvent());
                EventBus.Publish(new UpdateHintEvent(0,
                    "You completed the tutorial! Please wait for the snail to complete."
                ));
                firstcall = true;
            }

            EventBus.Publish(new EndBuilderTutorialEvent());
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !lockClick && readyToDetectClick)
        {
            clicked = true;
        }
    }

    IEnumerator vitality_change()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            if (vitality < 1000)
                EventBus.Publish(new ModifyVitalityEvent(vitality + 50));
        }
    }

    IEnumerator waitForGrowth()
    {
        yield return new WaitForSeconds(8.0f);
        finishGrowth = true;
    }

    public GroundTileManager Position2GroundManager(int x, int y)
    {
        return Tile2GroundManager(Position2Tile(x, y));
    }

    public GroundTileManager Position2GroundManager(Vector2 vec)
    {
        return Tile2GroundManager(Position2Tile(vec));
    }

    public GroundTileManager Tile2GroundManager(GameObject tile)
    {
        return _gridManager.GetTileGroundAtPosition(tile).gameObject.GetComponent<GroundTileManager>();
    }

    public GameObject Position2Tile(int x, int y)
    {
        return _gridManager.GetTileAtPosition(new Vector2(x, y));
    }

    public bool Position2Growthed(Vector2 vec)
    {
        return Tile2GroundManager(Position2Tile(vec)).CheckGrowthed();
    }

    public GameObject Position2Tile(Vector2 vec)
    {
        return _gridManager.GetTileAtPosition(vec);
    }

    public bool Position2Mucused(Vector2 vec)
    {
        return Tile2GroundManager(Position2Tile(vec)).CheckMucused();
    }
}