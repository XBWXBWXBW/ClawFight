using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using message;

public class PlayerManager : ManagerBase<PlayerManager>
{
    public Player mainPlayer;
    public Dictionary<int, Player> playerDict = new Dictionary<int, Player>();
    public Dictionary<int, Player> playerInRoom_Dict = new Dictionary<int, Player>();
    public override void Init()
    {
        GameManager.instance.AddManager(this);
        EventManager.instance.RegistProto(EMessageType.SyncInfo, OnSyncInfo);

        EventManager.instance.RegistProto(EMessageType.SCP_JoinRoom, OnPlayerJoinRoom);
    }
    public override void OnDestroy()
    {
        EventManager.instance.UnRegistProto(EMessageType.SyncInfo, OnSyncInfo);
        EventManager.instance.UnRegistProto(EMessageType.SCP_JoinRoom, OnPlayerJoinRoom);
    }
    private void OnPlayerJoinRoom(IMessage obj)
    {
        SCP_JoinRoom msg = obj as SCP_JoinRoom;
        int _id = msg.PlayerID;
        if (!playerInRoom_Dict.ContainsKey(_id) && playerDict.ContainsKey(_id)) {
            playerInRoom_Dict.Add(_id, playerDict[_id]);
        }
        EventManager.instance.SendEvent(HallEvents.HALLEVENT_OTHER_PLAYER_IN_ROOM);
    }
    void OnSyncInfo(IMessage _m) {
        SyncInfo si = _m as SyncInfo;
        PlayerInfo pi = si.PlayerInfo;
        PlayerData pd = new PlayerData()
        {
            ID = pi.PlayerID,
        };
        Player p = new Player(pd);
        AddPlayer(p);
        mainPlayer = p;
        EventManager.instance.SendEvent(HallEvents.HALLEVENT_SYNCINFO);
    }
    public void AddPlayer(Player p) {
        int pID = p.playerData.ID;
        if (playerDict.ContainsKey(pID)) {
            playerDict.Remove(pID);
        }
        playerDict.Add(pID, p);
    }
}
