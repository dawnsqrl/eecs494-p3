using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BasecarController : MonoBehaviour
{
    [SerializeField] private bool isChosen = false;
    [SerializeField] private Vector3 tutorialInitPos;
    private Vector3 gameInitPos;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Animator _animator;
    [SerializeField] private HitHealth _snailHealth;

    private Controls controls;
    private Controls.PlayerActions playerActions;
    private bool isDialogBlocking;
    public Vector3 direction;
    public Vector3 forwardDirection;
    private Rigidbody _rigidbody;
    public static bool is_tutorial;
    public bool is_tutorial_end;

    private float normalSpeed = 2;
    private float fastSpeed = 5;
    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        EventBus.Subscribe<StartSnailTutorialEvent>(_ => StartTutorial());
        EventBus.Subscribe<GameStartEvent>(_ => StartGame());
        controls = new Controls();
    }

    private void Start()
    {
        isDialogBlocking = false;
        _rigidbody = GetComponent<Rigidbody>();
        playerActions = controls.Player;
        forwardDirection = Vector3.zero;
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

            Vector3 forwardPos = new Vector2((forwardDirection + transform.position).x,
                (forwardDirection + transform.position).y);

            GameObject MacusTile = null;
            GameObject ForwardTile = null;
            if (is_tutorial)
            {
                MacusTile = CaveGridManager._tiles[GetSnailPos(transform.position.x + 60, transform.position.y)];
                ForwardTile = CaveGridManager._tiles[GetSnailPos(forwardPos.x + 60, forwardPos.y)];
            }
            else
            {
                MacusTile = GridManager._tiles[GetSnailPos(transform.position.x, transform.position.y)];
                ForwardTile = GridManager._tiles[GetSnailPos(forwardPos.x, forwardPos.y)];
            }
            if (MacusTile)
            {
                GroundTileManager _groundTileManager = MacusTile.GetComponentInChildren<GroundTileManager>();
                if (!_groundTileManager.mucused)
                {
                    _groundTileManager.SetMucus();
                }
            }
            if (ForwardTile)
            {
                GroundTileManager _groundTileManager = ForwardTile.GetComponentInChildren<GroundTileManager>();
                speed = _groundTileManager.mucused ? fastSpeed : normalSpeed;
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
        Vector3 snailPos = Vector3.zero;
        while (true)
        {
            Vector3 mushroomPos = GameObject.Find("Mushroom").transform.position;
            snailPos = new Vector3(UnityEngine.Random.Range(5, 45), UnityEngine.Random.Range(5, 45), 0);
            if (Vector3.Distance(mushroomPos, snailPos) > 20)
                break;
        }

        transform.position = snailPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!is_tutorial_end && other.CompareTag("SnailTutorialEndTrigger"))
        {
            print("-------------------------------------------");
            EventBus.Publish(new EndSnailTutorialEvent());
            is_tutorial_end = true;
        }
    }
}