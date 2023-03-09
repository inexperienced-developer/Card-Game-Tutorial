using Riptide;
using UnityEngine.Events;

public static class NetworkEvents
{
    public static event UnityAction<Message> SendMessageToAll;
    public static void Send(Message msg) => SendMessageToAll?.Invoke(msg);
    public static event UnityAction<Message, ushort> SendMessageToPlayer;
    public static void Send(Message msg, ushort id) => SendMessageToPlayer?.Invoke(msg, id);

}
