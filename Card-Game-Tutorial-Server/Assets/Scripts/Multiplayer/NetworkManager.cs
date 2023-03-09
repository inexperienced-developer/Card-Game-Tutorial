using Riptide;
using Riptide.Utils;
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
    [SerializeField] private NetworkSettingsSO m_netSettings;

    public Server Server { get; private set; }

    private void Awake()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    private void Start()
    {
        Server = new Server();
        Server.Start(m_netSettings.Port, m_netSettings.MaxPlayers);
        Server.ClientDisconnected += Disconnect;
        Subscribe();
    }


    private void OnDestroy()
    {
        Server.ClientDisconnected -= Disconnect;
        Unsubscribe();
    }


    private void Disconnect(object sender, ServerDisconnectedEventArgs e)
    {
        Destroy(PlayerManager.GetPlayer(e.Client.Id).gameObject);
    }

    private void Subscribe()
    {
        NetworkEvents.SendMessageToPlayer += SendToPlayer;
        NetworkEvents.SendMessageToAll += SendToAll;
    }

    private void Unsubscribe()
    {
        NetworkEvents.SendMessageToPlayer -= SendToPlayer;
        NetworkEvents.SendMessageToAll -= SendToAll;
    }

    private void SendToPlayer(Message msg, ushort id)
    {
        Server.Send(msg, id);
    }

    private void SendToAll(Message msg)
    {
        Server.SendToAll(msg);
    }

    private void FixedUpdate()
    {
        Server.Update();
    }
}
