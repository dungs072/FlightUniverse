using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private float distance = 20f;
    [SerializeField] private RectTransform crosshairUI;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private LayerMask layerMask;
    private Transform spaceship;
    private Camera mainCamera;
    public float crosshairSpeed = 5f;
    private Vector3 hitPoint;
    private void Start()
    {
        mainCamera = Camera.main;
        
    }

    private void Update()
    {
        if (spaceship != null)
        {
            Vector2 screenPoint = worldToUISpace(hitPoint);
            crosshairUI.position = Vector3.Lerp(crosshairUI.position,screenPoint,Time.deltaTime*crosshairSpeed);
        }
    }
    private void FixedUpdate() {
        if(spaceship==null){return;}
        if(Physics.Raycast(spaceship.transform.position, spaceship.transform.forward,out RaycastHit hit,999f,layerMask))
        {
            hitPoint = hit.point;
        }
        else
        {
            hitPoint = spaceship.transform.position+spaceship.forward*distance;
        }
    }

    public Vector3 worldToUISpace(Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
    
    public void SetSpaceShip(Transform spaceship)
    {
        this.spaceship = spaceship;
        hitPoint = spaceship.transform.position+spaceship.forward*distance;
    }
}
