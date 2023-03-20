using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class MushroomControl : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] bool isChosen = false;

    private bool isDialogBlocking;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
    }

    private void Start()
    {
        isDialogBlocking = false;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isDialogBlocking)
        {
            if (isChosen)
            {
                Vector3 Worldpos = targetCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                if (Worldpos is { x: >= 0 and <= 50, y: >= 0 and <= 50 })
                {
                    Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y + 0.5f));
                    GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
                    print(pos);
                    if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
                    {
                        Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
                            new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
                        growthDemo.Position2GroundManager(pos).SetGrowthed();
                        growthDemo.AddToEdge(pos);
                    }
                }

                isChosen = false;
            }

            Ray ray = targetCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider is not null && hit.collider.CompareTag("Mushroom"))
                {
                    isChosen = !isChosen;
                    print(isChosen);
                }
            }
        }
    }

    private void OnDestroy()
    {
        CitizenControl.citizenList.Remove(gameObject);
        Destroy(GameObject.Find("GrowthDemoController"));
    }
}