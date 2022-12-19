using InexperiencedDeveloper.Core;
using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
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

    public static Player LocalPlayer => GetPlayer(NetworkManager.Instance.Client.Id);
    public static bool IsLocalPlayer(ushort id) => id == LocalPlayer.Id;

    protected override void Awake()
    {
        base.Awake();
        s_PlayerPrefab = m_PlayerPrefab;
    }

    public void SpawnInitalPlayer(string username)
    {
        Player player = Instantiate(s_PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        player.name = $"{username} -- LOCAL PLAYER (WAITING FOR SERVER)";
        ushort id = NetworkManager.Instance.Client.Id;
        player.Init(id, username, true);
        s_Players.Add(id, player);
        player.RequestInit();
    }

    private static void InitializeLocalPlayer()
    {
        LocalPlayer.name = $"{LocalPlayer.Username} -- {LocalPlayer.Id} -- LOCAL";
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
