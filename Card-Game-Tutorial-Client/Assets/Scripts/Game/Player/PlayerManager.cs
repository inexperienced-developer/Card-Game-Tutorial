using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private NetworkSettingsSO m_netSettings;
    [SerializeField] private GameObject m_PlayerPrefab;
    private static GameObject s_PlayerPrefab;
    private static Dictionary<ushort, Player> s_Players = new Dictionary<ushort, Player>();
    public static Player GetPlayer(ushort id)
    {
        s_Players.TryGetValue(id, out Player player);
        return player;
    }
    public static bool RemovePlayer(ushort id)
    {
        if (s_Players.TryGetValue(id, out Player player))
        {
            s_Players.Remove(id);
            return true;
        }
        return false;
    }

    private static ushort s_localId = ushort.MaxValue;

    private void Awake()
    {
        s_PlayerPrefab = m_PlayerPrefab;
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        NetworkEvents.ConnectSuccess += SpawnInitalPlayer;
    }

    private void Unsubscribe()
    {
        NetworkEvents.ConnectSuccess -= SpawnInitalPlayer;
    }

    public void SpawnInitalPlayer(ushort id, string username)
    {
        Player player = Instantiate(s_PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        player.name = $"{username} -- LOCAL PLAYER (WAITING FOR SERVER)";
        player.Init(id, username, true);
        s_localId = id;
        s_Players.Add(id, player);
        player.RequestInit();
    }

    private static void InitializeLocalPlayer()
    {
        Player local = s_Players[s_localId];
        local.name = $"{local.Username} -- {local.Id} -- LOCAL";
    }

    #region Messages

    /* ================= MESSAGE Receiving ================*/
    [MessageHandler((ushort)ServerToClientMsg.ApproveLogin)]
    private static void ReceiveApproveLogin(Message msg)
    {
        bool approve = msg.GetBool();
        if (approve)
        {
            InitializeLocalPlayer();
        }
    }

    #endregion
}
