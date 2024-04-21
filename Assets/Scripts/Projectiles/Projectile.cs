using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float forceSpeed = 100f;
    [SerializeField] private float timeDestroyed = 5f;
    [SerializeField] private GameObject hitVFX;
    private GameObject projectilePrefab;
    private Rigidbody rb;
    private Transform shooter;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }
    private void OnEnable()
    {
        //if(!IsOwner){return;}
        StartCoroutine(InitializeProjectile());
    }
    private IEnumerator InitializeProjectile()
    {
        yield return null;
        rb.AddForce(transform.forward * forceSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(timeDestroyed);
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), projectilePrefab);

    }
    private void OnDisable()
    {
        trailRenderer.Clear();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }
    private void OnCollisionEnter(Collision other)
    {
        StopAllCoroutines();
        if(!isActiveAndEnabled){return;}
        var poolObj = NetworkObjectPool.Singleton.GetNetworkObject(hitVFX, transform.position, transform.rotation);
        if (poolObj.TryGetComponent(out ObjectDisable vfx))
        {
            vfx.SetPrefab(hitVFX);
        }
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), projectilePrefab);
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        

    }
    public void SetProjectilePrefab(GameObject prefab)
    {
        projectilePrefab = prefab;
    }
    public void SetShooter(Transform shooter)
    {
        this.shooter = shooter;
    }
}
