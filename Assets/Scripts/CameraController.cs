using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform knife;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - knife.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = knife.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
