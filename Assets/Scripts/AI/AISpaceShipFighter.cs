using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceShipFighter : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnProjectilePos;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 0.1f;
    private bool canAttack = true;
    public void Attack()
    {
        if (!canAttack) { return; }
        canAttack = false;
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
        canAttack = true;
    }
}
