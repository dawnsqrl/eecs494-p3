using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DestoryBuildingDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject holder;

    //[SerializeField] private GameObject gamePrefab;
    [SerializeField] private Sprite buildingTexture;
    [SerializeField] private GameObject RTScontroller, SelectedArea, fog;

    [SerializeField] private GridManager gridManager;

    //[SerializeField] private BuilderGridManager TgridManager;
    //private ViewDragging vd;
    //private int defenceRange = 3;

    private Transform parentAfterDrag;

    private GameObject buildingController;

    //private GrowthDemo growthDemo;
    private bool startTutorial = false;
    bool isBuilderTutorialActive = false;
    private bool isDialogBlocking;

    private Vector2 oldPos1 = Vector2.zero;

    //private int decayRange = 4;
    private List<Vector2> pos_list, oldPos_list;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        pos_list = new List<Vector2>();
        oldPos_list = new List<Vector2>();
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => startTutorial = true);
        buildingController = GameObject.Find("BuildingCanvas");
        //growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
    }

    private void Start()
    {
        //vd = Camera.main.gameObject.GetComponent<ViewDragging>();
        // buildingTexture.Reinitialize(100, 100);
        isDialogBlocking = false;
        EventBus.Publish(new UpdateCursorEvent(null));
        // temp_building = Instantiate(gameMapPrefab, new Vector3(100.0f, 100.0f, -2.0f), Quaternion.identity);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDialogBlocking)
        {
            EventBus.Publish(new UpdateCursorEvent(null));
            return;
        }

        EventBus.Publish(new StartBuildingDragEvent());
        RTScontroller.SetActive(false);
        SelectedArea.SetActive(false);
        parentAfterDrag = transform.parent;
        EventBus.Publish(new UpdateCursorEvent(buildingTexture, 100, 0.8f));
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDialogBlocking)
        {
            EventBus.Publish(new UpdateCursorEvent(null));
            return;
        }

        //vd.enabled = false;
        setHighlight(oldPos1, false);

        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos1 = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.CeilToInt(Worldpos.y - 0.5f));

        setHighlight(pos1, CheckAvai(pos1) ? true : false);
        oldPos1 = pos1;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDialogBlocking)
        {
            EventBus.Publish(new UpdateCursorEvent(null));
            return;
        }

        EventBus.Publish(new UpdateCursorEvent(null));
        //Color white = new Color(1.0f, 1.0f, 1.0f, 58.0f / 255.0f);

        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if ((Worldpos is { x: >= 0 and <= 50, y: >= 0 and <= 50 }) //|| startTutorial)
            && CheckAvai(oldPos1))
        {
            //new_building = Instantiate(gamePrefab, new Vector3(oldPos1.x, oldPos1.y, -2.0f), Quaternion.identity);

            //posForBuilding = oldPos1;
            GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().destoryBuildingUnregister(oldPos1);

            holder.GetComponent<SpellCooldown>().reStart();
            EventBus.Publish(new BuildingEndDragEvent());

            if (startTutorial)
            {
                fog.SetActive(true);
            }
        }

        setHighlight(oldPos1, false);
        transform.SetParent(parentAfterDrag);
        transform.position = transform.parent.position;
        transform.localScale = new Vector2(1, 1);
        RTScontroller.SetActive(true);
        //vd.enabled = true;

        EventBus.Publish(new EndBuildingDragEvent());
    }

    private void removeMucus(Vector2 pos)
    {
        gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).GetComponent<GroundTileManager>()
            .RemoveMucus();
    }

    bool CheckAvai(Vector2 pos)
    {
        return !GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().check_avai(pos);
    }

    void setHighlight(Vector2 pos, bool status)
    {
        GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().setBuildingHighlight(pos, status);
    }
}