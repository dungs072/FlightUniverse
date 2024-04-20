using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AIShooterController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private List<Transform> spawnProjectilePos;

    [SerializeField] private List<SpaceShipController> players;

    [Header("Attributes")]
    [SerializeField] private float detectionDistance = 50f;
    [SerializeField] private float maxHorizontalAngel = 150f;
    [SerializeField] private float minHorizontalAngel = -150f;
    [SerializeField] private float maxVerticalAngel = 45;
    [SerializeField] private float minVerticalAngel = -45;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool isDown = false;
    private bool canShoot = true;
    private Transform target;
    private void Update()
    {
        if (target == null) { return; }
        if (IsInDistance(target.position))
        {
            RotateToTarget();
            Attack();
        }



    }
    private void RotateToTarget()
    {
        Vector3 directionToTarget = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        Vector3 targetEulerAngles = targetRotation.eulerAngles;
        float clampedXRotation = Mathf.Clamp(targetEulerAngles.x, 360f - maxVerticalAngel, 360 + maxVerticalAngel);
        float clampedYRotation = Mathf.Clamp(targetEulerAngles.y, minHorizontalAngel, maxHorizontalAngel);
        if (isDown)
        {
            print(targetRotation.eulerAngles);
            clampedXRotation = targetRotation.eulerAngles.x; //Mathf.Clamp(targetEulerAngles.x, 360 + minVerticalAngel, 360 + maxVerticalAngel);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(clampedXRotation, clampedYRotation, transform.rotation.eulerAngles.z), Time.deltaTime * rotationSpeed);
    }

    private void Attack()
    {
        if (!canShoot) { return; }
        canShoot = false;
        Invoke(nameof(Fire), fireRate);
    }
    private void Fire()
    {
        foreach (var pos in spawnProjectilePos)
        {
            var obj = NetworkObjectPool.Singleton.GetNetworkObject(projectilePrefab, pos.position, pos.rotation);
            obj.transform.forward = pos.forward;
            if (obj.TryGetComponent(out Projectile projectile))
            {
                projectile.SetProjectilePrefab(projectilePrefab);
            }

        }
        canShoot = true;
    }
    private bool IsInDistance(Vector3 targetPoint)
    {
        return (targetPoint - transform.position).sqrMagnitude <= detectionDistance * detectionDistance;
    }
    private void GetTarget()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }

    public void AddPlayers(SpaceShipController player)
    {
        if (players == null)
        {
            players = new List<SpaceShipController>();
        }
        players.Add(player);
        target = player.transform;
    }

}
