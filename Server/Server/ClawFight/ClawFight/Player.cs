using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using message;
namespace ClawFight
{
    class Player : Entity
    {
        public PlayerData playerData;

        public void Init() {
            SendPlayerInfo();
        }
        private void SendPlayerInfo() {
            SyncInfo si = new SyncInfo();
            PlayerInfo pi = new PlayerInfo();
            pi.PlayerID = playerData.ID;
            si.PlayerInfo = pi;
            ConnectManager.instance.tcpConnect.SendMessage(this, si, (int)EMessageType.SyncInfo);
        }
        
    }
}
