using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fighter : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private List<Transform> spawnProjectilePos;
    [SerializeField] private float fireRate = 0.3f;
    private bool canFire = true;
    public void Attack()
    {   
        if(!canFire){return;}
        canFire = false;
        Invoke(nameof(Fire),fireRate);
    }
    private void Fire()
    {
        foreach (var pos in spawnProjectilePos)
        {
            var obj = NetworkObjectPool.Singleton.GetNetworkObject(projectilePrefab, pos.position, pos.rotation);
            obj.transform.forward = pos.forward;
            if(obj.TryGetComponent(out Projectile projectile))
            {
                projectile.SetProjectilePrefab(projectilePrefab);
            }
            
        }
        canFire = true;
    }
    [ServerRpc]
    private void FireServerRpc()
    {
       
    }
}
