using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followTime = 0.2f;

    private Vector2 currVel;

    private void FixedUpdate()
    {
        Vector2 targetPos = Vector2.SmoothDamp(transform.position, followTarget.position, ref currVel, followTime);
        transform.position = (Vector3)targetPos + Vector3.forward * transform.position.z;
    }
}
