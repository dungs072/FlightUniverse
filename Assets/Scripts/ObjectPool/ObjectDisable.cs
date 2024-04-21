using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ObjectDisable : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1f;
    private GameObject prefab;
    private void OnEnable() {
        StartCoroutine(DestroyItSelf());
    }
    private IEnumerator DestroyItSelf()
    {
        yield return new WaitForSeconds(timeToDestroy);
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(),prefab);
    }
    public void SetPrefab(GameObject prefab)
    {
        this.prefab = prefab;
    }
}
