using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform playerObj;
    [SerializeField] Vector3 offset;
    public float smoothSpeed = 0.125f;
    
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }

    private void LateUpdate() 
    {
        Vector3 desiredPos = playerObj.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothPos; 
    }
}
