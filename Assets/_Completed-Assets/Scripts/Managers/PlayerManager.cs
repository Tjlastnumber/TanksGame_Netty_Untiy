using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerManager : BaseManager
{
    /*
     * 使用单例模式
     *
     */
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerManager();
            return _instance;
        }
    }

    private Color[] tankColors = new Color[] { Color.red, Color.blue, Color.yellow, Color.green, Color.grey };
    private UserLoginHandler userLogin;
    private PlayerInitHandler playerInitHandler;
    private TankBehaviorHanlder tankBehavior;
    private PlayerLogOutHandler playerLogOutHandler;
    private GameObject syncPlayerInfo;
    private Complete.GameManager gameManager;

    public UserLoginData LocalPlayer { get; set; }
    public Dictionary<string, GameObject> PlayerList = new Dictionary<string, GameObject>();

    private PlayerManager()
    {
        gameManager = Complete.GameManager.Instance;
    }

    public override void OnInit()
    {
        base.OnInit();
        syncPlayerInfo = new GameObject("SyncPlayerInfo");
    }

    public void CreateSyncPlayerInfo()
    {
        userLogin = syncPlayerInfo.AddComponent<UserLoginHandler>();
        userLogin.LoggedEvent += LoggedEvent;
        userLogin.Send(new ProtocalData(ActionCode.MsgLogin, new UserLoginData("123", "123")));

        playerLogOutHandler = syncPlayerInfo.AddComponent<PlayerLogOutHandler>();
        playerLogOutHandler.PlayerLogOutEvent += PlayerLogOutEvent;
    }

    /// <summary>
    /// 玩家退出事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlayerLogOutEvent(object sender, ProtocalData e)
    {
        PlayInfoData playInfo = ProtoBufSerializable.Decode<PlayInfoData>(e.Bytes);
        GameObject tank;
        if (PlayerList.TryGetValue(playInfo.UserId, out tank))
        {
            RemovePlayer(playInfo.UserId);
            gameManager.RemoveTank(playInfo.UserId);
            gameManager.m_PlayerList.RemoveItem(tank.name);
            UnityEngine.Object.DestroyObject(tank);
        }
    }

    /// <summary>
    /// 玩家登录后触发事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LoggedEvent(object sender, ProtocalData e)
    {
        playerInitHandler = syncPlayerInfo.AddComponent<PlayerInitHandler>();
        playerInitHandler.InitPlayerListEvent += InitPlayerListEvent;
        playerInitHandler.Send(new ProtocalData(ActionCode.MsgInitPlay, new PlayInfoData()));

        LocalPlayer = ProtoBufSerializable.Decode<UserLoginData>(e.Bytes);
    }

    /// <summary>
    /// 初始化玩家列表事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void InitPlayerListEvent(object sender, ProtocalData e)
    {
        tankBehavior = syncPlayerInfo.AddComponent<TankBehaviorHanlder>();
        List<PlayInfoData> datas = ProtoBufSerializable.Decode<List<PlayInfoData>>(e.Bytes);
        for (int i = 0; i < datas.Count; i++)
        {
            GameObject tank;
            var item = datas[i];
            Color color = tankColors[item.PlayerNumber];
            if (item.UserId == LocalPlayer.UserId)
            {
                tank = gameManager.AddTank(item.PlayerNumber, color, LocalPlayer.UserName, LocalPlayer.UserId, true);
                tankBehavior.SetLocalPlayer(tank, tank.transform);
            }
            else
            {
                tank = gameManager.AddTank(item.PlayerNumber, color, item.PlayName, item.UserId, item.GetPosition(), item.GetRotation());
            }

            PlayerList.Add(item.UserId, tank);
            gameManager.m_PlayerList.AddItem(tank.name, color);
        }
    }

    public void AddPlayer(PlayInfoData data)
    {
        Color color = tankColors[PlayerList.Count];
        GameObject tank = gameManager.AddTank(PlayerList.Count + 1, color, data.PlayName, data.UserId, data.GetPosition(), data.GetRotation());
        PlayerList.Add(data.UserId, tank);
        gameManager.m_PlayerList.AddItem(tank.name, color);
    }

    public void RemovePlayer(string userId)
    {
        lock (PlayerList)
        {
            PlayerList.Remove(userId);
        }
    }

    public void Shoot(float force)
    {
        tankBehavior.SendFire(force);
    }

    public void TakeDamage(float amount)
    {
        tankBehavior.SendHealth(amount);
    }

    public void SendPostion()
    {
    }

    /// <summary>
    /// 释放资源时发送玩家退出信息
    /// </summary>
    public override void OnDestroy()
    {
        playerLogOutHandler.Send(new ProtocalData());
        base.OnDestroy();
    }

}
