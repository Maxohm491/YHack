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

    private float angle = 0f;

    void Update()
    {
        Vector2 planetToPlayer = player.position - planet.position;
        // transform.up = planetToPlayer;
        angle = Vector2.Angle(Vector2.up, planetToPlayer);

        Vector2 cameraShadow = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(0, height, 0);
        transform.position = new Vector3(cameraShadow.x, cameraShadow.y, -10);


        // Vector2 cameraShadow = (Vector2) planet.position + (planetToPlayer.normalized * height);

        // transform.position = new Vector3(cameraShadow.x, cameraShadow.y, -10);
    }
}
