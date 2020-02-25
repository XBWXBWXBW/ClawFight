using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClawFight
{
    class TcpConnect
    {
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
                Socket client = tcpSocket.EndAccept(ia);
                Console.WriteLine("XBW~~  "+client.RemoteEndPoint.ToString());
            }
            catch {

            }
        }
    }
}
