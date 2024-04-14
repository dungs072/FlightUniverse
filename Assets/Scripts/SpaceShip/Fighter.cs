using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
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
            NetworkObjectPool.Singleton.GetNetworkObject(projectile.gameObject, pos.position, pos.rotation);
        }
        canFire = true;
    }
}
