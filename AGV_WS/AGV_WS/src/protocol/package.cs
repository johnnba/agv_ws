using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AGV_WS.src.protocol
{
    class package
    {
    }
}
namespace RemotePLC.src.comm.protocol
{
    /********************************************************************
    帧协议格式：
    ---------------------------------------------------------------------
    |  2B   |   2B   |   8B   |   8B   |  2B  |       n B       |  2B   |
    ---------------------------------------------------------------------
    |  stx  |   len  |  src   |  dst   |  id  |      body       |  end  |
    ---------------------------------------------------------------------

    stx: 0xEFEF
    len: len = src + dst + id + body + end
    src:
    dst:
    id : 消息id
    end: 0xEEEE
    ********************************************************************/
    public class Package
    {
        private byte[] _src;
        private byte[] _dst;
        private UInt16 _id;
        private byte[] _payload;

        public UInt16 Id { get { return _id; } }
        public byte[] Src { get { return _src; } }
        public byte[] Payload { get { return _payload; } }

        private Package(byte[] src, byte[] dst, UInt16 id, byte[] payload)
        {
            _src = src;
            _dst = dst;
            _id = id;
            _payload = payload;
        }

        public static List<Package> decode(byte[] buffer, int length)
        {
            List<Package> packages = new List<Package>();
            byte[] bytes = buffer.Take(length).ToArray();
            MemoryStream ms = new MemoryStream(bytes);
            while (ms.Length - ms.Position >= 2 + 2 + 8 + 8 + 2 + 2)
            {
                if (ms.ReadByte() == 0xef && ms.ReadByte() == 0xef)
                {
                    byte[] len = new byte[2];
                    ms.Read(len, 0, len.Length);
                    //Array.Reverse(len);
                    UInt16 ulen = BitConverter.ToUInt16(len, 0);
                    if (ulen < ms.Length)
                    {
                        byte[] src = new byte[8];
                        ms.Read(src, 0, src.Length);
                        byte[] dst = new byte[8];
                        ms.Read(dst, 0, dst.Length);
                        byte[] id = new byte[2];
                        ms.Read(id, 0, id.Length);
                        //Array.Reverse(id);
                        UInt16 uid = BitConverter.ToUInt16(id, 0);

                        int payloadlen = ulen - 8 - 8 - 2 - 2;
                        byte[] payload = new byte[payloadlen];
                        ms.Read(payload, 0, payload.Length);

                        if (ms.ReadByte() == 0xee && ms.ReadByte() == 0xee)
                        {
                            packages.Add(new Package(src, dst, uid, payload));
                        }
                    }
                }
            }

            return packages;
        }

        public static Package encode(UInt64 agvId, UInt16 msgId, byte[] payload)
        {
            byte[] src = new byte[8];
            byte[] dst = BitConverter.GetBytes(agvId);
            Array.Reverse(dst);

            int len = src.Length + dst.Length + BitConverter.GetBytes(msgId).Length + payload.Length + 2;
            if (len > 0xffff)
            {
                return new Package(src, dst, msgId, new byte[0]);
            }

            return new Package(src, dst, msgId, payload);
        }

        public byte[] ToSocketData()
        {
            byte[] _stx = { 0xEF, 0xEF };
            byte[] _end = { 0xEE, 0xEE };

            MemoryStream ms = new MemoryStream();
            //stx
            ms.Write(_stx, 0, _stx.Length);

            //len
            //len = src + dst + id + payload + end
            byte[] len = BitConverter.GetBytes((UInt16)(_src.Length + _dst.Length + BitConverter.GetBytes(_id).Length + _payload.Length + _end.Length));
            Array.Reverse(len);
            ms.Write(len, 0, len.Length);

            //src
            ms.Write(_src, 0, _src.Length);
            //dst
            ms.Write(_dst, 0, _dst.Length);

            //id
            byte[] msgid = BitConverter.GetBytes(_id);
            Array.Reverse(msgid);
            ms.Write(msgid, 0, msgid.Length);

            //payload
            ms.Write(_payload, 0, _payload.Length);

            //end
            ms.Write(_end, 0, _end.Length);

            return ms.ToArray();
        }

    }
}