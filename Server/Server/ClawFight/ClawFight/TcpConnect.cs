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
        Dictionary<Player, Socket> player_socket_dict = new Dictionary<Player, Socket>(); 
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

                Socket pClientSocket = tcpSocket.EndAccept(ia);
                BeginReceive(pClientSocket);
                Player p = PlayerManager.instance.AddPlayer();
                player_socket_dict.Add(p, pClientSocket);
                p.Init();
            }
            catch {

            }
        }
        void BeginReceive(Socket pClientSocket)
        {
            try
            {
                pClientSocket.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, EndReceive, pClientSocket);
            }
            catch { }
        }
        void EndReceive(IAsyncResult ia)
        {
            try
            {
                Socket pClientSocket = ia.AsyncState as Socket;
                int _size = pClientSocket.EndReceive(ia);

                RawMessage rawMessage = new RawMessage();
                using (MemoryStream ms_raw = new MemoryStream ()) {
                    using (CodedInputStream cis_raw = new CodedInputStream(ms_raw)) {
                        ms_raw.Write(bytes, 0, _size);
                        ms_raw.Position = 0;
                        rawMessage.MergeFrom(cis_raw);
                    }
                }
                CodedInputStream cis_body = rawMessage.MessageBody.CreateCodedInput();
                IMessage messageBody = MessageCreater.CreateMessage((EMessageType)rawMessage.MessageType);
                messageBody.MergeFrom(cis_body);

                foreach (var e in player_socket_dict) {
                    if (e.Value == pClientSocket) {
                        Console.WriteLine("Receive : playerID " +e.Key.playerData.ID+"  messageType: "+ (EMessageType)rawMessage.MessageType);
                    }
                }

                BeginReceive(pClientSocket);
            }
            catch { }
        }

        public void SendMessage(Player player, IMessage message, int messageID)
        {
            Socket pClientSocket = player_socket_dict[player];
            if (pClientSocket == null) return;
            ByteString bs = null;
            using (MemoryStream ms_body = new MemoryStream())
            {
                using (CodedOutputStream cos_body = new CodedOutputStream(ms_body))
                {
                    message.WriteTo(cos_body);
                    cos_body.Flush();
                    ms_body.Position = 0;
                    bs = ByteString.FromStream(ms_body);
                }
            }

            RawMessage rawMessage = new RawMessage();
            rawMessage.MessageType = messageID;
            rawMessage.MessageBody = bs;

            using (MemoryStream ms_raw = new MemoryStream())
            {
                using (CodedOutputStream cos_raw = new CodedOutputStream(ms_raw))
                {
                    rawMessage.WriteTo(cos_raw);
                    cos_raw.Flush();
                    byte[] buffer = ms_raw.GetBuffer();
                    try
                    {
                        pClientSocket.BeginSend(buffer, 0, (int)ms_raw.Position, SocketFlags.None, EndSend, pClientSocket);
                    }
                    catch {

                    }
                }
            }
        }
        void EndSend(IAsyncResult ia)
        {
            Socket pClientSocket = ia.AsyncState as Socket;
            try
            {
                pClientSocket.EndSend(ia);
            }
            catch {

            }
        }
    }
}
