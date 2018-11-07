using System;
using System.Collections;
using System.IO;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class ProtocalData: ProtocalBase
{
    ////协议名称
    [ProtoMember(1)]
    public string ProtocolName { set; get; }
    ////描述
    [ProtoMember(2)]
    public string ProtocolDesc { set; get; }
    //传输的字节流数据
    [ProtoMember(3)]
    public byte[] Bytes { set; get; }

    public ProtocalData() { }

    public ProtocalData(string protocolName, string protocolDesc, byte[] bytes)
    {
        ProtocolName = protocolName;
        ProtocolDesc = protocolDesc;
        Bytes = bytes;
    }

    public ProtocalData(string protocolName, string protocolDesc, object @object)
    {
        ProtocolName = protocolName;
        ProtocolDesc = protocolDesc;
        Bytes = ProtoBufSerializable.Encode(@object);
    }

    public ProtocalData(ActionCode code, object @object):this(code.ToString(), "", @object) { } 

    //协议名称
    public string GetName()
    {
        return ProtocolName;
    }

    //描述
    public string GetDesc()
    {
        return ProtocolDesc;
    }
}
