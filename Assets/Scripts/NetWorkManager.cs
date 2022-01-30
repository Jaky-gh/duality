using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NetWorkManager : MonoBehaviour
{
    public TMP_InputField InputField;
    public TMP_Text RoomCode;

    NetWork netWork;
    string lobyCode;

    void Start()
    {
        netWork = NetWork.Instance;
        if (!netWork.Connect())
        {
            RoomCode.text = "Can't reach server";
        }
        netWork.OnReceive += NetWork_OnReceive;
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
        switch ((NetWorkReceive)type)
        {
            case NetWorkReceive.SendId:
                lobyCode = dataSplit[1];
                RoomCode.text = "Room code: " + lobyCode;
                break;
            case NetWorkReceive.JoinLobyOk:
                OnJoinLoby(dataSplit[1]);
                break;
        }
    }

    void Update()
    {
        netWork.Update();
    }

    public void OnClickJoin()
    {
        netWork.Send(NetWorkSend.JoinLoby, InputField.text);
    }

    public void OnJoinLoby(string loby)
    {
        if (loby == lobyCode)
        {
            netWork.Player = 1;
        }
        else
        {
            netWork.Player = 2;
        }
        SceneManager.LoadScene("MainScene");
    }
}
