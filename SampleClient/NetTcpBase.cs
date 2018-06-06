using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace NetUtil
{
    class NetTcpBase
    {
        private static readonly int RECEIVE_FROM_SERVER_SIZE = 12;
        private static readonly int MESSAGE_LENGTH = 4;
        private static readonly int MESSAGE_TYPE_SIZE = 4;
        private static readonly int MESSAGE_ID_SIZE = 4;
        public static byte[] ReadFromServer(BinaryReader br)
        {
            byte[] sizeByte = new byte[RECEIVE_FROM_SERVER_SIZE];
            sizeByte = br.ReadBytes(RECEIVE_FROM_SERVER_SIZE);
            int size = LittleEndian.LittleEndianToInt32(sizeByte, 0);
            int count = size - 12;
            byte[] b = br.ReadBytes(count);
            byte[] all = new byte[size];
            Array.Copy(sizeByte, 0, all, 0, RECEIVE_FROM_SERVER_SIZE);
            Array.Copy(b, 0, all, RECEIVE_FROM_SERVER_SIZE, count);
            return all;
        }

        public static byte[] PackMessage(int messageType, byte[] messageIdBytes, byte[] packetBytes)
        {
            int size = RECEIVE_FROM_SERVER_SIZE + packetBytes.Length;
            byte[] messageBytes = new byte[size];
            byte[] sizeBytes = LittleEndian.GetLittleEndianBytes(size);

            int i = 0;
            Array.Copy(sizeBytes, 0, messageBytes, i, MESSAGE_LENGTH);
            i += MESSAGE_LENGTH;

            byte[] type = LittleEndian.GetLittleEndianBytes(messageType);

            Array.Copy(type, 0, messageBytes, i, MESSAGE_TYPE_SIZE);
            i += MESSAGE_TYPE_SIZE;

            Array.Copy(messageIdBytes, 0, messageBytes, i, MESSAGE_ID_SIZE);
            i += MESSAGE_ID_SIZE;

            Array.Copy(packetBytes, 0, messageBytes, i, packetBytes.Length);

            return messageBytes;
        }
    }
}
