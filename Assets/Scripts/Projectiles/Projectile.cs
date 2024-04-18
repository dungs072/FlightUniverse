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
        StartCoroutine(DestroyProjectile());

    }
    private void OnDisable()
    {
        trailRenderer.Clear();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }
    private void OnTriggerEnter(Collider other) {
    }
    private void OnCollisionEnter(Collision other) {
        var poolObj = NetworkObjectPool.Singleton.GetNetworkObject(hitVFX,transform.position,transform.rotation);
        if(poolObj.TryGetComponent(out VFXControls vFXControls))
        {
            vFXControls.SetVFXPrefab(poolObj.gameObject);
        }
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), projectilePrefab);
    }
    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(timeDestroyed);
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), projectilePrefab);
    }
    public void SetProjectilePrefab(GameObject prefab)
    {
        projectilePrefab = prefab;
    }
}
