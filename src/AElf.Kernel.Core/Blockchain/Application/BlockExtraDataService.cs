using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;

namespace AElf.Kernel.Blockchain.Application
{
    public class BlockExtraDataService : IBlockExtraDataService
    {
        private readonly List<IBlockExtraDataProvider> _blockExtraDataProviders;
        
        public BlockExtraDataService(IServiceContainer<IBlockExtraDataProvider> blockExtraDataProviders)
        {
            _blockExtraDataProviders = blockExtraDataProviders.ToList();
        }

        public async Task FillBlockExtraData(BlockHeader blockHeader)
        {
            foreach (var blockExtraDataProvider in _blockExtraDataProviders)
            {
                var extraData = await blockExtraDataProvider.GetExtraDataForFillingBlockHeaderAsync(blockHeader);
                if (extraData != null) 
                {
                    // Actually extraData cannot be NULL if it is mining processing, as the index in BlockExtraData is fixed.
                    // So it can be ByteString.Empty but not NULL.
                    blockHeader.ExtraData.Add(extraData);
                }
            }
        }

        public ByteString GetExtraDataFromBlockHeader(string blockExtraDataProviderSymbol, BlockHeader blockHeader)
        {
            if (blockHeader.Height == AElfConstants.GenesisBlockHeight)
                return null;
            for (var i = 0; i < _blockExtraDataProviders.Count; i++)
            {
                //TODO!! add Name readonly property to IBlockExtraDataProvider
                var blockExtraDataProviderName = _blockExtraDataProviders[i].GetType().Name;
                if (blockExtraDataProviderName.Contains(blockExtraDataProviderSymbol) && i < blockHeader.ExtraData.Count)
                {
                    return blockHeader.ExtraData[i];
                }
            }
            
            return null;
        }
    }
}