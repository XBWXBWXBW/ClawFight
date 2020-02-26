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

        private Socket socket = null;
        private byte[] bytes = new byte[1024];
        public void Init() {
            socket = playerData.socket;

            BeginReceive();
            SendPlayerInfo();
        }
        private void SendPlayerInfo() {
            PlayerInfo pi = new PlayerInfo();
            pi.PlayerID = playerData.ID;
            SendMessage(pi, (int)EMessageType.PlayerInfo);
        }
        public void SendMessage(IMessage message,int messageID) {
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

            using (MemoryStream ms_raw = new MemoryStream()) {
                using (CodedOutputStream cos_raw = new CodedOutputStream(ms_raw)) {
                    rawMessage.WriteTo(cos_raw);
                    cos_raw.Flush();
                    byte[] buffer = ms_raw.GetBuffer();
                    socket.BeginSend(buffer, 0, (int)ms_raw.Position, SocketFlags.None, EndSend, null);
                }
            }
        }
        void EndSend(IAsyncResult ia) {
            socket.EndSend(ia);
        }
        void BeginReceive() {
            socket.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, EndReceive, null);
        }
        void EndReceive(IAsyncResult ia) {
            int _size = socket.EndReceive(ia);

        }
    }
}
