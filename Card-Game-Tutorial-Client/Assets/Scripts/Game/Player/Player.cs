using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ushort Id { get; private set; }
    public string Username { get; private set; }
    public bool IsLocal { get; private set; }

    public void Init(ushort id, string username, bool isLocal)
    {
        Id = id;
        Username = username;
        IsLocal = isLocal;
    }

    private void OnDestroy()
    {
        PlayerManager.RemovePlayer(Id);
    }

    #region Messages

    /* ================= MESSAGE SENDING ================*/
    public void RequestInit()
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ClientToServerMsg.RequestLogin);
        msg.AddString(Username);
        NetworkManager.Instance.Client.Send(msg);
    }

    #endregion
}
