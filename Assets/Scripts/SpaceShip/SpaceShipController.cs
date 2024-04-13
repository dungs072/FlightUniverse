using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
public class SpaceShipController : NetworkBehaviour
{
    [SerializeField] private float forwardSpeed = 25f;
    [SerializeField] private float strafeSpeed = 7.5f;
    [SerializeField] private float hoverSpeed = 5f;
    [SerializeField] private float lookRateSpeed = 90f;
    [SerializeField] private float rollSpeed = 90f;
    [SerializeField] private float rollAcceleration = 3.5f;
    private float activeForwardSpeed;
    private float activeStrafeSpeed;
    private float activeHoverSpeed;
    private float forwardAcceleration = 2.5f;
    private float strafeAcceleration = 2f;
    private float hoverAcceleration = 2f;
    private Vector2 lookInput, screenCenter, mouseDistance,moveInput;
    private float rollInput;
    private Controls controls;
    private void Awake() {
        controls = new Controls();
    }
    private void OnEnable() {
        controls.Player.Enable();
    }
    private void OnDisable() {
        controls.Player.Disable();
    }
    private void Start() {
        screenCenter.x = Screen.width / 2;
        screenCenter.y = Screen.height / 2;
        Cursor.lockState = CursorLockMode.Confined;
        controls.Player.Move.performed+=OnMove;
        controls.Player.Move.canceled+=OnMove;
    }
    private void Update() {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;
        mouseDistance.x = (lookInput.x-screenCenter.x)/screenCenter.y;
        mouseDistance.y = (lookInput.y-screenCenter.y)/screenCenter.y;
        mouseDistance = Vector2.ClampMagnitude(mouseDistance,1f);
        rollInput = Mathf.Lerp(rollInput,Input.GetAxisRaw("Roll"),rollAcceleration*Time.deltaTime);
        transform.Rotate(-mouseDistance.y*lookRateSpeed*Time.deltaTime,mouseDistance.x*lookRateSpeed*Time.deltaTime, rollInput*rollSpeed*Time.deltaTime,Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed,Input.GetAxisRaw("Vertical")*forwardSpeed,forwardAcceleration*Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,Input.GetAxisRaw("Horizontal")*strafeSpeed,strafeAcceleration*Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,Input.GetAxisRaw("Hover")*hoverSpeed,hoverAcceleration*Time.deltaTime);

        transform.position+=transform.forward*activeForwardSpeed*Time.deltaTime;
        transform.position+=(transform.right*activeStrafeSpeed*Time.deltaTime)+(transform.up*activeHoverSpeed*Time.deltaTime);
    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
}
