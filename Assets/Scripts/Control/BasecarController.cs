using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasecarController : MonoBehaviour
{
    [SerializeField] private bool isChosen = false;
    [SerializeField] private Vector3 tutorialInitPos;
    [SerializeField] private Vector3 gameInitPos;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Animator _animator;
    [SerializeField] private HitHealth _snailHealth;

    private Controls controls;
    private Controls.PlayerActions playerActions;
    private bool isDialogBlocking;
    public Vector3 direction;
    public Vector3 forwardDirection;
    private Rigidbody _rigidbody;
    public bool is_tutorial;
    public bool is_tutorial_end;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        EventBus.Subscribe<StartSnailTutorialEvent>(_ => StartTutorial());
        EventBus.Subscribe<GameStartEvent>(_ => StartGame());
        controls = new Controls();
        playerActions = controls.Player;
        forwardDirection = Vector3.zero;
    }

    private void Start()
    {
        isDialogBlocking = false;
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

        if ((is_tutorial || GameProgressControl.isGameActive) && !isDialogBlocking)
        {
            // Move the player in the direction of the input
            direction = playerActions.MoveBaseCar.ReadValue<Vector2>();
            _animator.SetFloat("dir_x", direction.x);

            if (direction.magnitude > 0)
            {
                _rigidbody.velocity = direction.normalized * (
                    speed * SimulationSpeedControl.GetSimulationSpeed()
                );
         
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
            }
            if (direction != Vector3.zero)
            {
                forwardDirection = direction;
            }
            
            GameObject tile = null;
            if (is_tutorial)
            {
                tile = CaveGridManager._tiles[GetSnailPos(transform.position.x + 60, transform.position.y)];
            }
            else
            {
                tile = GridManager._tiles[GetSnailPos(transform.position.x, transform.position.y)];
            }
            if (tile)
            {
                GroundTileManager _groundTileManager = tile.GetComponentInChildren<GroundTileManager>();
                if (!_groundTileManager.mucused)
                {
                    _groundTileManager.SetMucus();
                }
            }
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
    
    Vector2 GetSnailPos(float x, float y) {
        if (x < 0) {
            x = 0;
        }
        if (y < 0) {
            y = 0;
        }
        return new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }

    private void StartTutorial()
    {
        is_tutorial = true;
        is_tutorial_end = false;
        transform.position = tutorialInitPos;
    }

    private void StartGame()
    {
        is_tutorial = false;
        transform.position = gameInitPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!is_tutorial_end && other.CompareTag("SnailTutorialEndTrigger"))
        {
            EventBus.Publish("EndSnailTutorialEvent");
            is_tutorial_end = true;
        }
    }
}