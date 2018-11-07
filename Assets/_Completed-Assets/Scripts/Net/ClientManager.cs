using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

class ClientManager : BaseManager
{
    private const string IP = "192.168.4.13";
    private const int PORT = 5678;

    private Socket clientScoket;
    private Message msg = new Message();

    public event MessageEventHandler ReceiveEvent;
    public event EventHandler ClientConnected;

    public ClientManager() { }

    public override void OnInit()
    {
        base.OnInit();
        clientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientScoket.Connect(IP, PORT);
            if (ClientConnected != null) ClientConnected(this, null);
            BeginReceive();
        }
        catch (Exception e)
        {
            Debug.Log("Net Error : " + e);
            throw;
        }
    }

    private void BeginReceive()
    {
        clientScoket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (clientScoket == null || !clientScoket.Connected) return;
            int dataLenght = clientScoket.EndReceive(ar);
            msg.DecodeMessage(dataLenght, ActionCallback);
            BeginReceive();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void ActionCallback(ProtocalData data)
    {
        if (ReceiveEvent != null)
            ReceiveEvent(this, data);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientScoket.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Socket Close Fail!" +e);
        }
    }

    internal void Send(ProtocalData e)
    {
        if (clientScoket != null && clientScoket.Connected)
        {
            clientScoket.Send(Message.EncodeMessage(e));
        }
    }
}
public delegate void MessageEventHandler(object sender, ProtocalData e);
     
