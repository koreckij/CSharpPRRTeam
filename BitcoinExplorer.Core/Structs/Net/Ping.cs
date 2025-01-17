﻿using System;
using System.IO;

namespace BitcoinExplorer.Core.Structs.Net
{
    public class Ping : IPayload
    {
        public UInt64 nonce;
        public static Ping FromStream(Stream s)
        {
            Ping x = new Ping();
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