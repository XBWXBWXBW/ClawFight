using Google.Protobuf;
using message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClawFight
{
    class PlayerManager : ManagerBase<PlayerManager>
    {
        private Dictionary<int, Player> playerDict = new Dictionary<int, Player>();
        private Dictionary<int, Player> playerInRoom = new Dictionary<int, Player>();
        private int maxID = 0;

        public Dictionary<int, Player> GetPlayerInRoom() {
            return playerInRoom;
        }
        public override void Init()
        {
            base.Init();
            EventManager.instance.RegistProto(EMessageType.CSP_JoinRoom, OnOtherJoinRoom);
            EventManager.instance.RegistProto(EMessageType.CSP_JoinTeam, OnJoinTeam);
            EventManager.instance.RegistProto(EMessageType.CSP_ReadyToPlay, OnReadToPlay);
            EventManager.instance.RegistProto(EMessageType.CSP_MoveSync, OnMoveSync);
            EventManager.instance.RegistProto(EMessageType.CSP_BeginPlay, OnBeginPlay);

            EventManager.instance.RegistEventT<int>(AllEvents.PLAYER_JOIN_GAME, OnPlayerJoinGame);
            EventManager.instance.RegistEventT<int>(AllEvents.SYNC_INFO, OnSyncInfo);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            EventManager.instance.UnRegistProto(EMessageType.CSP_JoinRoom, OnOtherJoinRoom);
            EventManager.instance.UnRegistProto(EMessageType.CSP_JoinTeam, OnJoinTeam);
            EventManager.instance.UnRegistProto(EMessageType.CSP_ReadyToPlay, OnReadToPlay);
            EventManager.instance.UnRegistProto(EMessageType.CSP_MoveSync, OnMoveSync);
            EventManager.instance.UnRegistProto(EMessageType.CSP_BeginPlay, OnBeginPlay);

            EventManager.instance.UnRegistEventT<int>(AllEvents.PLAYER_JOIN_GAME, OnPlayerJoinGame);
            EventManager.instance.UnRegistEventT<int>(AllEvents.SYNC_INFO, OnSyncInfo);
        }

        private void OnBeginPlay(IMessage obj)
        {
            CSP_BeginPlay msg = obj as CSP_BeginPlay;
            int _id = msg.PlayerID;
            Player p = playerInRoom[_id];
            PlayerData pd = p.playerData;
            if (pd.isReady) pd.isLoadDone = true;
            bool _ready = true;
            foreach (var e in playerInRoom) {
                PlayerData epd = e.Value.playerData;
                if (epd.isReady && !epd.isLoadDone) {
                    _ready = false;
                    break;
                }
            }
            if (_ready) {
                SCP_BeginPlay _msg = new SCP_BeginPlay();
                foreach (var e in playerInRoom) {
                    Player ep = e.Value;
                    PlayerData epd = ep.playerData;
                    if (epd.isReady)
                    {

                        ConnectManager.instance.tcpConnect.SendMessage(ep, _msg, (int)EMessageType.SCP_BeginPlay);
                    }
                }
            }
        }

        private void OnMoveSync(IMessage obj)
        {
            CSP_MoveSync msgClient = obj as CSP_MoveSync;
            SCP_MoveSync msg = new SCP_MoveSync();
            PlayerMoveSyncInfo pmsi = new PlayerMoveSyncInfo() {
                PlayerID = msgClient.PlayerID,
                CurPos = msgClient.CurPos,
            };
            msg.AllSyncInfo = pmsi;
            foreach (var e in playerInRoom) {
                if (e.Value.playerData.isReady) {
                    ConnectManager.instance.tcpConnect.SendMessage(e.Value, msg, (int)EMessageType.SCP_MoveSync);
                }
            }
        }

        private void OnReadToPlay(IMessage obj)
        {
            CSP_ReadyToPlay msg = obj as CSP_ReadyToPlay;
            int _id = msg.PlayerID;
            if (GameManager.instance.CanEnterReady()) {
                if (playerDict.ContainsKey(_id)) {
                    Player p = playerDict[_id];
                    PlayerData pd = p.playerData;
                    pd.isReady = true;

                    SCP_ReadyToPlay _msg = new SCP_ReadyToPlay();
                    _msg.PlayerID = _id;
                    foreach (var e in playerDict) {
                        ConnectManager.instance.tcpConnect.SendMessage(e.Value, _msg, (int)EMessageType.SCP_ReadyToPlay);
                    }
                }
            }
            if (GameManager.instance.CanEnterPlay()) {
                GameManager.instance.PlayStart();
            }
        }
        public void PlayStart() {
            foreach (var e in playerDict) {
                Player p = e.Value;
                PlayerData pd = p.playerData;
                SCP_EnterPlay msg = new SCP_EnterPlay();
                msg.MapID = GameManager.instance.mapID;
                if (pd.isReady) {
                    ConnectManager.instance.tcpConnect.SendMessage(p, msg, (int)EMessageType.SCP_EnterPlay);
                }
            }
        }
        private void OnJoinTeam(IMessage obj)
        {
            CSP_JoinTeam msg = obj as CSP_JoinTeam;
            int _id = msg.PlayerID;
            ETeam team = (ETeam)msg.Team;
            if (playerDict.ContainsKey(_id)) {
                playerDict[_id].playerData.team = team;
            }

            SCP_JoinTeam _msg = new SCP_JoinTeam();
            _msg.PlayerID = _id;
            _msg.Team = msg.Team;
            foreach (var e in playerDict) {
                ConnectManager.instance.tcpConnect.SendMessage(e.Value, _msg, (int)EMessageType.SCP_JoinTeam);
            }
        }

        private void OnSyncInfo(int obj)
        {
            //只给刚登陆的player发送
            SyncInfo si = new SyncInfo();
            PlayerInfo pi = new PlayerInfo() {
                PlayerID = obj,
                Team = (int)playerDict[obj].playerData.team,
                IsReadyToPlay = playerDict[obj].playerData.isReady,
            };
            si.MainPlayerInfo = pi;

            Player cur = null;
            foreach (var e in playerDict) {
                if (e.Key != obj)
                {
                    PlayerInfo _other = new PlayerInfo()
                    {
                        PlayerID = e.Key,
                        Team = (int)playerDict[e.Key].playerData.team,
                        IsReadyToPlay = playerDict[e.Key].playerData.isReady,
                    };
                    if (playerInRoom.ContainsKey(e.Key))
                    {
                        _other.IsInRoom = true;
                    }
                    else {
                        _other.IsInRoom = false;
                    }
                    si.OtherPlayerInfo.Add(_other);
                }
                else {
                    cur = e.Value;
                }
            }
            ConnectManager.instance.tcpConnect.SendMessage(cur, si, (int)EMessageType.SyncInfo);
        }

        private void OnPlayerJoinGame(int pID)
        {
            //只给非pID的player发送，告诉pID进入game了
            SCP_JoinGame msg = new SCP_JoinGame();
            msg.PlayerID = pID;
            foreach (var e in playerDict) {
                if (e.Key != pID) {
                    ConnectManager.instance.tcpConnect.SendMessage(e.Value, msg, (int)EMessageType.SCP_JoinGame);
                }
            }
        }

        void OnOtherJoinRoom(IMessage _msg) {
            CSP_JoinRoom msg = _msg as CSP_JoinRoom;
            int _id = msg.PlayerID;

            if (playerDict.ContainsKey(_id) && !playerInRoom.ContainsKey(_id)) {
                playerInRoom.Add(_id, playerDict[_id]);
            }

            SCP_JoinRoom m = new SCP_JoinRoom();
            m.PlayerID = _id;
            foreach (var e in playerDict) {
                Player p = e.Value;
                ConnectManager.instance.tcpConnect.SendMessage(p, m, (int)EMessageType.SCP_JoinRoom);
            }
        }
        public Player AddPlayer() {
            maxID++;
            PlayerData pd = new PlayerData()
            {
                ID = maxID,
            };
            Player p = new Player()
            {
                playerData = pd,
            };
            playerDict.Add(maxID, p);
            return p;
        }
    }
}
