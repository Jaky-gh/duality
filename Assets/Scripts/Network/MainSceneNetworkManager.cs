using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneNetworkManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Network network;
    private GameObject secondPlayer;
    private SecondPlayerController secondPlayerController;
    private GameObject player;

    void Start()
    {
        network = Network.Instance;

        secondPlayer = !network.Connected || network.Player == 1 ? player2 : player1;
        secondPlayer.GetComponent<PlayerController>().enabled = false;
        secondPlayerController = secondPlayer.GetComponent<SecondPlayerController>();
        secondPlayerController.enabled = true;
        network.OnReceive += Network_OnReceive;

        player = secondPlayer == player1 ? player2 : player1;
        StartCoroutine(SyncPlayerPosition());
    }

    private void Update()
    {
        network.Update();
    }

    private void Network_OnReceive(object sender, System.EventArgs e)
    {
        var data = (string)sender;
        var dataSplit = data.Split();
        var type = (NetworkReceive)int.Parse(dataSplit[0]);
        if (type != NetworkReceive.SyncValue)
        {
            return;
        }
        switch (int.Parse(dataSplit[2]))
        {
            case 0:
                secondPlayerController.SetDirection(new Vector3(
                    float.Parse(dataSplit[3]),
                    float.Parse(dataSplit[4]),
                    float.Parse(dataSplit[5])));
                break;
            case 1:
                secondPlayer.transform.position = new Vector3(
                    float.Parse(dataSplit[3]),
                    float.Parse(dataSplit[4]),
                    float.Parse(dataSplit[5]));
                break;
        }
    }

    private IEnumerator SyncPlayerPosition()
    {
        while (true)
        {
            var x = player.transform.position.x.ToString("0.00");
            var y = player.transform.position.y.ToString("0.00");
            var z = player.transform.position.z.ToString("0.00");
            network.Send(NetworkSend.SyncValue, $"1 {x} {y} {z}");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
