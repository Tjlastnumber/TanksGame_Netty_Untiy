using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerLogOutHandler: BaseHandler
{
    public event MessageEventHandler PlayerLogOutEvent;

    public override void Awake()
    {
        actionCode = ActionCode.MsgOnLogOut;
        base.Awake();
    }

    public override void OnResponse(ProtocalData msg)
    {
        if (PlayerLogOutEvent != null)
            PlayerLogOutEvent(this, msg);
    }
}
