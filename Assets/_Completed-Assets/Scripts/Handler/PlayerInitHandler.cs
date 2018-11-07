using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerInitHandler: BaseHandler
{
    public event MessageEventHandler InitPlayerListEvent;

    public override void Awake()
    {
        actionCode = ActionCode.MsgInitPlay;
        base.Awake();
    }

    public override void OnResponse(ProtocalData msg)
    {
        base.OnResponse(msg);
        if (InitPlayerListEvent != null)
            InitPlayerListEvent(this, msg);
    }
}
