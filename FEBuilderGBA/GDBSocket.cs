using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;  


namespace FEBuilderGBA
{
    //作ったけど使わないかも...
    class GDBSocket : IDisposable
    {
        TcpClient TCPSocket;
        string ErrorMessage = "";

        public void Dispose()
        {
            DisCoonect();
        }

        public void Connect(string server, int defport = 55555)
        {
            DisCoonect();

            string[] sp = server.Split(':');
            string ip = sp[0];
            int port = (int)U.atoi(U.at(sp, 1, defport.ToString()));

            this.TCPSocket = new System.Net.Sockets.TcpClient(ip, port);
        }

        public void DisCoonect()
        {
            if ( this.TCPSocket != null)
            {
                this.TCPSocket.Close();
                this.TCPSocket = null;
            }
        }

        public string SendAndRecv(string order)
        {
            System.Net.Sockets.NetworkStream ns = this.TCPSocket.GetStream();
            ns.ReadTimeout = 5000;
            ns.WriteTimeout = 5000;

            byte[] str = MakePacket(order);

            ns.Write(str,0,str.Length);

            byte[] resBytes = new byte[this.TCPSocket.ReceiveBufferSize];
            int resSize = ns.Read(resBytes, 0, resBytes.Length);
            if (resSize <= 5)
            {
                if (resSize <= 0)
                {
                    this.ErrorMessage = "";
                    return "";
                }
                if (resBytes[0] == '-')
                {
                    this.ErrorMessage = R._("メッセージを送信できません。エラーコードが戻りました。\r\n送信データ:\r\n{0}", str.ToString());
                    return "";
                }
                this.ErrorMessage = R._("サーバがおかしな応答を返しました。\r\n送信データ:\r\n{0}\r\n受信データ:\r\n{1}", str.ToString(), U.subrange(resBytes,0, (uint)resSize).ToString() );
                return "";
            }
//            return Encoding.ASCII.GetString(resBytes, 0, resSize);
            return UnPacket(resBytes, resSize);
        }

        string UnPacket(byte[] packet, int length)
        {
            if (length <= 5)
            {
                this.ErrorMessage = R._("パケットの長さが5バイト以下です。") + "\r\npacket:" + U.HexDump(packet);
                return "";
            }
            int i = 0;
            if (packet[i] == '+')
            {
                i++;
            }
            if (packet[i] != '$')
            {
                this.ErrorMessage = R._("パケットの先頭が$ではありません。") + "\r\npacket:" + U.HexDump(packet);
                return "";
            }
            i++;
            int header = i;
            if (packet[length - 3] != '#')
            {
                this.ErrorMessage = R._("パケットの終端が#ではありません。") + "\r\npacket:" + U.HexDump(packet);
                return "";
            }
            int size = length - header - 3;
            return Encoding.ASCII.GetString(packet, header, size);
        }
        byte[] MakePacket(string order)
        {
            byte[] str = Encoding.ASCII.GetBytes(order);
            byte[] sendBuffer = new byte[1 + str.Length + 1 + 2];

            byte sum = 0;
            sendBuffer[0] = (byte)'$';
            int i = 0;
            for ( ; i < str.Length; i++ )
            {
                sendBuffer[i+1] = str[i];
                sum += str[i];
            }

            sendBuffer[i + 1] = (byte)'#';
            sendBuffer[i + 2] = U.ToCharOneHex((byte)((sum >> 4) & 0xf));
            sendBuffer[i + 3] = U.ToCharOneHex((byte)((sum) & 0xf));
            return sendBuffer;
        }


        public static void TEST_GDB_CHECKSUM()
        {
            {
                GDBSocket g = new GDBSocket();
                byte[] str = g.MakePacket("g");
                Debug.Assert(str.Length == 5);
                Debug.Assert(str[0] == '$');
                Debug.Assert(str[1] == 'g');
                Debug.Assert(str[2] == '#');
                Debug.Assert(str[3] == '6');
                Debug.Assert(str[4] == '7');
            }
        }
    }
}
