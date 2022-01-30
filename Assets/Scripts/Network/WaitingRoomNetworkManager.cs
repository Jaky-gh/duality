using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class WaitingRoomNetworkManager : MonoBehaviour
{
    public TMP_InputField InputField;
    public TMP_Text RoomCode;

    Network network;
    string lobyCode;

    void Start()
    {
        network = Network.Instance;
        network.Connect();
        if (!network.Connected)
        {
            RoomCode.text = "Can't reach server";
        }
        network.OnReceive += NetWork_OnReceive;
    }

    private void NetWork_OnReceive(object sender, System.EventArgs e)
    {
        var data = (string)sender;
        var dataSplit = data.Split();
        if (dataSplit.Length == 0 || data.Length == 0)
        {
            return;
        }
        if (!int.TryParse(dataSplit[0], out int type))
        {
            return;
        }
        switch ((NetworkReceive)type)
        {
            case NetworkReceive.SendId:
                lobyCode = dataSplit[1];
                RoomCode.text = "Room code: " + lobyCode;
                break;
            case NetworkReceive.JoinLobyOk:
                OnJoinLoby(dataSplit[1]);
                break;
        }
    }

    void Update()
    {
        network.Update();
    }

    public void OnClickJoin()
    {
        network.Send(NetworkSend.JoinLoby, InputField.text);
    }

    public void OnJoinLoby(string loby)
    {
        if (loby == lobyCode)
        {
            network.Player = 1;
        }
        else
        {
            network.Player = 2;
        }
        SceneManager.LoadScene("MainScene");
    }
}
