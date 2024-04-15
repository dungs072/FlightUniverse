using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VFXControls : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1f;
    private GameObject VFXPrefab;
    private void OnEnable() {
        StartCoroutine(DestroyVfx());
    }
    private IEnumerator DestroyVfx()
    {
        yield return new WaitForSeconds(timeToDestroy);
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(),VFXPrefab);
    }
    public void SetVFXPrefab(GameObject VFXPrefab)
    {
        this.VFXPrefab = VFXPrefab;
    }
}
