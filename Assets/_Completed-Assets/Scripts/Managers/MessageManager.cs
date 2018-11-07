using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Complete;
using UnityEngine;

/// <summary>
/// 消息处理管理器
/// </summary>
public class MessageManager : BaseManager
{
    private static MessageManager _instance;
    public static MessageManager Instance
    {
        get
        {
            if (_instance == null) _instance = new MessageManager();
            return _instance;
        }
    }

    public List<ProtocalData> MessageList = new List<ProtocalData>();

    public Dictionary<ActionCode, BaseHandler> msgDict = new Dictionary<ActionCode, BaseHandler>();

    public event MessageEventHandler SendEvent;


    private MessageManager() { }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void Update()
    {
        for (int i = 0; i < 15; i++)
        {
            if (MessageList.Count > 0)
            {
                MessageHandler(MessageList[0]);
                Remove();
            }
        }
        base.Update();
    }

    public void Add(ProtocalData msg)
    {
        MessageList.Add(msg);
    }

    public void Remove()
    {
        lock (MessageList)
        {
            MessageList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="code">指令代码</param>
    /// <param name="handler">处理类</param>
    public MessageManager AddListener(ActionCode code, BaseHandler handler)
    {
        msgDict.Add(code, handler);
        return this;
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="code">指令代码</param>
    public void RemoveListener(ActionCode code)
    {
        msgDict.Remove(code);
    }

    /// <summary>
    /// 消息处理
    /// </summary>
    /// <param name="msg">消息</param>
    public void MessageHandler(ProtocalData msg)
    {
        if (msg == null) return;
        ActionCode code = msg.GetName().ToEnum<ActionCode>();
        BaseHandler baseHandler;
        if (msgDict.TryGetValue(code, out baseHandler))
        {
            baseHandler.OnResponse(msg);
        }
        else
        {
            Debug.LogWarning("没有对应的处理类: " + msg.GetName());
        }
    }

    public void Send(ProtocalData msg)
    {
        if (SendEvent != null)
        {
            SendEvent(this, msg);
        }
    }
}
