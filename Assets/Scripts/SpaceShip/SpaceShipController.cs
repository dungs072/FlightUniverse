using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
public class SpaceShipController : NetworkBehaviour
{
    [Header("Attack")]
    [SerializeField] private Fighter fighter;
    [Header("Mechanic")]
    [SerializeField] private MechanicController mechanicController;

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
    private Controls controls;
    private Rigidbody rb;
    // network vars
    //private NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    private void Awake()
    {
        controls = new Controls();
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
            rb = GetComponent<Rigidbody>();
            screenCenter.x = Screen.width / 2;
            screenCenter.y = Screen.height / 2;
            Cursor.lockState = CursorLockMode.Confined;
            mechanicController.ToggleForwardTrail(false);
            mechanicController.ToggleBackwardTrails(false);
            InputRegister();
            NetworkRegister();
            NetworkManager.Singleton.GetComponent<MyNetworkManager>().CinemachineVirtual.Follow = transform;
            NetworkManager.Singleton.GetComponent<MyNetworkManager>().CinemachineVirtual.LookAt = transform;
            rb.isKinematic = false;
        }

    }
    private void NetworkRegister()
    {
        //Position.OnValueChanged+=OnPositionChanged;
    }
    private void InputRegister()
    {
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
        controls.Player.Roll.performed += OnRoll;
        controls.Player.Roll.canceled += OnRoll;
        controls.Player.UpDown.performed += OnUpDown;
        controls.Player.UpDown.canceled += OnUpDown;
        controls.Player.Thrust.performed += OnThrust;
        controls.Player.Thrust.canceled += OnUnThrust;
        // controls.Player.Attack.performed+=OnAttack;
        // controls.Player.Attack.canceled += OnAttack;
    }
    private void Update()
    {
        if (!IsOwner) { return; }
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        lookInput = mousePosition;
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;
        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        rollValue = Mathf.Lerp(rollValue, rollInput, rollAcceleration * Time.deltaTime);
        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollValue * rollSpeed * Time.deltaTime, Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, moveInput.y * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, moveInput.x * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, hoverInput * hoverSpeed, hoverAcceleration * Time.deltaTime);
        transform.position += transform.forward * activeForwardSpeed * thrustValue * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);
        // Position.Value += transform.forward * activeForwardSpeed * thrustValue * Time.deltaTime;
        // Position.Value += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);
    }
    private void HandleAttack()
    {
        if(controls.Player.Attack.IsPressed())
        {
            fighter.Attack();
        }
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
        moveInput = ctx.ReadValue<Vector2>();
        ResetRigidbody();
        if (moveInput.y == 1)
        {
            mechanicController.ToggleForwardTrail(true);

        }
        else if (moveInput.y == -1)
        {
            mechanicController.ToggleBackwardTrails(true);
        }
        else
        {
            mechanicController.ToggleForwardTrail(false);
            mechanicController.ToggleBackwardTrails(false);
        }
    }
    private void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        rollInput = ctx.ReadValue<float>();
    }
    private void OnUpDown(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        hoverInput = ctx.ReadValue<float>();
    }
    private void OnThrust(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        thrustValue = thrustSpeed;
    }
    private void OnUnThrust(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) { return; }
        thrustValue = 1f;
    }
    private void OnAttack(InputAction.CallbackContext ctx)
    {
        fighter.Attack();
    }
    #endregion
}
