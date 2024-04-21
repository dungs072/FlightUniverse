using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShooter : MonoBehaviour
{
    [SerializeField] private List<Transform> rocketPos;
    [SerializeField] private RocketFollower rocketFollowerPrefab;
    [SerializeField] private float detectionDistance = 100f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private float rocketPerShootTime = 1f;
    
    private Transform target = null;
    private float currentTime = 0f;
    private void Start() {
        currentTime = fireRate;
    }
    private void Update() {
        if (target == null){return;}
        if(IsInDistance(target.position))
        {
            currentTime+=Time.deltaTime;
            if(currentTime > fireRate)
            {
                currentTime = 0f;
                StartCoroutine(ShootRocket());
            }
        }
    }
    private IEnumerator ShootRocket()
    {
        foreach (Transform t in rocketPos)
        {
            yield return new WaitForSeconds(rocketPerShootTime);
            var rocketInstance = Instantiate(rocketFollowerPrefab,t.position,t.rotation);
            rocketInstance.SetTarget(target);
        }
    }

    private bool IsInDistance(Vector3 targetPos)
    {
        return (targetPos-transform.position).sqrMagnitude <= detectionDistance*detectionDistance;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,detectionDistance);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
