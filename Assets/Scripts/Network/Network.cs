using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System;

public enum NetworkReceive
{
    SendId,
    JoinLobyOk,
    SyncValue,
}

public enum NetworkSend
{
    JoinLoby,
    SyncValue,
}

public class Network

{
    public static Network Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Network();
            }
            return instance;
        }
    }

    private static Network instance;

    public event EventHandler OnReceive;

    Socket socket;
    SocketAsyncEventArgs asyncSocket;
    byte[] buffer = new byte[1024];
    string dataReceived = "";
    string lobyCode;

    public int Player;
    public bool Connected { get; private set; } = false;

    public bool Connect()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("82.165.126.16", 11000);
            //socket.Connect("127.0.0.1", 11000);
        }
        catch
        {
            Connected = false;
            return false;
        }
        asyncSocket = new SocketAsyncEventArgs();
        asyncSocket.Completed += AsyncSocket_Completed;
        asyncSocket.SetBuffer(buffer, 0, 1024);
        ReceiveAsync();
        Connected = true;
        return true;
    }

    private void ReceiveAsync()
    {
        if (!socket.ReceiveAsync(asyncSocket))
        {
            AsyncSocket_Completed(this, asyncSocket);
        }
    }

    private void AsyncSocket_Completed(object sender, SocketAsyncEventArgs e)
    {
        if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
        {
            dataReceived += Encoding.ASCII.GetString(buffer, e.Offset, e.BytesTransferred);
        }
        ReceiveAsync();
    }

    public void Send(NetworkSend type, string data)
    {
        if (!Connected)
        {
            return;
        }
        var buffer = Encoding.ASCII.GetBytes($"{(int)type} {data}|");
        socket.Send(buffer);
    }

    public void Update()
    {
        if (!Connected)
        {
            return;
        }
        if (dataReceived.Length > 0)
        {
            var dataSplit = dataReceived.Split('|');
            for (int i = 0; i < dataSplit.Length; i++)
            {
                ProcessData(dataSplit[i]);
            }
            dataReceived = "";
        }
    }

    private void ProcessData(string data)
    {
        var dataSplit = data.Split();
        if (dataSplit.Length == 0 || data.Length == 0)
        {
            return;
        }
        if (!int.TryParse(dataSplit[0], out int type))
        {
            return;
        }

        if (OnReceive != null)
        {
            OnReceive.Invoke(data, EventArgs.Empty);
        }
    }
}
