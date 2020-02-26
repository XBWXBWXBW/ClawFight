using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using message;

namespace ClawFight
{
    class TcpConnect
    {
        byte[] bytes = new byte[1024];
        public void Start() {
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("172.20.16.71"), 50001);
            tcpSocket.Bind(endPoint);
            tcpSocket.Listen(10);

            try
            {
                tcpSocket.BeginAccept(EndAccept, tcpSocket);
            }
            catch {
            }
        }
        void EndAccept(IAsyncResult ia) {
            try
            {
                Socket tcpSocket = ia.AsyncState as Socket;
                tcpSocket.BeginAccept(EndAccept, tcpSocket);

                PlayerManager.instance.AddPlayer(tcpSocket.EndAccept(ia));
            }
            catch {

            }
        }
    }
}
