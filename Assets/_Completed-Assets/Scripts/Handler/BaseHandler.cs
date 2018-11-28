using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{
    private MessageManager _mm;
    public MessageManager MessageManager 
    {
        get
        {
            if (_mm == null) _mm = MessageManager.Instance; 
            return _mm;
        }
    }

    public ActionCode actionCode;

    public virtual void Awake()
    {
        MessageManager.AddListener(actionCode, this);
    }

    public virtual void OnResponse(ProtocalData msg) { }

    public virtual void Send() {
        Send(new ProtocalData(actionCode, null));
    }
    public virtual void Send(ProtocalData msg)
    {
        MessageManager.Send(msg);
    }
}
