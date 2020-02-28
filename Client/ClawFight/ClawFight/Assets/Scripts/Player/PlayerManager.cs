using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using message;
using System;

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
        EventManager.instance.RegistProto(EMessageType.SCP_JoinGame, OnPlayerJoinGame);
        EventManager.instance.RegistProto(EMessageType.SCP_JoinTeam, OnPlayerJoinTeam);
    }
    public override void OnDestroy()
    {
        EventManager.instance.UnRegistProto(EMessageType.SyncInfo, OnSyncInfo);
        EventManager.instance.UnRegistProto(EMessageType.SCP_JoinRoom, OnPlayerJoinRoom);
        EventManager.instance.UnRegistProto(EMessageType.SCP_JoinGame, OnPlayerJoinGame);
        EventManager.instance.UnRegistProto(EMessageType.SCP_JoinTeam, OnPlayerJoinTeam);
    }

    private void OnPlayerJoinTeam(IMessage obj)
    {
        SCP_JoinTeam msg = obj as SCP_JoinTeam;
        int _id = msg.PlayerID;
        ETeam team = (ETeam)msg.Team;
        if (playerDict.ContainsKey(_id)) {
            playerDict[_id].playerData.eTeam = team;

            EventManager.instance.SendEvent(HallEvents.HALLEVENT_PLAYER_JOIN_TEAM);
        }
    }

    private void OnPlayerJoinGame(IMessage obj)
    {
        SCP_JoinGame msg = obj as SCP_JoinGame;
        if (mainPlayer.playerData.ID == msg.PlayerID) return;

        PlayerData pd = new PlayerData()
        {
            ID = msg.PlayerID,
            isMainPlayer = false,
        };
        Player p = new Player(pd);
        AddPlayer(p);
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
        playerInRoom_Dict.Clear();

        SyncInfo si = _m as SyncInfo;
        PlayerInfo mainPlayerInfo = si.MainPlayerInfo;
        PlayerData pd = new PlayerData()
        {
            ID = mainPlayerInfo.PlayerID,
            isMainPlayer = true,
            isInRoom = mainPlayerInfo.IsInRoom,
            eTeam = (ETeam)mainPlayerInfo.Team,
        };
        Player p = new Player(pd);
        AddPlayer(p);
        mainPlayer = p;
        if (pd.isInRoom) {
            playerInRoom_Dict.Add(pd.ID, p);
        }

        for (int i = 0; i < si.OtherPlayerInfo.Count; i++) {
            PlayerInfo _otherInfo = si.OtherPlayerInfo[i];
            PlayerData _otherData = new PlayerData()
            {
                ID = _otherInfo.PlayerID,
                isMainPlayer = false,
                isInRoom = _otherInfo.IsInRoom,
                eTeam = (ETeam)_otherInfo.Team,
            };
            Player _otherPlayer = new Player(_otherData);
            AddPlayer(_otherPlayer);

            if (_otherData.isInRoom) {
                playerInRoom_Dict.Add(_otherData.ID, _otherPlayer);
            }
        }


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
