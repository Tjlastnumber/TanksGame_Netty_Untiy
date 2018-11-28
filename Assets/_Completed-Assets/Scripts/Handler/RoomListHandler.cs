using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RoomListHandler : BaseHandler
{
    public event Action<List<RoomData>> RoomListCallback;

    public override void Awake()
    {
        actionCode = ActionCode.MsgRoomList;
        base.Awake();
    }

    public RoomListHandler GetRoomList()
    {
        Send();
        return this;
    }

    public RoomListHandler Response(Action<List<RoomData>> callback)
    {
        if (callback != null)
            RoomListCallback = callback;
        return this;
    }

    public override void OnResponse(ProtocalData msg)
    {
        base.OnResponse(msg);
        if (RoomListCallback != null)
        {
            List<RoomData> rooms = ProtoBufSerializable.Decode<List<RoomData>>(msg.Bytes);
            RoomListCallback(rooms);
        }
    }
}
