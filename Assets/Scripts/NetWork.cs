using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System;

public enum NetWorkReceive
{
    SendId,
    JoinLobyOk,
    SyncValue,
}

public enum NetWorkSend
{
    JoinLoby,
    SyncValue,
}

public class NetWork
{
    public static NetWork Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetWork();
            }
            return instance;
        }
    }

    private static NetWork instance;

    public event EventHandler OnReceive;

    Socket socket;
    SocketAsyncEventArgs asyncSocket;
    byte[] buffer = new byte[1024];
    string dataReceived = "";
    string lobyCode;

    public int Player;

    public bool Connect()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("82.165.126.16", 11000);
        }
        catch
        {
            return false;
        }
        asyncSocket = new SocketAsyncEventArgs();
        asyncSocket.Completed += AsyncSocket_Completed;
        asyncSocket.SetBuffer(buffer, 0, 1024);
        ReceiveAsync();
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

    public void Send(NetWorkSend type, string data)
    {
        var buffer = Encoding.ASCII.GetBytes($"{(int)type} {data}|");
        socket.Send(buffer);
    }

    public void Update()
    {
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
