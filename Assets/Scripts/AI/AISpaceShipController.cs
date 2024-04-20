using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceShipController : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private Transform detectorPoint;
    [Header("Attributes")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationalDamp = .5f;
    [SerializeField] private float detectionDistance = 20f;
    [SerializeField] private float rayCastOffset = 2.5f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float offsetRate = 2f;
    [Header("Reference")]
    [SerializeField] private AISpaceShipFighter fighter;
    [Header("Layer")]
    [SerializeField] private LayerMask obstacleLayer;

    private bool isStopped = false;

    private void Update()
    {
        if (target == null) { return; }
        Fight();
        if (!isStopped)
        {
            FindPath();
            Move();
        }


    }
    private void Turn()
    {
        Vector3 pos = (target.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
    }
    private void Move()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    private void FindPath()
    {
        RaycastHit hit;
        Vector3 offset = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        // Debug.DrawRay(left, transform.forward * detectionDistance, Color.cyan);
        // Debug.DrawRay(right, transform.forward * detectionDistance, Color.cyan);
        // Debug.DrawRay(up, transform.forward * detectionDistance, Color.cyan);
        // Debug.DrawRay(down, transform.forward * detectionDistance, Color.cyan);

        if (Physics.Raycast(left, transform.forward, out hit, detectionDistance, obstacleLayer))
        {
            Debug.DrawLine(left, hit.point, Color.green);
            offset += transform.right*offsetRate;
        }
        else if (Physics.Raycast(right, transform.forward, out hit, detectionDistance, obstacleLayer))
        {
            Debug.DrawLine(right, hit.point, Color.green);
            offset -= transform.right*offsetRate;
        }
        else
        {
            Debug.DrawRay(left, transform.forward * detectionDistance, Color.cyan);
            Debug.DrawRay(right, transform.forward * detectionDistance, Color.cyan);
        }
        if (Physics.Raycast(up, transform.forward, out hit, detectionDistance, obstacleLayer))
        {
            Debug.DrawLine(up, hit.point, Color.green);
            offset -= transform.up*offsetRate;
        }
        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance, obstacleLayer))
        {
            Debug.DrawLine(down, hit.point, Color.green);
            offset += transform.up*offsetRate;
        }
        else
        {
            Debug.DrawRay(up, transform.forward * detectionDistance, Color.cyan);
            Debug.DrawRay(down, transform.forward * detectionDistance, Color.cyan);
        }



        if (offset != Vector3.zero)
        {
            transform.Rotate(offset * rotationalDamp * Time.deltaTime);
        }
        else
        {
            Turn();
        }

    }
    private void Fight()
    {
        if (CheckDistance(target.position))
        {
            isStopped = true;

            if (Physics.Raycast(detectorPoint.position, detectorPoint.forward, out RaycastHit hit, stopDistance, obstacleLayer))
            {
                Debug.DrawRay(transform.position, hit.point, Color.red);
                FindPath();
                Move();
            }
            else
            {
                Turn();
                fighter.Attack();
            }

        }
        else
        {
            isStopped = false;
        }
    }
    private bool CheckDistance(Vector3 targetPoint)
    {
        return (targetPoint - transform.position).sqrMagnitude <= stopDistance * stopDistance;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

}
