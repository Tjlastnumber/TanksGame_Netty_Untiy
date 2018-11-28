using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    protected InputField roomNameInput;
    [SerializeField]
    protected GameObject createRoomPanel;
    [SerializeField]
    protected GameObject roomListPanel;

    private RoomListHandler roomListHandler;
    private CreateRoomHandler createRoomHandler;
    private JoinRoomHandler joinRoomHandler;
    private ExitRoomHandler exitRoomHandler;

    public void Start()
    {
        roomListHandler = roomListPanel.AddComponent<RoomListHandler>();
        createRoomHandler = createRoomPanel.AddComponent<CreateRoomHandler>();
        joinRoomHandler = gameObject.AddComponent<JoinRoomHandler>();
        exitRoomHandler = gameObject.AddComponent<ExitRoomHandler>();
    }

    public void OnClickCreateRoom()
    {
        Debug.Log("Create Room");
        if (string.IsNullOrEmpty(roomNameInput.text)) return;

        CreateRoom(roomNameInput.text);
    }

    public void CreateRoom(string roomName)
    {
        createRoomHandler.CreateRoom(roomName).Response(room =>
        {
            Debug.Log(room.Players.Count);
        });
    }

    public void OnClickRefreshRoomList()
    {
        Debug.Log("Refresh Room");        
        roomListHandler.GetRoomList().Response(rooms =>
        {
            Debug.Log(rooms.Count);
        });
    }

    private void OnDestroy()
    {
        exitRoomHandler.ExitRoom();
    }
}
