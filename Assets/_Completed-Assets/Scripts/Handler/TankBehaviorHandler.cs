using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TankBehaviorHanlder : BaseHandler
{
    [SerializeField]
    public Transform localTransform;
    public GameObject player;

    private UserLoginData user;
    private GameObject otherPlayer;
    private Vector3 pose;
    private Quaternion rota;
    private bool isSyncRemotePlayer = false;
    private Dictionary<GameObject, Vector3> syncPosList = new Dictionary<GameObject, Vector3>();

    public override void Awake()
    {
        actionCode = ActionCode.MsgPlayInfoData;

        base.Awake();
        user = PlayerManager.Instance.LocalPlayer;
    }

    private void Start()
    {
        //InvokeRepeating("SendLocalPlayer", 1f, 1f / 45);
    }

    public void Update()
    {

    }

    private void FixedUpdate()
    {
        SendLocalPlayer();
    }

    public TankBehaviorHanlder SetLocalPlayer(GameObject player, Transform tf)
    {
        localTransform = tf;
        this.player = player;
        return this;
    }

    private void SendLocalPlayer()
    {
        Complete.TankHealth th = this.player.GetComponent<Complete.TankHealth>();
        var health = th.GetCurrentHealth();
        PlayInfoData playInfo = new PlayInfoData(user.UserId, user.UserName, player.transform, false, (int)health, 15f);
        ProtocalData data = new ProtocalData(ActionCode.MsgPlayInfoData, playInfo);
        Send(data);
    }

    public void SendFire(float force)
    {
        Complete.TankHealth th = this.player.GetComponent<Complete.TankHealth>();
        var health = th.GetCurrentHealth();
        PlayInfoData playInfo = new PlayInfoData(user.UserId, user.UserName, localTransform, true, (int)health, force);
        ProtocalData data = new ProtocalData(ActionCode.MsgPlayInfoData, playInfo);
        Send(data);
    }

    public void SendHealth(float health)
    {
        PlayInfoData playInfo = new PlayInfoData(user.UserId, user.UserName, localTransform, false, (int)health, 15f);
        ProtocalData data = new ProtocalData(ActionCode.MsgPlayInfoData, playInfo);
        Send(data);
    }

    public override void OnResponse(ProtocalData msg)
    {
        PlayInfoData playInfo = ProtoBufSerializable.Decode<PlayInfoData>(msg.Bytes);
        if (PlayerManager.Instance.LocalPlayer.UserId == playInfo.UserId) return;
        GameObject tank;
        if (PlayerManager.Instance.PlayerList.TryGetValue(playInfo.UserId, out tank))
        {
            tank.GetComponent<Complete.TankMovement>().RemoteTransform(playInfo.GetPosition(), playInfo.GetRotation());
            if (playInfo.Shoot)
                tank.GetComponent<Complete.TankShooting>().Fire(playInfo.Force);
            tank.GetComponent<Complete.TankHealth>().SyncHealth(playInfo.ServerHealth);
        }
        else
        {
            PlayerManager.Instance.AddPlayer(playInfo);
        }
    }
}
