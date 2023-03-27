using System.Collections.Generic;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BuilderTutorialController : MonoBehaviour
{
    [SerializeField] private Camera BuilderCamera;
    [SerializeField] private GameObject VitalityBar, Building1, Building2, Building3;//, textPrompt;
    private bool startTutorial = false, dragBuilding = false;
    [SerializeField] private BuilderGridManager _gridManager;
    [SerializeField] private SpellCooldown cool1, cool2, cool3;
    [SerializeField] private GameObject fog1, fog2, fog3;
    [SerializeField] private GameObject citizenPrefab, snailPrefab;

    TMPro.TextMeshProUGUI message;
    bool first_enter = true, clicked = false, mushroomStep = false, myceliumStep = false;
    bool resourceStep = false, building1Step = false, otherBuildingStep = false, citizenStep = false;
    bool movedCitizen = false, temp_first = true;
    int init_x, init_y;
    private int vitality;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => startTutorial = true);
        EventBus.Subscribe<BuildingEndDragEvent>(_ => dragBuilding = true);
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }
    // Start is called before the first frame update
    void Start()
    {
        //message = textPrompt.GetComponent<TMPro.TextMeshProUGUI>();
        //EventBus.Publish(new DisplayHintEvent("Start Tutorial."));
        //message.text = "Start Tutorial";
        init_x = (int)GameObject.Find("TMushroom").transform.position.x - 50 - 20;
        init_y = (int)GameObject.Find("TMushroom").transform.position.y - 50 - 20;
    }

    // Update is called once per frame
    void Update()
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
            EventBus.Publish(new DisplayHintEvent("Your goal is to protect your main mushroom.[click]"));
            //message.text = "Your goal is to protect your main mushroom.[click]";
        }

        if (mushroomStep && clicked)
        {
            clicked = false;
            mushroomStep = false;
            myceliumStep = true;

            EventBus.Publish(new UpdateHintEvent("You have automatically expanding mycelium.[click]"));
            //message.text = "You have automatically expanding mycelium.[click]";
            for (int i = init_x - 5; i < init_x + 5; i++)
            {
                for (int j = init_y - 5; j < init_y + 5; j++)
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

            EventBus.Publish(new UpdateHintEvent("This is vitality bar. vitality influence the speed of construction and expansion.[click]"));
            //message.text = "This is vitality bar. vitality influence the speed of construction and expansion.[click]";
            VitalityBar.SetActive(true);
        }

        if (resourceStep && clicked)
        {
            clicked = false;
            resourceStep = false;
            building1Step = true;

            EventBus.Publish(new UpdateHintEvent("Drag and drop builing into your mycelium. This building can increase the growth rate of vitality."));
            //message.text = "Drag and drop builing into your mycelium. This building can increase the growth rate of vitality.";
            Building1.SetActive(true);
            cool1.enabled = false;
            fog1.SetActive(false);
        }

        if(building1Step && dragBuilding)
        {
            building1Step = false;
            otherBuildingStep = true;
            EventBus.Publish(new UpdateHintEvent("Construction of buildings reduces vitality. [1] Increase the growth rate of vitality. [2] Produce small mushroom citizens. [3] ......"));
            //message.text = "Construction of buildings reduces vitality. [1] Increase the growth rate of vitality. [2] Produce small mushroom citizens. [3] ......";

            StartCoroutine(vitality_change());
            StartCoroutine(timer());
            Building2.SetActive(true);
            Building3.SetActive(true);

            cool2.enabled = false;
            fog2.SetActive(false);

            cool3.enabled = false;
            fog3.SetActive(false);
        }

        if(citizenStep && otherBuildingStep)
        {
            otherBuildingStep = false;
            EventBus.Publish(new UpdateHintEvent("Left click and drag to select citizen. Right click to move them."));
            //message.text = "Left click and drag to select citizen. Right click to move them.";
            if (temp_first)
            {
                temp_first = false;
                Instantiate(citizenPrefab, new Vector3(init_x - 2.0f + 70.0f, init_y - 2.0f + 70.0f, -2.0f), Quaternion.identity);
                Instantiate(citizenPrefab, new Vector3(init_x - 2.0f + 70.0f, init_y - 1.0f + 70.0f, -2.0f), Quaternion.identity);
            }
            
        }

        if (movedCitizen)
        {
            movedCitizen = false;
            EventBus.Publish(new UpdateHintEvent("The snail is coming, control your citizens to attack it!"));
            //message.text = "The snail is coming, control your citizens to attack it!";
            if (!temp_first)
            {
                temp_first = true;
                Instantiate(snailPrefab, new Vector3(init_x - 6.0f + 50.0f, init_y + 50.0f, -2.0f), Quaternion.identity);
            }
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

    IEnumerator timer()
    {
        yield return new WaitForSeconds(13.0f);
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
}
