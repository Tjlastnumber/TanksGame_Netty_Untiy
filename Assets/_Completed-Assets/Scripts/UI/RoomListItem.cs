using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField]
    protected Text m_RoomName;
    [SerializeField]
    protected Text m_PlayerNumber;
    [SerializeField]
    protected Text m_RoomMaster;

    public void InitRoomItem(string roomName, List<string> players, string roomMaster)
    {
        m_RoomName.text = roomName;
        m_PlayerNumber.text = players != null ? players.Count.ToString() : "";
        m_RoomMaster.text = roomMaster;
    }

    public Action JoinRoomAction;
    public void OnClickJoinRoom()
    {
        JoinRoomAction();
    }
}
