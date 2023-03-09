using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_Username;
    [SerializeField] private TMP_InputField m_Password;
    [SerializeField] private Button m_LoginButton;

    public void Connect()
    {
        string username = m_Username.text;
        string password = m_Password.text;
        NetworkEvents.OnConnectRequest(username, password);
    }
}
