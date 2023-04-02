using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBuilderTutorialController : MonoBehaviour
{
    [SerializeField] private Camera BuilderCamera;
    [SerializeField] private GameObject vitalityBar, Mushroom, miniMap, snail, VitalityBar;
    [SerializeField] private GridManager _gridManager;

    [SerializeField] private GameObject building1, fog1, building2, fog2, building3, fog3;
    [SerializeField] private SpellCooldown cool1, cool2, cool3;

    private int STEP_NUM = 0;
    // stwp 0 -> mushroom and snail 
    // 2 -> mycelium and mucus
    // 3 -> vitality
    // 3.5 -> voew drag and minimap
    // 4 -> building
    // 5 -> max building
    // 6 -> enemy

    bool maxzoom = false, clicked = false;
    bool firstcall = false;
    private int vitality;

    private void Awake()
    {
        EventBus.Subscribe<ZoomMaxEvent>(_ => maxzoom = true);
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }

    private void Start()
    {
        EventBus.Publish(new StartBuilderTutorialEvent());
        Mushroom.transform.position = new Vector3(25.0f, 15.0f, -2.0f);
        BuilderCamera.transform.position = new Vector3(25.0f, 15.0f, -10.0f);
    }

    private void Update()
    {
        if (STEP_NUM == 0)
        {
            EventBus.Publish(new OpenZoomEvent());
            EventBus.Publish(new DisplayHintEvent(
                "Use the mouse wheel to zoom the screen."
            ));
            if (maxzoom)
                STEP_NUM = 1;
        }

        if (STEP_NUM == 1)
        {      
            if(!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(
                "You find the snail.[Lclick]"));
                firstcall = true;
            }
            if (clicked)
            {
                clicked = false;
                STEP_NUM = 2;
                firstcall = false;
            }  
        }

        if (STEP_NUM == 2)
        {
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(
                "Mushrooms have periodically expanding mycelium.\nSnail movement leaves mucus.[Lclick]"));
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
                clicked = false;
                STEP_NUM = 3;
                firstcall = false;
            }
        }

        if (STEP_NUM == 3)
        {
            snail.transform.position = new Vector3(10.0f, 25.0f, 0.0f);
            EventBus.Publish(new OpenDragEvent());
            miniMap.SetActive(true);
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(
                "You can click on the mini-map or move the mouse to the edge of the screen to move the view. " +
                "Try to find the snail!"));
                firstcall = true;
            }

            if (BuilderCamera.transform.position.x < 17.0f && BuilderCamera.transform.position.y > 17.0f)
            {
                STEP_NUM = 4;
                firstcall = false;
            }
        }

        if (STEP_NUM == 4)
        {
            VitalityBar.SetActive(true);
            StartCoroutine(vitality_change());
            if (!firstcall)
            {
                EventBus.Publish(new UpdateHintEvent(
                "At the top of the screen is the vitality bar. Vitality controls how fast you can expand and construct.[Lclick]"));
                firstcall = true;
            }

            if (clicked)
            {
                clicked = false;
                STEP_NUM = 5;
                firstcall = false;
            }
        }

        if (STEP_NUM == 5)
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

            EventBus.Publish(new StartBuilderTutorialEvent());
            if (!firstcall)
            {
                BuilderCamera.transform.position = new Vector3(10.0f, 27.5f, -10.0f);
                EventBus.Publish(new UpdateHintEvent(
                "Drag and Drop buildings on your mycelium."));
                firstcall = true;
            }

            EventBus.Publish(new CloseZoomEvent());
            EventBus.Publish(new CloseDragEvent());
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
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

    public GroundTileManager Position2GroundManager(int x, int y)
    {
        return Tile2GroundManager(Position2Tile(x, y));
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
}