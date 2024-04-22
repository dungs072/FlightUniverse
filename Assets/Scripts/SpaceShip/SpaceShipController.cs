using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
public class SpaceShipController : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] private Health health;
    [Header("Attack")]
    [SerializeField] private Fighter fighter;
    [Header("Mechanic")]
    [SerializeField] private MechanicController mechanicController;
    [SerializeField] private Transform followTransform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerSound playerSound;

    [Header("Attributes")]
    [SerializeField] private float forwardSpeed = 25f;
    [SerializeField] private float strafeSpeed = 7.5f;
    [SerializeField] private float hoverSpeed = 5f;
    [SerializeField] private float lookRateSpeed = 90f;
    [SerializeField] private float rollSpeed = 90f;
    [SerializeField] private float rollAcceleration = 3.5f;
    [SerializeField] private float thrustSpeed = 100f;
    private float activeForwardSpeed;
    private float activeStrafeSpeed;
    private float activeHoverSpeed;
    private float forwardAcceleration = 2.5f;
    private float strafeAcceleration = 2f;
    private float hoverAcceleration = 2f;
    private Vector2 lookInput, screenCenter, mouseDistance, moveInput;
    private float rollValue;
    private float rollInput;
    private float hoverInput;
    private float thrustValue = 1f;
    private bool canControl = false;
    private Controls controls;
    private Rigidbody rb;
    public MechanicController MechanicController { get { return mechanicController; } }
    public bool CanControl
    {
        get
        {
            return canControl;
        }
        set
        {
            canControl = value;
        }
    }
    // network vars
    //private NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    private void Awake()
    {
        controls = new Controls();
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            screenCenter.x = Screen.width / 2;
            screenCenter.y = Screen.height / 2;
            mechanicController.HandleToggleForwardTrail(false);
            mechanicController.HandleToggleBackwardTrails(false);
            InputRegister();
            CameraController.instance.targetPoint = followTransform;
            UIManager.Instance.CrossHair.SetSpaceShip(transform);
            ReferenceManager.Instance.SetAITarget(transform);
            health.OnHealthChanged+=ChangedHealth;
        }

    }

    private void InputRegister()
    {
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnStopMove;
        controls.Player.Roll.performed += OnRoll;
        controls.Player.Roll.canceled += OnRoll;
        controls.Player.UpDown.performed += OnUpDown;
        controls.Player.UpDown.canceled += OnUpDown;
        controls.Player.Thrust.performed += OnThrust;
        controls.Player.Thrust.canceled += OnUnThrust;
        // controls.Player.Attack.performed+=OnAttack;
        // controls.Player.Attack.canceled += OnAttack;
    }
    private void ChangedHealth(int currentHealth, int maxHealth)
    {
        UIManager.Instance.SetPlayerHealthBarValue(currentHealth,maxHealth);
    }
    public void SetMechanicController(MechanicController mechanicController)
    {
        this.mechanicController = mechanicController;
    }
    private void Update()
    {
        if (!IsOwner) { return; }
        HandleMovement();
        HandleAttack();
    }


    private void HandleMovement()
    {
        if (!CanControl) { return; }
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        lookInput = mousePosition;
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.x;
        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        rollValue = Mathf.Lerp(rollValue, rollInput, rollAcceleration * Time.deltaTime);
        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime,
                        mouseDistance.x * lookRateSpeed * Time.deltaTime,
                        rollValue * rollSpeed * Time.deltaTime, Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, moveInput.y * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, moveInput.x * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, hoverInput * hoverSpeed, hoverAcceleration * Time.deltaTime);
        transform.position += transform.forward * activeForwardSpeed * thrustValue * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);
    }
    private void HandleAttack()
    {
        if (!CanControl) { return; }
        if (controls.Player.Attack.IsPressed())
        {
            fighter.Attack();
        }
        // else
        // {
        //     fighter.ToggleLaser(false);
        // }
    }
    private void OnCollisionEnter(Collision other)
    {
        ResetRigidbody();
        activeForwardSpeed = 0f;
        activeStrafeSpeed = 0f;
        activeHoverSpeed = 0f;
    }
    private void ResetRigidbody()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    #region Input
    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        if (!CanControl) { return; }
        moveInput = ctx.ReadValue<Vector2>();
        playerSound.PlayMovingAudio();
        ResetRigidbody();
        HandleTrail();
    }
    private void OnStopMove(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        if (!CanControl) { return; }
        moveInput = ctx.ReadValue<Vector2>();
        playerSound.StopMovingAudio();
        ResetRigidbody();
        HandleTrail();
    }

    private void HandleTrail()
    {
        mechanicController.HandleToggleForwardTrail(false);
        mechanicController.HandleToggleBackwardTrails(false);
        ToggleForwardTrailServerRpc(false);
        ToggleBackwardTrailsServerRpc(false);
        if (moveInput.y == 1)
        {
            mechanicController.HandleToggleForwardTrail(true);
            ToggleForwardTrailServerRpc(true);

        }
        else if (moveInput.y == -1)
        {
            mechanicController.HandleToggleBackwardTrails(true);
            ToggleBackwardTrailsServerRpc(true);
        }
    }

    private void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        if (!CanControl) { return; }
        rollInput = ctx.ReadValue<float>();
        HandleTrail();
    }
    private void OnUpDown(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        if (!CanControl) { return; }
        hoverInput = ctx.ReadValue<float>();
    }
    private void OnThrust(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        if (!CanControl) { return; }
        thrustValue = thrustSpeed;
        playerSound.StopMovingAudio();
        playerSound.PlayThrustAudio();
        
    }
    private void OnUnThrust(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        if (!CanControl) { return; }
        thrustValue = 1f;
        if(moveInput==Vector2.zero)
        {
            playerSound.StopThrustAudio();
        }
        
    }
    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        if (!CanControl) { return; }
        fighter.Attack();
    }
    #endregion


    #region Network
    [ServerRpc]
    private void ToggleForwardTrailServerRpc(bool state)
    {
        ToggleForwardTrailClientRpc(state);
    }
    [ClientRpc]
    private void ToggleForwardTrailClientRpc(bool state)
    {
        if (IsOwner) { return; }
        mechanicController.ToggleForwardTrail(state);
    }
    [ServerRpc]
    private void ToggleBackwardTrailsServerRpc(bool state)
    {
        ToggleBackwardTrailsClientRpc(state);
    }
    [ClientRpc]
    private void ToggleBackwardTrailsClientRpc(bool state)
    {
        if (IsOwner) { return; }
        mechanicController.ToggleBackwardTrails(state);
    }
    #endregion
}
