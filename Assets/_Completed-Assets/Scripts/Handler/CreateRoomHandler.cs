using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CreateRoomHandler: BaseHandler
{
    public event Action<RoomData> CreateRoomCallback;
    public override void Awake()
    {
        actionCode = ActionCode.MsgCreateRoom;
        base.Awake();
    }

    public CreateRoomHandler CreateRoom(string name)
    {
        Send(new ProtocalData(actionCode, new RoomData(name)));
        return this;
    }

    public void Response(Action<RoomData> callback)
    {
        if (callback != null)
        {
            CreateRoomCallback = callback;
        }
    }

    public override void OnResponse(ProtocalData msg)
    {
        base.OnResponse(msg);
        RoomData room = ProtoBufSerializable.Decode<RoomData>(msg.Bytes);
        if (CreateRoomCallback != null)
            CreateRoomCallback(room);
    }
}
