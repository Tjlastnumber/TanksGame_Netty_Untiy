using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[ProtoContract]
public class RoomData : ProtocalBase
{
    [ProtoMember(1)]
    public string Id { get; set; }
    [ProtoMember(2)]
    public string Name { get; set; }
    [ProtoMember(3)]
    public string MasterId { get; set; }
    [ProtoMember(4)]
    public List<string> Players { get; set; }

    public RoomData() { }
    public RoomData(string name)
    {
        Name = name;
    }
}
