﻿using Google.Protobuf;
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
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            EventManager.instance.UnRegistProto(EMessageType.CSP_JoinRoom, OnOtherJoinRoom);
        }
        void OnOtherJoinRoom(IMessage _msg) {
            CSP_JoinRoom msg = _msg as CSP_JoinRoom;
            MessageCreater.CreateMessage
            EMessageType.SCP_JoinRoom;
            SCP_JoinRoom
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
