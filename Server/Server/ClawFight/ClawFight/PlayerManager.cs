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
        private int maxID = 0;

        public override void Init()
        {
            base.Init();
            EventManager.instance.RegistProto(EMessageType.CSP_JoinRoom, OnOtherJoinRoom);
            EventManager.instance.RegistEventT<int>(AllEvents.PLAYER_JOIN_GAME, OnPlayerJoinGame);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            EventManager.instance.UnRegistProto(EMessageType.CSP_JoinRoom, OnOtherJoinRoom);
            EventManager.instance.UnRegistEventT<int>(AllEvents.PLAYER_JOIN_GAME, OnPlayerJoinGame);
        }

        private void OnPlayerJoinGame(int pID)
        {
            SCP_JoinGame msg = new SCP_JoinGame();
            msg.PlayerID = pID;
            foreach (var e in playerDict) {

                ConnectManager.instance.tcpConnect.SendMessage(e.Value, msg, (int)EMessageType.SCP_JoinGame);
            }
        }

        void OnOtherJoinRoom(IMessage _msg) {
            CSP_JoinRoom msg = _msg as CSP_JoinRoom;
            int _id = msg.PlayerID;
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
