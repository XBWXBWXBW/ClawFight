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

        public override void Init()
        {
            base.Init();
            EventManager.instance.RegistProto(EMessageType.CSP_JoinRoom, OnOtherJoinRoom);
            EventManager.instance.RegistEventT<int>(AllEvents.PLAYER_JOIN_GAME, OnPlayerJoinGame);
            EventManager.instance.RegistEventT<int>(AllEvents.SYNC_INFO, OnSyncInfo);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            EventManager.instance.UnRegistProto(EMessageType.CSP_JoinRoom, OnOtherJoinRoom);
            EventManager.instance.UnRegistEventT<int>(AllEvents.PLAYER_JOIN_GAME, OnPlayerJoinGame);
            EventManager.instance.UnRegistEventT<int>(AllEvents.SYNC_INFO, OnSyncInfo);
        }

        private void OnSyncInfo(int obj)
        {
            //只给刚登陆的player发送
            SyncInfo si = new SyncInfo();
            PlayerInfo pi = new PlayerInfo();
            pi.PlayerID = obj;
            pi.Team = (int)playerDict[obj].playerData.team;
            si.MainPlayerInfo = pi;

            Player cur = null;
            foreach (var e in playerDict) {
                if (e.Key != obj)
                {
                    PlayerInfo _other = new PlayerInfo();
                    _other.PlayerID = e.Key;
                    _other.Team = (int)playerDict[e.Key].playerData.team;
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
