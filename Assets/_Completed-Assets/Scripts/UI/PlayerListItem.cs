using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField]
    protected Text m_PlayerName;
    [SerializeField]
    protected Text m_IP;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitPlayerItem(string playerName, string ip)
    {
        m_PlayerName.text = playerName;
        m_IP.text = ip;
    }
}
