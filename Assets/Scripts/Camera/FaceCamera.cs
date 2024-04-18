using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 lookDirection = mainCamera.transform.position - transform.position;
            lookDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = targetRotation;
        }
        // else
        // {
        //     Debug.LogError("Main camera reference not set!");
        // }
    }
}
