using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameRoundEndHandler : BaseHandler
{
    public override void Awake()
    {
        actionCode = ActionCode.MsgGameRoundEnd;
        base.Awake();
    }

    public override void OnResponse(ProtocalData msg)
    {
        base.OnResponse(msg);
    }
}
