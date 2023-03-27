using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderTutorialController : MonoBehaviour
{
    [SerializeField] private Camera BuilderCamera;
    [SerializeField] private GameObject VitalityBar, Building1, Building2, Building3; //, textPrompt;
    private bool startTutorial = false, dragBuilding = false;
    [SerializeField] private BuilderGridManager _gridManager;
    [SerializeField] private SpellCooldown cool1, cool2, cool3;
    [SerializeField] private GameObject fog1, fog2, fog3;
    [SerializeField] private GameObject citizenPrefab, snailPrefab;

    //TMPro.TextMeshProUGUI message;
    bool first_enter = true, clicked = false, mushroomStep = false, myceliumStep = false;
    bool resourceStep = false, building1Step = false, otherBuildingStep = false, citizenStep = false;
    bool movedCitizen = false, temp_first = true, endTutorial = false;
    int init_x, init_y;
    private int vitality;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => startTutorial = true);
        EventBus.Subscribe<BuildingEndDragEvent>(_ => dragBuilding = true);
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
        EventBus.Subscribe<BuilderTutorialSnailDeadEvent>(_ => endTutorial = true);
        EventBus.Subscribe<EndAllTutorialEvent>(_ => EventBus.Publish(new DismissHintEvent()));
    }

    private void Start()
    {
        //message = textPrompt.GetComponent<TMPro.TextMeshProUGUI>();
        //EventBus.Publish(new DisplayHintEvent("Start Tutorial."));
        //message.text = "Start Tutorial";
        init_x = (int)GameObject.Find("TMushroom").transform.position.x - 50 - 20;
        init_y = (int)GameObject.Find("TMushroom").transform.position.y - 50 - 20;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && startTutorial)
        {
            clicked = true;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame && citizenStep)
        {
            movedCitizen = true;
            citizenStep = false;
        }

        if (startTutorial && first_enter)
        {
            mushroomStep = true;

            first_enter = false;
            // Camera transfer
            BuilderCamera.transform.position = new Vector3(85f, 85f, -10.0f);
            // remove useless component
            VitalityBar.SetActive(false);
            Building1.SetActive(false);
            Building2.SetActive(false);
            Building3.SetActive(false);
            // Mushroom
            EventBus.Publish(new DisplayHintEvent(
                "Protect your home mushroom from the snail! [Lclick]"
            ));
        }

        if (mushroomStep && clicked)
        {
            clicked = false;
            mushroomStep = false;
            myceliumStep = true;

            EventBus.Publish(new UpdateHintEvent(
                "Mycelium expands periodically with time. [Lclick]"
            ));
            int radius = 3;
            for (int i = init_x - radius; i < init_x + radius; i++)
            {
                for (int j = init_y - radius; j < init_y + radius; j++)
                {
                    Position2GroundManager(i, j).SetGrowthed();
                }
            }
        }

        if (myceliumStep && clicked)
        {
            clicked = false;
            resourceStep = true;
            myceliumStep = false;

            EventBus.Publish(new UpdateHintEvent(
                "This is vitality bar. Vitality controls how fast you can expand and construct. [Lclick]"
            ));
            VitalityBar.SetActive(true);
        }

        if (resourceStep && clicked)
        {
            clicked = false;
            resourceStep = false;
            building1Step = true;

            EventBus.Publish(new UpdateHintEvent(
                "Drag and drop buildings onto mycelium. Construction costs vitality."
            ));
            Building1.SetActive(true);
            cool1.enabled = false;
            fog1.SetActive(false);
        }

        if (building1Step && dragBuilding)
        {
            building1Step = false;
            otherBuildingStep = true;
            EventBus.Publish(new UpdateHintEvent(
                "Hover over the buildings to see their function and stats."
            ));

            StartCoroutine(vitality_change());
            StartCoroutine(timer(5));
            Building2.SetActive(true);
            Building3.SetActive(true);

            cool2.enabled = false;
            fog2.SetActive(false);

            cool3.enabled = false;
            fog3.SetActive(false);
        }

        if (citizenStep && otherBuildingStep)
        {
            otherBuildingStep = false;
            EventBus.Publish(new UpdateHintEvent(
                "[LDrag] to select citizens. [RClick] lead them to a point."
            ));
            if (temp_first)
            {
                temp_first = false;
                Instantiate(citizenPrefab, new Vector3(init_x - 2.0f + 70.0f, init_y - 2.0f + 70.0f, -2.0f),
                    Quaternion.identity);
                Instantiate(citizenPrefab, new Vector3(init_x - 2.0f + 70.0f, init_y - 1.0f + 70.0f, -2.0f),
                    Quaternion.identity);
            }
        }

        if (movedCitizen)
        {
            movedCitizen = false;
            EventBus.Publish(new UpdateHintEvent(
                "The snail is coming, guide your citizens to attack it!"
            ));
            if (!temp_first)
            {
                temp_first = true;
                Instantiate(snailPrefab, new Vector3(init_x - 4.0f + 70.0f, init_y + 70.0f, -2.0f),
                    Quaternion.identity);
            }
        }

        if (endTutorial)
        {
            EventBus.Publish(new UpdateHintEvent(
                "You completed the tutorial! Please wait for the snail to complete."
            ));
            endTutorial = false;
            Building1.SetActive(true);
            Building2.SetActive(true);
            Building3.SetActive(true);
            VitalityBar.SetActive(true);
            cool1.enabled = true;
            cool2.enabled = true;
            cool3.enabled = true;
            fog1.SetActive(true);
            fog2.SetActive(true);
            fog3.SetActive(true);
            print("++++++++++++++++++++++++++++++++++++++++++++++");
            EventBus.Publish(new EndBuilderTutorialEvent());
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

    IEnumerator timer(float duration)
    {
        yield return new WaitForSeconds(duration);
        citizenStep = true;
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

    public bool getStart()
    {
        return startTutorial;
    }
}