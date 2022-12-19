using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSceneUI : MonoBehaviour
{
    [SerializeField] private List<ButtonComponent> m_Buttons = new List<ButtonComponent>();
    [SerializeField] private List<InputComponent> m_Inputs = new List<InputComponent>();
    private Dictionary<string, UIComponent> m_Components = new Dictionary<string, UIComponent>();
    public Dictionary<string, UIComponent> Components => m_Components;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        UIManager.Instance.LocalUI = this;
        foreach (var btn in m_Buttons)
            m_Components.Add(btn.Key, btn);
        foreach(var input in m_Inputs)
            m_Components.Add(input.Key, input);
    }
}
