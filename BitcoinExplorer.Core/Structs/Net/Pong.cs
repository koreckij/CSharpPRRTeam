using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BitcoinExplorer.Core.Structs.Net
{
    public class Pong : IPayload
    {
        public UInt64 nonce;

        private Pong()
        {
        }

        public Pong(ulong nonceFromPing)
        {
            this.nonce = nonceFromPing;
        }

        public static Pong FromStream(Stream s)
        {
            var x = new Pong();
            x.Read(s);
            return x;
        }

        public void Read(Stream s)
        {
            var br = new BinaryReader(s);
            nonce = br.ReadUInt64();
        }

        public byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Write(ms);
                return ms.ToArray();
            }
        }

        public void Write(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((UInt64)nonce);
        }
    }
}
