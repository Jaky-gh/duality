using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0f, 10f)]
    public float speed = 2f;

    private Vector3? currentDestination = null;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!Move())
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private bool Move()
    {
        if (currentDestination == null)
        {
            return false;
        }
        var direction = currentDestination.Value - transform.position;
        direction = direction.normalized * speed;
        RotateToward(currentDestination.Value);
        direction.y = rb.velocity.y;
        rb.velocity = direction;

        if (Vector3.Distance(transform.position, currentDestination.Value) < 0.01f)
        {
            currentDestination = null;
            return false;
        }
        return true;
    }

    private void RotateToward(Vector3 destination)
    {
        var totalAngle = Quaternion.FromToRotation(transform.forward, (destination - transform.position).normalized).eulerAngles.y;
        if (totalAngle > 180)
        {
            totalAngle -= 360;
        }
        if (totalAngle == 0)
        {
            return;
        }
        float angle = (totalAngle < 0 ? -1 : 1) * Time.deltaTime * 1000;
        if (Mathf.Abs(angle) > Mathf.Abs(totalAngle))
        {
            angle = totalAngle;
        }
        transform.Rotate(Vector3.up, angle);
    }

    public void MoveDirection(Vector3 direction)
    {
        MoveDestination(direction + gameObject.transform.position);
    }

    public void MoveDestination(Vector3 destination)
    {
        currentDestination = destination;
    }

    public void Stop()
    {
        currentDestination = null;
    }
}
