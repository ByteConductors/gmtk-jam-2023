using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 currentPoint;
    public float maxDistance;
    public float followSpeed;

    private void FixedUpdate()
    {
        if (((Vector2)transform.position - (Vector2)target.position).magnitude > maxDistance)
        {
            currentPoint = target.position + offset;
        } 
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentPoint, Time.deltaTime * followSpeed);
    }
}
