using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fighter : NetworkBehaviour
{
    [SerializeField] private InfoFighter infoFighter;
    [SerializeField] private PlayerSound playerSound;
    private bool canFire = true;
    public void Attack()
    {
        FireServerRpc();
        //FireLaser();
    }
    private void Fire()
    {
        foreach (var pos in infoFighter.SpawnProjectilePos)
        {
            var obj = NetworkObjectPool.Singleton.GetNetworkObject(infoFighter.ProjectilePrefab, 
                                                                    pos.position, pos.rotation);
            obj.transform.forward = pos.forward;
            if (obj.TryGetComponent(out Projectile projectile))
            {
                projectile.SetProjectilePrefab(infoFighter.ProjectilePrefab);
            }
            playerSound.PlayShootSound(infoFighter.RifleShootSound);
        }
        canFire = true;
    }
    private void FireLaser()
    {
        ToggleLaser(true);
        for(int i =0;i<infoFighter.LaserLines.Count;i++)
        {
            infoFighter.LaserLines[i].SetPosition(0,infoFighter.LaserLines[i].transform.localPosition);
            if(Physics.Raycast(infoFighter.LaserLines[i].transform.localPosition,
                                infoFighter.LaserLines[i].transform.forward,
                                out RaycastHit hit,infoFighter.LaserLength,infoFighter.ObstacleLayer))
            {   
                infoFighter.LaserLines[i].SetPosition(1,hit.point);
            }
            else
            {
                infoFighter.LaserLines[i].SetPosition(1,Vector3.forward*infoFighter.LaserLength);
            }
        }
    }
    public void ToggleLaser(bool state)
    {
        foreach(var laser in infoFighter.LaserLines)
        {
            laser.gameObject.SetActive(state);
        }
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
