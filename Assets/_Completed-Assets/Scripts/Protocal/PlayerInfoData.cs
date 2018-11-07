using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ProtoContract]
public class PlayInfoData : ProtocalBase
{
    [ProtoMember(1)]
    public string UserId { get; set; }
    [ProtoMember(2)]
    public float Position1 { get; set; }
    [ProtoMember(3)]
    public float Position2 { get; set; }
    [ProtoMember(4)]
    public float Position3 { get; set; }
    [ProtoMember(5)]
    public float Rotation1 { get; set; }
    [ProtoMember(6)]
    public float Rotation2 { get; set; }
    [ProtoMember(7)]
    public float Rotation3 { get; set; }
    [ProtoMember(8)]
    public float Rotation4 { get; set; }
    [ProtoMember(9)]
    public bool Shoot { get; set; }
    [ProtoMember(10)]
    public float Force { get; set; }
    [ProtoMember(11)]
    public int ServerHealth { get; set; }
    [ProtoMember(12)]
    public string PlayName { get; set; }
    [ProtoMember(13)]
    public int PlayerNumber { get; set; }


    public PlayInfoData()
    {
    }

    public PlayInfoData(string userId, string name, Transform transform, bool shoot, int health, float force)
    {
        UserId = userId;
        PlayName = name;
        Shoot = shoot;
        ServerHealth = health;

        Position1 = transform.position.x;
        Position2 = transform.position.y;
        Position3 = transform.position.z;

        Rotation1 = transform.rotation.x;
        Rotation2 = transform.rotation.y;
        Rotation3 = transform.rotation.z;
        Rotation4 = transform.rotation.w;

        Force = force;
    }

    static Vector3 positionVector3 = new Vector3();
    public Vector3 GetPosition()
    {
        positionVector3.Set(Position1, Position2, Position3);
        return positionVector3;
    }

    static Quaternion rotationQuaternion = new Quaternion();

    public Quaternion GetRotation()
    {
        rotationQuaternion.Set(Rotation1, Rotation2, Rotation3, Rotation4);
        return rotationQuaternion;
    }
}