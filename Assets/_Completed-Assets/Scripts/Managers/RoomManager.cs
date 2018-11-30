using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    protected InputField m_RoomNameInput;
    [SerializeField]
    protected GameObject m_CreateRoomPanel;
    [SerializeField]
    protected GameObject m_RoomListPanel;
    [SerializeField]
    protected GameObject m_RoomPanel;

    [SerializeField]
    protected GameObject m_PlayerListContainer;
    [SerializeField]
    protected GameObject m_PlayerListItemPrefab;
    [SerializeField]
    protected GameObject m_RoomListContainer;
    [SerializeField]
    protected GameObject m_RoomListItemPrefab;

    private RoomListHandler roomListHandler;
    private CreateRoomHandler createRoomHandler;
    private JoinRoomHandler joinRoomHandler;
    private ExitRoomHandler exitRoomHandler;

    public void Start()
    {
        createRoomHandler = m_CreateRoomPanel.AddComponent<CreateRoomHandler>();
        roomListHandler = m_RoomListPanel.AddComponent<RoomListHandler>();
        joinRoomHandler = m_RoomListPanel.AddComponent<JoinRoomHandler>();
        exitRoomHandler = m_RoomPanel.AddComponent<ExitRoomHandler>();

        joinRoomHandler.JoinRoomEvent += JoinRoomHandler_JoinRoomEvent;
        roomListHandler.OnRoomListEvent += RoomListHandler_OnRoomListEvent;
    }

    private List<GameObject> playerItemCache = new List<GameObject>();
    private void JoinRoomHandler_JoinRoomEvent(object sender, ProtocalData msg)
    {
        RoomData room = ProtoBufSerializable.Decode<RoomData>(msg.Bytes);
        RefreshPlayerItem(room);
    }

    private void RefreshPlayerItem(RoomData room)
    {
        playerItemCache.Clear();
        foreach (var player in room.Players)
        {
            var playerListItem = Instantiate(m_PlayerListItemPrefab);
            var pli = playerListItem.GetComponent<PlayerListItem>();

            playerListItem.transform.SetParent(m_PlayerListContainer.transform);
            playerListItem.transform.localScale = Vector3.one;
            pli.InitPlayerItem(player, "");
            playerItemCache.Add(playerListItem);
        }
    }

    public void CreateRoom(string roomName)
    {
        createRoomHandler.CreateRoom(roomName).Response(room => RefreshPlayerItem(room));
    }

    private List<GameObject> roomItemCache = new List<GameObject>();
    public void OnClickRefreshRoomList()
    {
        roomListHandler.GetRoomList().Response(rooms =>
        {
            RefreshRoomList(rooms);
        });
    }

    private void RefreshRoomList(List<RoomData> rooms)
    {
        Debug.Log("Refresh Room");
        foreach (var roomItem in roomItemCache)
        {
            Destroy(roomItem);
        }
        playerItemCache.Clear();
        foreach (var room in rooms)
        {
            if (room == null) continue;

            var roomListItem = Instantiate(m_RoomListItemPrefab);
            var rli = roomListItem.GetComponent<RoomListItem>();

            roomListItem.transform.SetParent(m_RoomListContainer.transform);
            roomListItem.transform.localScale = Vector3.one;

            rli.InitRoomItem(room.Name, room.Players, room.MasterName);
            rli.JoinRoomAction = () => OnClickJoinRoom(room.Id);
            roomItemCache.Add(roomListItem);
        }
    }

    private void RoomListHandler_OnRoomListEvent(List<RoomData> rooms)
    {
        RefreshRoomList(rooms);
    }

    public void OnClickJoinRoom(string roomId)
    {
        MenuUIManager.Instance.OnClickRoomPanel();
        joinRoomHandler.JoinRoom(roomId);
    }

    public void OnClickExitRoom()
    {
        exitRoomHandler.ExitRoom();
        foreach (var playerItem in playerItemCache)
        {
            Destroy(playerItem);
        }
    }

    public void OnClickCreateRoom()
    {
        Debug.Log("Create Room");
        if (string.IsNullOrEmpty(m_RoomNameInput.text)) return;

        CreateRoom(m_RoomNameInput.text);
    }
    private void OnDestroy()
    {
        exitRoomHandler.ExitRoom();
    }
}
