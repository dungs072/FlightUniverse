using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFollower : MonoBehaviour
{
    [SerializeField] private ObjectDisable hitVfx;
    [SerializeField] private float delayTimeToFollow = 2f;
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    private bool canFollow = false;
    private float currentTime = 0f;
    private Transform target;


    private void Update()
    {
        if (target == null) { return; }
        if (canFollow)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion quaternion = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, rotateSpeed * Time.deltaTime);
            transform.position += transform.forward * movingSpeed * Time.deltaTime;

        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime >= delayTimeToFollow)
            {
                canFollow = true;
            }
            else
            {
                transform.position += transform.forward * movingSpeed * Time.deltaTime;
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        var poolObj = NetworkObjectPool.Singleton.GetNetworkObject(hitVfx.gameObject, transform.position, transform.rotation);
        if (poolObj.TryGetComponent(out ObjectDisable vfx))
        {
            vfx.SetPrefab(hitVfx.gameObject);
        }
        Destroy(gameObject);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
