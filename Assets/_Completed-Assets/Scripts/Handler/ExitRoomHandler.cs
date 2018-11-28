using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExitRoomHandler : BaseHandler
{
    public event MessageEventHandler ExitRoomEvent;
    public override void Awake()
    {
        actionCode = ActionCode.MsgExitRoom;
        base.Awake();
    }

    public void ExitRoom()
    {
        Send();
    }

    public override void OnResponse(ProtocalData msg)
    {
        base.OnResponse(msg);
        if (ExitRoomEvent != null)
            ExitRoomEvent(this, msg);
    }
}
