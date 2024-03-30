using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform planet;

    [SerializeField]
    private float height = 4f;

    [SerializeField]
    private float cameraBuffer = 30;

    private float cameraAngle = 0f;

    void Update()
    {
        Vector2 planetToPlayer = player.position - planet.position;
        float angle = Vector2.SignedAngle(Vector2.up, planetToPlayer);
        float deltaAngle = Mathf.DeltaAngle(cameraAngle, angle);

        if(deltaAngle > cameraBuffer) {
            cameraAngle = angle - cameraBuffer;
        }
        else if (deltaAngle < -cameraBuffer) {
            cameraAngle = angle + cameraBuffer;
        }

        Vector2 cameraShadow = (Quaternion.AngleAxis(cameraAngle, Vector3.forward) * new Vector3(0, height, 0)) + planet.position;
        transform.position = new Vector3(cameraShadow.x, cameraShadow.y, -10);
        transform.up = cameraShadow;
    }
}
