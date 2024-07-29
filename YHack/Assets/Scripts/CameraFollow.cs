using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -10);
    [SerializeField] private float smoothTime = 0.25f;

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
