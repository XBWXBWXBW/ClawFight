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

        public Player AddPlayer(Socket _socket) {
            maxID++;
            PlayerData pd = new PlayerData()
            {
                socket = _socket,
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
