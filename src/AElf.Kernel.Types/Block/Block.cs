﻿using Google.Protobuf;

namespace AElf.Kernel
{
    public partial class Block : ICustomDiagnosticMessage, IBlock
    {
        /// <summary>
        /// Used to override IMessage's default string representation.
        /// </summary>
        /// <returns></returns>
        public string ToDiagnosticString()
        {
            return $"{{ id: {GetHash()}, height: {Height} }}";
        }

        // TODO: check for "new"
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AElf.Kernel.Block"/> class.
        /// A previous block must be referred, except for the genesis block.
        /// </summary>
        /// <param name="preBlockHash">Pre block hash.</param>
        public Block(Hash preBlockHash)
        {
            Header = new BlockHeader(preBlockHash);
            Body = new BlockBody();
        }

        public long Height
        {
            get => Header?.Height ?? 0;
        }

        public Hash GetHash()
        {
            return Header.GetHash();
        }

        // TODO: remove
        public Hash GetHashWithoutCache()
        {
            return Header.GetHashWithoutCache();
        }

        // TODO: remove
        public byte[] GetHashBytes()
        {
            return Header.GetHashBytes();
        }

        // TODO: remove
        public byte[] Serialize()
        {
            return this.ToByteArray();
        }
    }
}