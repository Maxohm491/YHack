
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    [SerializeField] protected GameObject planet;
    [SerializeField] protected Rigidbody2D rb;
    protected float planetRadius = 6f;

    protected Vector2 velocity = new();

    // cartesianVector should have origin at the orbiter
    protected Vector2 ToLocal(Vector2 cartesianVector) {
        float angle = Vector2.SignedAngle(Vector2.up, transform.position - planet.transform.position);

        return Quaternion.AngleAxis(-angle, Vector3.forward) * cartesianVector;
    }

    protected void UpdatePosition() {
        // Vector2 planetToShip = transform.position - planet.transform.position;
        // Vector2 newPosition = Quaternion.AngleAxis(Mathf.Rad2Deg * (-velocity.x / planetToShip.magnitude), Vector3.forward) * planetToShip;

        // newPosition += velocity.y * planetToShip.normalized;

        // rb.MovePosition(newPosition);
        // transform.Rotate(0, 0, Mathf.Rad2Deg * (-velocity.x / planetToShip.magnitude), Space.Self);
        rb.MovePosition((Vector2) transform.position + velocity);
    }
}
