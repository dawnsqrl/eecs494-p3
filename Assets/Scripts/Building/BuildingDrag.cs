using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject gamePrefab;
    [SerializeField] private Texture2D buildingTexture;
    [SerializeField] private GameObject RTScontroller, SelectedArea;
    [SerializeField] private bool isGrowthSource;

    private Transform parentAfterDrag;
    private GameObject buildingController;
    private GrowthDemo growthDemo;
    private bool startTutorial = false;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => startTutorial = true);
        buildingController = GameObject.Find("BuildingCanvas");
        growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
    }

    private void Start()
    {
        // buildingTexture.Reinitialize(100, 100);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // temp_building = Instantiate(gameMapPrefab, new Vector3(100.0f, 100.0f, -2.0f), Quaternion.identity);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RTScontroller.SetActive(false);
        SelectedArea.SetActive(false);
        parentAfterDrag = transform.parent;
        Cursor.SetCursor(buildingTexture, Vector2.zero, CursorMode.Auto);
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
<<<<<<< HEAD
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //temp_building.transform.localScale = new Vector2(0.3f, 0.3f);
        temp_building.transform.position = new Vector3(Worldpos.x, Worldpos.y, -2.0f);
=======
        //     Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //     // temp_building.transform.localScale = new Vector2(0.3f, 0.3f);
        //     // temp_building.transform.position = new Vector3(Worldpos.x, Worldpos.y, -2.0f);
>>>>>>> 14e6a2210e6742b0a8fca9b612130827105f803c
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // temp_building.transform.position = new Vector3(100.0f, 100.0f, -2.0f);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if ((Worldpos is { x: >= 0 and <= 50, y: >= 0 and <= 50 } || startTutorial)
            && buildingController.GetComponent<BuildingController>().check_avai(Worldpos))
        {
            Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y + 0.5f));
            // GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
            //if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
            //{
            //Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
            //    new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            GameObject new_building = Instantiate(gamePrefab, new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            //growthDemo.Position2GroundManager(pos).SetGrowthed();

            buildingController.GetComponent<BuildingController>().register_building(pos, new_building);
            if (isGrowthSource)
            {
                growthDemo.Position2GroundManager(pos).SetGrowthed();
                growthDemo.AddToEdge(pos);
            }
            //}

            EventBus.Publish(new BuildingEndDragEvent());
        }

        holder.GetComponent<SpellCooldown>().reStart();
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector2(1, 1);
        RTScontroller.SetActive(true);
    }
}