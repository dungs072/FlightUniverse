using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float forceSpeed=100f;
    [SerializeField] private float timeDestroyed = 5f;
    private Rigidbody rb;
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward*forceSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyProjectile());
    }
    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(timeDestroyed);
        //NetworkObjectPool.Singleton.ReturnNetworkObject(this,);
    }
}
