using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClawFight
{
    class ConnectManager : ManagerBase<ConnectManager>
    {
        public TcpConnect tcpConnect;
        public void Start() {
            tcpConnect = new TcpConnect();
            tcpConnect.Start();
        }
    }
}
