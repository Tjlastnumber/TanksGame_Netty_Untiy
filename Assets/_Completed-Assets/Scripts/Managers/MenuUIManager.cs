using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{

    public CanvasGroup m_DefaultPanel;
    public CanvasGroup m_LoadPanel;
    public CanvasGroup m_RoomPanel;
    public CanvasGroup m_RoomListPanel;
    public CanvasGroup m_CreateRoomPanel;

    private CanvasGroup m_CurrentPanel;

    private static MenuUIManager _instance;
    public static MenuUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("UIManager").GetComponent<MenuUIManager>();
            }
            return _instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        m_DefaultPanel.gameObject.SetActive(true);
        m_RoomPanel.gameObject.SetActive(false);
        m_RoomListPanel.gameObject.SetActive(false);
        m_CreateRoomPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ShowPanel(CanvasGroup panel)
    {
        if (m_CurrentPanel != null) m_CurrentPanel.gameObject.SetActive(false);
        m_CurrentPanel = panel;
        if (m_CurrentPanel != null) m_CurrentPanel.gameObject.SetActive(true);
    }

    private void ShowMainPanel()
    {
        ShowPanel(m_DefaultPanel);
    }

    private void ShowCreateRoomPanel()
    {
        ShowPanel(m_CreateRoomPanel);
    }

    private void ShowRoomListPanel()
    {
        ShowPanel(m_RoomListPanel);
    }
    private void ShowRoomPanel()
    {
        ShowPanel(m_RoomPanel);
    }

    private void ShowLoading()
    {
        ShowPanel(m_LoadPanel);
    }

    public void OnClickCreateRoomPanel()
    {
        ShowCreateRoomPanel();
    }

    public void OnClickRoomListPanel()
    {
        ShowRoomListPanel();
    }

    public void OnClickRoomPanel()
    {
        ShowRoomPanel();
    }


    public void OnClickBackMainPanel()
    {
        ShowMainPanel();
    }
}
