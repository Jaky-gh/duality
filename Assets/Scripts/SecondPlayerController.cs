using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayerController : MonoBehaviour
{
    Player player;
    Network network;

    Vector3 direction = Vector3.zero;

    private void Start()
    {
        player = GetComponent<Player>();
        network = Network.Instance;
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

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    Vector3 getDirection()
    {
        if (!network.Connected)
        {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                dir.z += 1;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                dir.x -= 1;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                dir.z -= 1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                dir.x += 1;
            }
            return dir;
        }
        return direction;
    }
}
