using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DecayBuildingDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject holder;

    //[SerializeField] private GameObject gamePrefab;
    [SerializeField] private Sprite buildingTexture;
    [SerializeField] private GameObject RTScontroller, growthDemo, SelectedArea, fog;

    [SerializeField] private GridManager gridManager;

    //[SerializeField] private BuilderGridManager TgridManager;
    //private ViewDragging vd;
    //private int defenceRange = 3;

    private Transform parentAfterDrag;

    private GameObject buildingController;

    //private GrowthDemo growthDemo;
    private bool startTutorial = false;
    private bool isBuilderTutorialActive = false;
    private bool isDialogBlocking;

    private int decayRange = 4;
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
        EventBus.Publish(new UpdateCursorEvent(buildingTexture, 420, 0.8f));
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
        Color white = new Color(1.0f, 1.0f, 1.0f, 58.0f / 255.0f);
        Color blue = new Color(1.0f, 0.0f, 0.0f, 58.0f / 255.0f);

        foreach (Vector2 oldPos in oldPos_list)
        {
            setHighlight(oldPos, false, white);
        }

        //setHighlight(oldPos2, false, white);
        //setHighlight(oldPos3, false, white);
        //setHighlight(oldPos4, false, white);

        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //if (startTutorial)
        //{
        //    Worldpos = new Vector3(Worldpos.x - 70.0f, Worldpos.y - 70.0f, Worldpos.z);
        //}

        //     // temp_building.transform.localScale = new Vector2(0.3f, 0.3f);
        //     // temp_building.transform.position = new Vector3(Worldpos.x, Worldpos.y, -2.0f);
        Vector2 pos1 = new Vector2(Mathf.FloorToInt(Worldpos.x - (decayRange / 2 - 0.5f) + 0.5f),
            Mathf.CeilToInt(Worldpos.y + (decayRange / 2 - 0.5f) - 0.5f));
        if (pos1 is { x: >= 0 and < 50, y: >= 0 and < 50 })
            pos_list.Add(pos1);
        for (int i = 0; i < decayRange; i++)
        {
            for (int j = 0; j < decayRange; j++)
            {
                Vector2 p = new Vector2(Mathf.FloorToInt(pos1.x + (float)i), Mathf.CeilToInt(pos1.y - (float)j));
                if (p is { x: >= 0 and < 50, y: >= 0 and < 50 })
                    pos_list.Add(p);
            }
        }

        //Vector2 pos2 = new Vector2(Mathf.FloorToInt(pos1.x + 1.1f), Mathf.CeilToInt(pos1.y));
        //Vector2 pos3 = new Vector2(Mathf.FloorToInt(pos1.x + 1.1f), Mathf.CeilToInt(pos1.y - 1.1f));
        //Vector2 pos4 = new Vector2(Mathf.FloorToInt(pos1.x), Mathf.CeilToInt(pos1.y - 1.1f));
        foreach (Vector2 pos in pos_list)
            setHighlight(pos, true, CheckAvai(pos) ? white : blue);
        //setHighlight(pos1, true, CheckAvai(pos1) ? white : blue);
        //setHighlight(pos2, true, CheckAvai(pos2) ? white : blue);
        //setHighlight(pos3, true, CheckAvai(pos3) ? white : blue);
        //setHighlight(pos4, true, CheckAvai(pos4) ? white : blue);

        oldPos_list = new List<Vector2>(pos_list);
        pos_list.Clear();
        //oldPos1 = pos1;
        //oldPos2 = pos2;
        //oldPos3 = pos3;
        //oldPos4 = pos4;

        //vd.enabled = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDialogBlocking)
        {
            EventBus.Publish(new UpdateCursorEvent(null));
            return;
        }

        //vd.enabled = false;
        //GameObject new_building;
        Color white = new Color(1.0f, 1.0f, 1.0f, 58.0f / 255.0f);
        // temp_building.transform.position = new Vector3(100.0f, 100.0f, -2.0f);
        EventBus.Publish(new UpdateCursorEvent(null));
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //if (startTutorial)
        //{
        //    Worldpos = new Vector3(Worldpos.x - 70.0f, Worldpos.y - 70.0f, Worldpos.z);
        //}
        bool avaCheckRes = false;
        foreach (Vector2 oldPos in oldPos_list)
        {
            if (CheckAvai(oldPos))
                avaCheckRes = true;
        }

        if ((Worldpos is { x: >= 0 and < 50, y: >= 0 and < 50 }) //|| startTutorial)
            && avaCheckRes)
        {
            //Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y - 0.5f));
            // GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
            //if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
            //{
            //Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
            //    new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            //if (startTutorial)
            //{
            //    new_building = Instantiate(gamePrefab,
            //        new Vector3(oldPos1.x + 0.5f + 70.0f, oldPos1.y - 0.5f + 70.0f, -2.0f), Quaternion.identity);
            //}
            //else
            //{
            //new_building = Instantiate(gamePrefab, new Vector3(oldPos1.x + 0.5f, oldPos1.y - 0.5f, -2.0f),
            //Quaternion.identity);
            //}
            //growthDemo.Position2GroundManager(pos).SetGrowthed();

            //ChangeAlpha(oldPos1, 1.0f);
            //ChangeAlpha(oldPos2, 1.0f);
            //ChangeAlpha(oldPos3, 1.0f);
            //ChangeAlpha(oldPos4, 1.0f);

            //buildingController.GetComponent<BuildingController>().register_building(oldPos1, new_building);
            //}

            if (startTutorial)
            {
                fog.SetActive(true);
            }

            AudioClip clip = Resources.Load<AudioClip>("Audio/Shovel");
            AudioSource.PlayClipAtPoint(clip, GameProgressControl.audioListenerPos);

            foreach (Vector2 oldPos in oldPos_list)
            {
                if (isBuilderTutorialActive)
                {
                    if (GameObject.Find("BuilderTutorial").GetComponent<NewBuilderTutorialController>().Position2Mucused(oldPos))
                        removeMucus(oldPos);
                }
                else
                {
                    if (growthDemo.GetComponent<GrowthDemo>().Position2Mucused(oldPos))
                        removeMucus(oldPos);
                }
            }
            EventBus.Publish(new BuildingEndDragEvent());

            holder.GetComponent<SpellCooldown>().reStart();
            GameObject.Find("VitalityController").GetComponent<VitalityController>().decreaseVitality(150);
        }

        foreach (Vector2 oldPos in oldPos_list)
            setHighlight(oldPos, false, white);

        transform.SetParent(parentAfterDrag);
        transform.position = transform.parent.position;
        transform.localScale = new Vector2(1, 1);
        RTScontroller.SetActive(true);
        //vd.enabled = true;

        EventBus.Publish(new EndBuildingDragEvent());
    }

    private void removeMucus(Vector2 pos)
    {
        if (pos.x < 0 || pos.x > 49 || pos.y < 0 || pos.y > 49)
            return;
        gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).GetComponent<GroundTileManager>()
            .RemoveMucus();
        gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).GetComponent<GroundTileManager>()
            .RemoveGrowthed();
    }

    bool CheckAvai(Vector2 pos)
    {
        if (!isBuilderTutorialActive)
        {
            GrowthDemo gd = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
            if (gd.Position2Mucused(pos))
                return true;
            return false;
        }
        else
        {
            NewBuilderTutorialController gd = GameObject.Find("BuilderTutorial")
                .GetComponent<NewBuilderTutorialController>();
            if (gd.Position2Mucused(pos) && buildingController.GetComponent<BuildingController>().check_avai(pos))
                return true;
            return false;
        }
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
        //if (startTutorial)
        //    return TgridManager.GetTileGroundAtPosition(TgridManager.GetTileAtPosition(pos)).gameObject
        //        .GetComponent<SpriteRenderer>();
        return gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).gameObject
            .GetComponent<SpriteRenderer>();
    }

    void setHighlight(Vector2 pos, bool status, Color color)
    {
        //if (startTutorial)
        //{
        //    TgridManager.GetTileGroundAtPosition(TgridManager.GetTileAtPosition(pos)).gameObject.transform
        //        .Find("Highlight").gameObject.SetActive(status);
        //    TgridManager.GetTileGroundAtPosition(TgridManager.GetTileAtPosition(pos)).gameObject.transform
        //        .Find("Highlight").gameObject.GetComponent<SpriteRenderer>().color = color;

        //    return;
        //}

        gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).gameObject.transform.Find("Highlight")
            .gameObject.SetActive(status);
        gridManager.GetTileGroundAtPosition(gridManager.GetTileAtPosition(pos)).gameObject.transform.Find("Highlight")
            .gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}