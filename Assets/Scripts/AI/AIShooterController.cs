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
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    private bool canShoot = true;
    private Transform target;
    private void Update()
    {
        if (target == null) { return; }
        if (IsInDistance(target.position))
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (!Physics.Raycast(transform.position, directionToTarget , out RaycastHit hit, detectionDistance, obstacleLayer))
            {
                RotateToTarget();
                Attack();
            }

        }



    }
    private void RotateToTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToTarget), rotationSpeed * Time.deltaTime);
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
