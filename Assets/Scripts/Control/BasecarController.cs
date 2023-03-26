using UnityEngine;
using UnityEngine.InputSystem;

public class BasecarController : MonoBehaviour
{
    [SerializeField] private bool isChosen = false;
    [SerializeField] private float speed = 4f;
    [SerializeField] private Animator _animator;
    [SerializeField] private HitHealth _snailHealth;

    private Controls controls;
    private Controls.PlayerActions playerActions;
    private bool isDialogBlocking;
    public Vector3 direction;
    public Vector3 forwardDirection;
    public bool on_wall;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        controls = new Controls();
        playerActions = controls.Player;
        forwardDirection = Vector3.zero;
    }

    private void Start()
    {
        isDialogBlocking = false;
        on_wall = false;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (_snailHealth.health <= 0)
        {
            _animator.SetBool("is_dead", true);
            return;
        }
        
        if (GameProgressControl.isGameActive && !isDialogBlocking)
        {
            // Move the player in the direction of the input
            direction = playerActions.MoveBaseCar.ReadValue<Vector2>();
            if (on_wall && (direction.x * forwardDirection.x > 0 || direction.y * forwardDirection.y > 0))
            {
                return;
            }
            _animator.SetFloat("dir_x", direction.x);
            transform.position += direction.normalized * (
                speed * SimulationSpeedControl.GetSimulationSpeed() * Time.deltaTime
            );
            
            // _rigidbody.AddForce(direction.y * 5 * transform.forward);
            // _rigidbody.AddForce(direction.x * 5 * transform.right);

            if (!on_wall && direction != Vector3.zero)
            {
                forwardDirection = direction;
            }
        }

        // growth
        //if (Mouse.current.leftButton.wasPressedThisFrame && !isDialogBlocking)
        //{
        //    GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
        //    Vector2 position = transform.position;
        //    position = new Vector2(
        //        Mathf.FloorToInt(position.x + 0.5f), Mathf.FloorToInt(position.y + 0.5f)
        //    );
        //    if (!growthDemo.Position2Growthed(position) && !growthDemo.FakeGrowthed(position))
        //    {
        //        Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
        //            new Vector3(position.x, position.y, -2.0f), Quaternion.identity);
        //        growthDemo.Position2GroundManager(position).SetGrowthed();
        //    }
        //}
    }
}