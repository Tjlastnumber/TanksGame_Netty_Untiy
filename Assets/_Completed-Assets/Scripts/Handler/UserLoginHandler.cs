using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UserLoginHandler : BaseHandler
{
    public event MessageEventHandler LoggedEvent;
    public override void Awake()
    {
        actionCode = ActionCode.MsgLogin;
        base.Awake();
    }

    public override void OnResponse(ProtocalData msg)
    {
        base.OnResponse(msg);
        UserLoginData data = ProtoBufSerializable.Decode<UserLoginData>(msg.Bytes);
        if (LoggedEvent != null)
            LoggedEvent(this, msg);
    }
}
