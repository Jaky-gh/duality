using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneNetworkManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Network network;

    void Start()
    {
        network = Network.Instance;

        var secondPlayer = !network.Connected || network.Player == 1 ? player2 : player1;
        secondPlayer.GetComponent<PlayerController>().enabled = false;
        secondPlayer.GetComponent<SecondPlayerController>().enabled = true;
    }

    void Update()
    {

    }
}
