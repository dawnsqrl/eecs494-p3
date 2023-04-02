using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject gamePrefab;
    [SerializeField] private Sprite buildingTexture;
    [SerializeField] private GameObject RTScontroller, SelectedArea;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private BuilderGridManager TgridManager;
    [SerializeField] private ViewDragging vd;
    private int defenceRange = 3;

    private Transform parentAfterDrag;
    private GameObject buildingController;
    private GrowthDemo growthDemo;
    private bool startTutorial = false;

    Vector2 oldPos1 = Vector2.zero, oldPos2 = Vector2.zero, oldPos3 = Vector2.zero, oldPos4 = Vector2.zero;

    GameObject AttackRange;
    Vector3 denfenceOriginScale;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => startTutorial = true);
        buildingController = GameObject.Find("BuildingCanvas");
        growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();

        if (gameObject.name == "Defence")
        {
            AttackRange = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/RangeCircle"),
                new Vector3(100.0f, 100.0f, 0.0f), Quaternion.identity);
            denfenceOriginScale = AttackRange.transform.localScale;
        }
    }

    private void Start()
    {
        // buildingTexture.Reinitialize(100, 100);
        EventBus.Publish(new UpdateCursorEvent(null));
        // temp_building = Instantiate(gameMapPrefab, new Vector3(100.0f, 100.0f, -2.0f), Quaternion.identity);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        EventBus.Publish(new StartBuildingDragEvent());
        RTScontroller.SetActive(false);
        SelectedArea.SetActive(false);
        parentAfterDrag = transform.parent;
        EventBus.Publish(new UpdateCursorEvent(buildingTexture, 128, 0.8f));
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        vd.enabled = false;
        Color white = new Color(1.0f, 1.0f, 1.0f, 58.0f / 255.0f);
        Color blue = new Color(1.0f, 0.0f, 0.0f, 58.0f / 255.0f);

        setHighlight(oldPos1, false, white);
        setHighlight(oldPos2, false, white);
        setHighlight(oldPos3, false, white);
        setHighlight(oldPos4, false, white);

        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (startTutorial)
        {
            Worldpos = new Vector3(Worldpos.x - 70.0f, Worldpos.y - 70.0f, Worldpos.z);
        }

        //     // temp_building.transform.localScale = new Vector2(0.3f, 0.3f);
        //     // temp_building.transform.position = new Vector3(Worldpos.x, Worldpos.y, -2.0f);
        Vector2 pos1 = new Vector2(Mathf.FloorToInt(Worldpos.x), Mathf.CeilToInt(Worldpos.y));
        Vector2 pos2 = new Vector2(Mathf.FloorToInt(pos1.x + 1.1f), Mathf.CeilToInt(pos1.y));
        Vector2 pos3 = new Vector2(Mathf.FloorToInt(pos1.x + 1.1f), Mathf.CeilToInt(pos1.y - 1.1f));
        Vector2 pos4 = new Vector2(Mathf.FloorToInt(pos1.x), Mathf.CeilToInt(pos1.y - 1.1f));

        setHighlight(pos1, true, CheckAvai(pos1) ? white : blue);
        setHighlight(pos2, true, CheckAvai(pos2) ? white : blue);
        setHighlight(pos3, true, CheckAvai(pos3) ? white : blue);
        setHighlight(pos4, true, CheckAvai(pos4) ? white : blue);

        oldPos1 = pos1;
        oldPos2 = pos2;
        oldPos3 = pos3;
        oldPos4 = pos4;

        //vd.enabled = true;

        if (gameObject.name == "Defence")
        {
            AttackRange.transform.localScale = denfenceOriginScale * defenceRange;
            AttackRange.transform.position = new Vector3(Worldpos.x + 0.5f + 0.2f, Worldpos.y - 0.5f - 0.2f, -2.0f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //vd.enabled = false;
        GameObject new_building;
        Color white = new Color(1.0f, 1.0f, 1.0f, 58.0f / 255.0f);
        // temp_building.transform.position = new Vector3(100.0f, 100.0f, -2.0f);
        EventBus.Publish(new UpdateCursorEvent(null));
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (startTutorial)
        {
            Worldpos = new Vector3(Worldpos.x - 70.0f, Worldpos.y - 70.0f, Worldpos.z);
        }

        if ((Worldpos is { x: >= 0 and <= 50, y: >= 0 and <= 50 } || startTutorial)
            && CheckAvai(oldPos1) && CheckAvai(oldPos2) && CheckAvai(oldPos3) && CheckAvai(oldPos4))
        {
            Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y - 0.5f));
            // GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
            //if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
            //{
            //Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
            //    new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            if (startTutorial)
            {
                new_building = Instantiate(gamePrefab,
                    new Vector3(oldPos1.x + 0.5f + 70.0f, oldPos1.y - 0.5f + 70.0f, -2.0f), Quaternion.identity);
                if (gameObject.name == "Defence")
                {
                    AttackRange.transform.position = new Vector3(100.0f, 100.0f, 0.0f);
                    new_building.GetComponent<DefenceBuilding>().SetPosition(
                        new Vector3(oldPos1.x + 0.5f + 70.0f, oldPos1.y - 0.5f + 70.0f, -2.0f), defenceRange);
                }
            }
            else
            {
                new_building = Instantiate(gamePrefab, new Vector3(oldPos1.x + 0.5f, oldPos1.y - 0.5f, -2.0f),
                    Quaternion.identity);
                if (gameObject.name == "Defence")
                {
                    AttackRange.transform.position = new Vector3(100.0f, 100.0f, 0.0f);
                    new_building.GetComponent<DefenceBuilding>()
                        .SetPosition(new Vector3(oldPos1.x + 0.5f, oldPos1.y - 0.5f, -2.0f), defenceRange);
                }
            }
            //growthDemo.Position2GroundManager(pos).SetGrowthed();

            //ChangeAlpha(oldPos1, 1.0f);
            //ChangeAlpha(oldPos2, 1.0f);
            //ChangeAlpha(oldPos3, 1.0f);
            //ChangeAlpha(oldPos4, 1.0f);
            setHighlight(oldPos1, false, white);
            setHighlight(oldPos2, false, white);
            setHighlight(oldPos3, false, white);
            setHighlight(oldPos4, false, white);

            buildingController.GetComponent<BuildingController>().register_building(oldPos1, new_building);
            //}

            EventBus.Publish(new BuildingEndDragEvent());

            holder.GetComponent<SpellCooldown>().reStart();
        }
        else
        {
            setHighlight(oldPos1, false, white);
            setHighlight(oldPos2, false, white);
            setHighlight(oldPos3, false, white);
            setHighlight(oldPos4, false, white);
        }

        if (gameObject.name == "Defence")
        {
            AttackRange.transform.position = new Vector3(100.0f, 100.0f, 0.0f);
        }

        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector2(1, 1);
        RTScontroller.SetActive(true);
        vd.enabled = true;

        EventBus.Publish(new EndBuildingDragEvent());
    }

    bool CheckAvai(Vector2 pos)
    {
        if (startTutorial)
            return buildingController.GetComponent<BuildingController>().check_avai(pos);
        GrowthDemo gd = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
        if (gd.Position2Growthed(pos) && buildingController.GetComponent<BuildingController>().check_avai(pos))
            return true;
        return false;
    }

    IEnumerator GenerateShade(float time, Vector2 pos, float alpha)
    {
        ChangeAlpha(pos, alpha);
        yield return new WaitForSeconds(time);
        ChangeAlpha(pos, 1.0f);
    }

    void ChangeAlpha(Vector2 pos, float alpha)
    {
        GetSpriteRenderAtPos(pos).color = new Color(GetSpriteRenderAtPos(pos).color.r,
            GetSpriteRenderAtPos(pos).color.g, GetSpriteRenderAtPos(pos).color.b, alpha);
    }

    SpriteRenderer GetSpriteRenderAtPos(Vector2 pos)
    {
        if (startTutorial)
            return TgridManager.GetTileGroundAtPosition(TgridManager.GetTileAtPosition(pos)).gameObject
                .GetComponent<SpriteRenderer>();
        return gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).gameObject
            .GetComponent<SpriteRenderer>();
    }

    void setHighlight(Vector2 pos, bool status, Color color)
    {
        if (startTutorial)
        {
            TgridManager.GetTileGroundAtPosition(TgridManager.GetTileAtPosition(pos)).gameObject.transform
                .Find("Highlight").gameObject.SetActive(status);
            TgridManager.GetTileGroundAtPosition(TgridManager.GetTileAtPosition(pos)).gameObject.transform
                .Find("Highlight").gameObject.GetComponent<SpriteRenderer>().color = color;

            return;
        }

        gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).gameObject.transform.Find("Highlight")
            .gameObject.SetActive(status);
        gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).gameObject.transform.Find("Highlight")
            .gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}