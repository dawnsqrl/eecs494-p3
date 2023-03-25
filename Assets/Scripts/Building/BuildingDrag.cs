using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform parentAfterDrag;
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject gameMapPrefab, gamePrefab;
    [SerializeField] private GameObject RTScontroller, SelectedArea;

    private GameObject buildingController, temp_building;
    private bool startTutorial = false;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => startTutorial = true);
    }

    private void Start()
    {
        buildingController = GameObject.Find("BuildingCanvas");
        temp_building = Instantiate(gameMapPrefab, new Vector3(100.0f, 100.0f, -2.0f), Quaternion.identity);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RTScontroller.SetActive(false);
        SelectedArea.SetActive(false);
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        temp_building.transform.localScale = new Vector2(0.3f, 0.3f);
        temp_building.transform.position = new Vector3(Worldpos.x, Worldpos.y, -2.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        temp_building.transform.position = new Vector3(100.0f, 100.0f, -2.0f);
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (( Worldpos is { x: >= 0 and <= 50, y: >= 0 and <= 50 } || startTutorial ) && buildingController.GetComponent<BuildingController>().check_avai(Worldpos))
        {
            
            Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y + 0.5f));
            GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
            //if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
            //{
            //Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
            //    new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            GameObject new_building = Instantiate(gamePrefab, new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            //growthDemo.Position2GroundManager(pos).SetGrowthed();

            buildingController.GetComponent<BuildingController>().register_building(pos, new_building);
            //}

            EventBus.Publish(new BuildingEndDragEvent());
        }

        holder.GetComponent<SpellCooldown>().reStart();
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector2(1, 1);
        RTScontroller.SetActive(true);
    }
}