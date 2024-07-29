using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new(0, 0, -10);
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            player.position + offset, 
            ref currentVelocity, 
            smoothTime
            );
    }
}
