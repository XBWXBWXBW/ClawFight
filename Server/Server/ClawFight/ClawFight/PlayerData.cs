using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClawFight
{
    class PlayerData : EntityData
    {
        public ETeam team = ETeam.None;
        public bool isReady = false;
    }
}
