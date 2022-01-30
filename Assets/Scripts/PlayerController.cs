using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        var direction = getDirection();
        if (direction.magnitude < 0.5)
        {
            player.Stop();
        }
        else
        {
            player.MoveDirection(direction.normalized);
        }
    }

    Vector3 getDirection()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
        {
            dir.z += 1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            dir.x -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.z -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x += 1;
        }
        return dir;
    }
}
