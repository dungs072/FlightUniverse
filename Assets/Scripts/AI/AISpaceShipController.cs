using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceShipController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationalDamp = .5f;
    [SerializeField] private float detectionDistance = 20f;
    [SerializeField] private float rayCastOffset = 2.5f;
    [SerializeField] private float stopDistance = 5f;

    private bool isStopped = false;

    private void Update()
    {
        if (target == null) { return; }
        Fight();
        if (!isStopped)
        {
            PathFinding();
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

    private void PathFinding()
    {
        RaycastHit hit;
        Vector3 offset = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        Debug.DrawRay(left, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawRay(right, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawRay(up, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawRay(down, transform.forward * detectionDistance, Color.cyan);

        if (Physics.Raycast(left, transform.forward, out hit, detectionDistance))
        {
            offset += Vector3.right;
        }
        else if (Physics.Raycast(right, transform.forward, out hit, detectionDistance))
        {
            offset -= Vector3.right;
        }
        if (Physics.Raycast(up, transform.forward, out hit, detectionDistance))
        {
            offset -= Vector3.up;
        }
        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance))
        {
            offset += Vector3.up;
        }

        if (offset != Vector3.zero)
        {
            transform.Rotate(offset * 5f * Time.deltaTime);
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
            Turn();
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
