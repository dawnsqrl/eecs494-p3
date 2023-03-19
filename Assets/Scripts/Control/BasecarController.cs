using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class BasecarController : MonoBehaviour
{
    [SerializeField] bool isChosen = false;
    [SerializeField] private float speed = 4f;
    bool isDialogBlocking;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
    }
    // Start is called before the first frame update
    void Start()
    {
        isDialogBlocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Move the player in the direction of the input
        Vector3 direction = new Vector3(horizontalInput, verticalInput,0);
        transform.position += direction.normalized * speed * Time.deltaTime;

        //growth
        if(Mouse.current.leftButton.wasPressedThisFrame && !isDialogBlocking) {
            GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
            Vector2 pos = new Vector2(Mathf.FloorToInt(transform.position.x + 0.5f), Mathf.FloorToInt(transform.position.y + 0.5f));
            if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
                    new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
                growthDemo.Position2GroundManager(pos).SetGrowthed();
            }
        }
    }

}
