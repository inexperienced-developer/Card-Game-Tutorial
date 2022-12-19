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

    protected override void Awake()
    {
        base.Awake();
        s_PlayerPrefab = m_PlayerPrefab;
    }

    private static void SpawnPlayer(ushort id, string username)
    {
        Player player = Instantiate(s_PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        player.name = $"{username} -- {id}";
        player.Init(id, username);
        s_Players.Add(id, player);
        bool shouldApprove = true; //Could be validation from DB or something
        player.ApproveLogin(shouldApprove);
    }

    #region Messages

    /* ================= MESSAGE RECEIVING ================*/
    [MessageHandler((ushort)ClientToServerMsg.RequestLogin)]
    private static void ReceiveLoginRequest(ushort fromId, Message msg)
    {
        string username = msg.GetString();
        SpawnPlayer(fromId, username);
    }

    #endregion
}
