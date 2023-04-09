using UnityEngine;

public class MushroomControl : MonoBehaviour
{
    //[SerializeField] private Camera targetCamera;
    [SerializeField] bool isChosen = false;
    [SerializeField] GameObject BuildingCanvas;
    private GameObject buildingController;

    private bool isDialogBlocking;
    private int vitality;
    private GameObject vitalityController;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }

    private void Start()
    {
        isDialogBlocking = false;
    }

    // private void Update()
    // {
    //     if (Mouse.current.leftButton.wasPressedThisFrame && !isDialogBlocking)
    //     {
    //         if (isChosen)
    //         {
    //             Vector3 Worldpos = targetCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //             if (Worldpos is { x: >= 0 and <= 50, y: >= 0 and <= 50 } && vitality > 200)
    //             {
    //                 vitalityController = GameObject.Find("VitalityController");
    //                 vitalityController.GetComponent<VitalityController>().decreaseVitality(150);
    //
    //                 Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y + 0.5f));
    //                 GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
    //                 //print(pos);
    //                 if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
    //                 {
    //                     Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
    //                         new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
    //                     growthDemo.Position2GroundManager(pos).SetGrowthed();
    //                     growthDemo.AddToEdge(pos);
    //                 }
    //             }
    //
    //             isChosen = false;
    //         }
    //
    //         Ray ray = targetCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
    //
    //         if (Physics.Raycast(ray, out RaycastHit hit))
    //         {
    //             if (hit.collider is not null && hit.collider.CompareTag("Mushroom"))
    //             {
    //                 isChosen = !isChosen;
    //                 print(isChosen);
    //             }
    //         }
    //     }
    // }

    private void OnDestroy()
    {
        BuildingCanvas.GetComponent<BuildingController>().unregister_building(gameObject);
        CitizenControl.citizenList.Remove(gameObject);
        Destroy(GameObject.Find("GrowthDemoController"));
    }
}