using InexperiencedDeveloper.Core;
using Riptide;
using Riptide.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ServerToClientMsg : ushort
{
    ApproveLogin,
}

public enum ClientToServerMsg : ushort
{
    RequestLogin,
}

public class NetworkManager : MonoBehaviour
{
    protected void Awake()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    [SerializeField] private NetworkSettingsSO m_netSettings;
    public Client Client {get; private set;}

    private void Start()
    {
        Client = new Client();
        Client.Connected += OnClientConnected;
        Subscribe();
    }

    private void Subscribe()
    {
        NetworkEvents.ConnectRequest += Connect;
        NetworkEvents.SendMessage += OnSendMessage;
    }

    private void Unsubscribe()
    {
        NetworkEvents.ConnectRequest -= Connect;
        NetworkEvents.SendMessage -= OnSendMessage;
    }

    private void OnSendMessage(Message msg)
    {
        Client.Send(msg);
    }

    private void OnClientConnected(object sender, EventArgs e)
    {
        NetworkEvents.OnConnectSuccess(Client.Id, m_netSettings.LocalUsername);
        m_netSettings.LocalId = Client.Id;
        //PlayerManager.Instance.SpawnInitalPlayer(s_LocalUsername);

    }

    public void Connect(string username, string password)
    {
        m_netSettings.LocalUsername = string.IsNullOrEmpty(username) ? $"Guest" : username;
        //TODO: Send Password and validate
        Client.Connect($"{m_netSettings.Ip}:{m_netSettings.Port}");
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    protected void OnDestroy()
    {
        Unsubscribe();
        Client.Connected -= OnClientConnected;
    }
}
