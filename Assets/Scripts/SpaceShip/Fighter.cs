using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fighter : NetworkBehaviour
{
    [SerializeField] private InfoFighter infoFighter;
    private bool canFire = true;
    public void Attack()
    {
        FireServerRpc();
    }
    private void Fire()
    {
        foreach (var pos in infoFighter.SpawnProjectilePos)
        {
            var obj = NetworkObjectPool.Singleton.GetNetworkObject(infoFighter.ProjectilePrefab, pos.position, pos.rotation);
            obj.transform.forward = pos.forward;
            if (obj.TryGetComponent(out Projectile projectile))
            {
                projectile.SetProjectilePrefab(infoFighter.ProjectilePrefab);
            }

        }
        canFire = true;
    }
    public void SetInfoFighter(InfoFighter infoFighter)
    {
        this.infoFighter = infoFighter;
    }
    [ServerRpc]
    private void FireServerRpc()
    {
        FireClientRpc();
    }
    [ClientRpc]
    private void FireClientRpc()
    {
        if (!canFire) { return; }
        canFire = false;
        Invoke(nameof(Fire), infoFighter.FireRate);
    }
}
