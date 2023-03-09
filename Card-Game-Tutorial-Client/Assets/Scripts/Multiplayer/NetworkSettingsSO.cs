using UnityEngine;

[CreateAssetMenu(menuName = "Multiplayer/Network/Settings")]
public class NetworkSettingsSO : ScriptableObject
{
    [SerializeField] private ushort m_port = 7777;
    [SerializeField] private string m_ip = "127.0.0.1";

    [HideInInspector] public string LocalUsername;
    [HideInInspector] public ushort LocalId;
    public ushort Port => m_port;
    public string Ip => m_ip;
}
